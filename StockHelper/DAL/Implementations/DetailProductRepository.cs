using DAL.Contracts;
using DAL.Helpers;
using Domain;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL.Implementations
{
    public class DetailProductRepository : IRepository<DetailProduct, int>
    {
        // =====================================================================
        // IRepository interface methods
        // Note: Create, Update, Delete require ProductId context not available
        // on the DetailProduct domain entity. Use the extra methods below.
        // =====================================================================

        /// <summary>
        /// Not supported. Use CreateForProduct instead.
        /// </summary>
        public void Create(DetailProduct entity)
        {
            throw new NotSupportedException(
                "Cannot insert a DetailProduct without a parent ProductId. " +
                "Use CreateForProduct(int productId, DetailProduct entity) instead.");
        }

        /// <summary>
        /// Not supported. Use ProductRepository.Update to replace the full detail list.
        /// </summary>
        public void Update(DetailProduct entity)
        {
            throw new NotSupportedException(
                "Updating individual details is not supported. " +
                "Use ProductRepository.Update to replace the full detail list.");
        }

        /// <summary>
        /// Not supported. Use DeleteByProductId to remove all details of a product.
        /// </summary>
        public void Delete(DetailProduct entity)
        {
            throw new NotSupportedException(
                "Cannot delete a DetailProduct without its row Id. " +
                "Use DeleteByProductId(int productId) to remove all details of a product.");
        }

        /// <summary>
        /// Retrieves a single DetailProduct by its unique identifier.
        /// </summary>
        public DetailProduct GetById(int id)
        {
            string command = @"
                SELECT pd.Id AS DetailId, pd.QuantityToConsume,
                       i.Id AS ItemId, i.Name AS ItemName, i.Unit, i.IntegerUnit, i.CurrentStock,
                       c.Id AS CategoryId, c.Name AS CategoryName
                FROM PRODUCT_DETAILS pd
                INNER JOIN ITEMS i ON pd.ItemId = i.Id
                INNER JOIN ITEMS_CATEGORY c ON i.ItemsCategoryId = c.Id
                WHERE pd.Id = @Id";

            var parameters = new[] { new SqlParameter("@Id", id) };

            using (SqlDataReader reader = SqlHelper.ExecuteReader(command, CommandType.Text, parameters))
            {
                if (reader != null && reader.Read())
                    return ProductRepository.MapDetailToEntity(reader);
            }
            return null;
        }

        /// <summary>
        /// Retrieves all DetailProduct entries from the database.
        /// </summary>
        public IEnumerable<DetailProduct> GetAll()
        {
            string command = @"
                SELECT pd.Id AS DetailId, pd.QuantityToConsume,
                       i.Id AS ItemId, i.Name AS ItemName, i.Unit, i.IntegerUnit, i.CurrentStock,
                       c.Id AS CategoryId, c.Name AS CategoryName
                FROM PRODUCT_DETAILS pd
                INNER JOIN ITEMS i ON pd.ItemId = i.Id
                INNER JOIN ITEMS_CATEGORY c ON i.ItemsCategoryId = c.Id";

            var details = new List<DetailProduct>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(command, CommandType.Text))
            {
                if (reader != null)
                {
                    while (reader.Read())
                        details.Add(ProductRepository.MapDetailToEntity(reader));
                }
            }

            return details;
        }

        // =====================================================================
        // Extra methods used by ProductService for granular detail management
        // =====================================================================

        /// <summary>
        /// Inserts a single DetailProduct row linked to the given product.
        /// </summary>
        public void CreateForProduct(int productId, DetailProduct entity)
        {
            string command = @"
                INSERT INTO PRODUCT_DETAILS (ProductId, ItemId, QuantityToConsume)
                VALUES (@ProductId, @ItemId, @QuantityToConsume)";

            var parameters = new[]
            {
                new SqlParameter("@ProductId", productId),
                new SqlParameter("@ItemId", entity.Item.Id),
                new SqlParameter("@QuantityToConsume", entity.QuantityToConsume)
            };

            SqlHelper.ExecuteNonQuery(command, CommandType.Text, parameters);
        }

        /// <summary>
        /// Returns all details associated with the given product.
        /// </summary>
        public List<DetailProduct> GetByProductId(int productId)
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
                        details.Add(ProductRepository.MapDetailToEntity(reader));
                }
            }

            return details;
        }

        /// <summary>
        /// Deletes all details belonging to the given product.
        /// </summary>
        public void DeleteByProductId(int productId)
        {
            string command = "DELETE FROM PRODUCT_DETAILS WHERE ProductId = @ProductId";
            SqlHelper.ExecuteNonQuery(command, CommandType.Text, new SqlParameter("@ProductId", productId));
        }
    }
}
