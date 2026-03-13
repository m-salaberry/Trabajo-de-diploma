using DAL.Contracts;
using DAL.Helpers;
using Domain;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL.Implementations
{
    public class ProductRepository : IRepository<Product, int>
    {
        /// <summary>
        /// Inserts a new Product into the database and sets the generated Id.
        /// </summary>
        public void Create(Product entity)
        {
            string command = @"
                INSERT INTO PRODUCTS (Code, Name, CreatedDate)
                OUTPUT INSERTED.Id
                VALUES (@Code, @Name, GETDATE())";

            var parameters = new[]
            {
                new SqlParameter("@Code", entity.Code),
                new SqlParameter("@Name", entity.Name)
            };

            var result = SqlHelper.ExecuteScalar(command, CommandType.Text, parameters);
            if (result != null)
            {
                typeof(Product).GetProperty("Id")?.SetValue(entity, Convert.ToInt32(result));
            }

            if (entity.DetailProducts != null)
            {
                foreach (var detail in entity.DetailProducts)
                    InsertDetail(entity.Id, detail);
            }
        }

        /// <summary>
        /// Updates an existing Product in the database.
        /// </summary>
        public void Update(Product entity)
        {
            string command = @"
                UPDATE PRODUCTS
                SET Code = @Code, Name = @Name, ModifiedDate = GETDATE()
                WHERE Id = @Id";

            var parameters = new[]
            {
                new SqlParameter("@Id", entity.Id),
                new SqlParameter("@Code", entity.Code),
                new SqlParameter("@Name", entity.Name)
            };

            SqlHelper.ExecuteNonQuery(command, CommandType.Text, parameters);

            // Replace all details: delete old ones and re-insert current list
            DeleteDetailsByProductId(entity.Id);

            if (entity.DetailProducts != null)
            {
                foreach (var detail in entity.DetailProducts)
                    InsertDetail(entity.Id, detail);
            }
        }

        /// <summary>
        /// Deletes a Product from the database by its Id.
        /// </summary>
        public void Delete(Product entity)
        {
            // ON DELETE CASCADE in PRODUCT_DETAILS handles child rows automatically
            string command = "DELETE FROM PRODUCTS WHERE Id = @Id";
            SqlHelper.ExecuteNonQuery(command, CommandType.Text, new SqlParameter("@Id", entity.Id));
        }

        /// <summary>
        /// Retrieves a single Product by its unique identifier.
        /// </summary>
        public Product GetById(int id)
        {
            string command = @"
                SELECT p.Id, p.Code, p.Name, p.CreatedDate, p.ModifiedDate
                FROM PRODUCTS p
                WHERE p.Id = @Id";

            var parameters = new[] { new SqlParameter("@Id", id) };

            using (SqlDataReader reader = SqlHelper.ExecuteReader(command, CommandType.Text, parameters))
            {
                if (reader != null && reader.Read())
                {
                    var product = MapToEntity(reader);
                    product.DetailProducts = LoadDetails(product.Id);
                    return product;
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves all Products from the database with their associated details.
        /// </summary>
        public IEnumerable<Product> GetAll()
        {
            string command = @"
                SELECT p.Id, p.Code, p.Name, p.CreatedDate, p.ModifiedDate
                FROM PRODUCTS p
                ORDER BY p.Name";

            var products = new List<Product>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(command, CommandType.Text))
            {
                if (reader != null)
                {
                    while (reader.Read())
                        products.Add(MapToEntity(reader));
                }
            }

            foreach (var product in products)
                product.DetailProducts = LoadDetails(product.Id);

            return products;
        }

        /// <summary>
        /// Maps a SqlDataReader row to a Product entity.
        /// </summary>
        private static Product MapToEntity(SqlDataReader reader)
        {
            var product = (Product)Activator.CreateInstance(typeof(Product), true);
            typeof(Product).GetProperty("Id")?.SetValue(product, reader.GetInt32(reader.GetOrdinal("Id")));
            product.Code = reader.GetInt32(reader.GetOrdinal("Code"));
            product.Name = reader.GetString(reader.GetOrdinal("Name"));
            return product;
        }

        /// <summary>
        /// Loads all DetailProduct entries for a given product.
        /// </summary>
        private List<DetailProduct> LoadDetails(int productId)
        {
            string command = @"
                SELECT pd.Id AS DetailId, pd.QuantityToConsume,
                       i.Id AS ItemId, i.Name AS ItemName, i.Unit, i.IntegerUnit, i.CurrentStock,
                       c.Id AS CategoryId, c.Name AS CategoryName
                FROM PRODUCT_DETAILS pd
                INNER JOIN ITEMS i ON pd.ItemId = i.Id
                INNER JOIN ITEMS_CATEGORY c ON i.ItemsCategoryId = c.Id
                WHERE pd.ProductId = @ProductId";

            var parameters = new[] { new SqlParameter("@ProductId", productId) };
            var details = new List<DetailProduct>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(command, CommandType.Text, parameters))
            {
                if (reader != null)
                {
                    while (reader.Read())
                        details.Add(MapDetailToEntity(reader));
                }
            }

            return details;
        }

        /// <summary>
        /// Inserts a single product detail row.
        /// </summary>
        private void InsertDetail(int productId, DetailProduct detail)
        {
            string command = @"
                INSERT INTO PRODUCT_DETAILS (ProductId, ItemId, QuantityToConsume)
                VALUES (@ProductId, @ItemId, @QuantityToConsume)";

            var parameters = new[]
            {
                new SqlParameter("@ProductId", productId),
                new SqlParameter("@ItemId", detail.Item.Id),
                new SqlParameter("@QuantityToConsume", detail.QuantityToConsume)
            };

            SqlHelper.ExecuteNonQuery(command, CommandType.Text, parameters);
        }

        /// <summary>
        /// Deletes all detail rows for a given product.
        /// </summary>
        private void DeleteDetailsByProductId(int productId)
        {
            string command = "DELETE FROM PRODUCT_DETAILS WHERE ProductId = @ProductId";
            SqlHelper.ExecuteNonQuery(command, CommandType.Text, new SqlParameter("@ProductId", productId));
        }

        /// <summary>
        /// Shared mapper used by DetailProductRepository to avoid duplication.
        /// </summary>
        internal static DetailProduct MapDetailToEntity(SqlDataReader reader)
        {
            var item = (Item)Activator.CreateInstance(typeof(Item), true);
            typeof(Item).GetProperty("Id")?.SetValue(item, reader.GetGuid(reader.GetOrdinal("ItemId")));
            item.Name = reader.GetString(reader.GetOrdinal("ItemName"));
            item.Unit = new Dictionary<string, object>
            {
                { "Name", reader.IsDBNull(reader.GetOrdinal("Unit")) ? string.Empty : reader.GetString(reader.GetOrdinal("Unit")) },
                { "IsInteger", reader.GetBoolean(reader.GetOrdinal("IntegerUnit")) }
            };
            item.Stock = reader.GetDecimal(reader.GetOrdinal("CurrentStock"));
            item.Category = new ItemsCategory
            {
                Id = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                Name = reader.GetString(reader.GetOrdinal("CategoryName"))
            };

            var detail = (DetailProduct)Activator.CreateInstance(typeof(DetailProduct), true);
            typeof(DetailProduct).GetProperty("Id")?.SetValue(detail, reader.GetInt32(reader.GetOrdinal("DetailId")));
            detail.Item = item;
            detail.QuantityToConsume = reader.GetDecimal(reader.GetOrdinal("QuantityToConsume"));

            return detail;
        }
    }
}
