using DAL.Contracts;
using DAL.Helpers;
using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DAL.Implementations
{
    public class OrderRowRepository : IRepository<OrderRow, Guid>
    {
        public void Create(OrderRow entity)
        {
            string command = @"
                INSERT INTO ORDER_ROWS (ReplacementOrderId, ItemId, Quantity)
                OUTPUT INSERTED.Id
                VALUES (@ReplacementOrderId, @ItemId, @Quantity)";

            var parameters = new[]
            {
                new SqlParameter("@ReplacementOrderId", entity.ReplacementOrder.Id),
                new SqlParameter("@ItemId", entity.Item.Id),
                new SqlParameter("@Quantity", entity.Quantity)
            };

            var result = SqlHelper.ExecuteScalar(command, CommandType.Text, parameters);
            if (result != null)
            {
                typeof(OrderRow).GetProperty("Id")?.SetValue(entity, (Guid)result);
            }
        }

        public void Delete(OrderRow entity)
        {
            string command = "DELETE FROM ORDER_ROWS WHERE Id = @Id";
            var parameters = new[] { new SqlParameter("@Id", entity.Id) };
            SqlHelper.ExecuteNonQuery(command, CommandType.Text, parameters);
        }

        public IEnumerable<OrderRow> GetAll()
        {
            string command = @"
                SELECT r.Id, r.ReplacementOrderId, r.Quantity,
                       i.Id AS ItemId, i.Name AS ItemName, i.Unit, i.IntegerUnit, i.CurrentStock,
                       c.Id AS CategoryId, c.Name AS CategoryName
                FROM ORDER_ROWS r
                INNER JOIN ITEMS i ON r.ItemId = i.Id
                INNER JOIN ITEMS_CATEGORY c ON i.ItemsCategoryId = c.Id";

            var rows = new List<OrderRow>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(command, CommandType.Text))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        rows.Add(MapToEntity(reader));
                    }
                }
            }
            return rows;
        }

        public OrderRow GetById(Guid id)
        {
            string command = @"
                SELECT r.Id, r.ReplacementOrderId, r.Quantity,
                       i.Id AS ItemId, i.Name AS ItemName, i.Unit, i.IntegerUnit, i.CurrentStock,
                       c.Id AS CategoryId, c.Name AS CategoryName
                FROM ORDER_ROWS r
                INNER JOIN ITEMS i ON r.ItemId = i.Id
                INNER JOIN ITEMS_CATEGORY c ON i.ItemsCategoryId = c.Id
                WHERE r.Id = @Id";

            var parameters = new[] { new SqlParameter("@Id", id) };

            using (SqlDataReader reader = SqlHelper.ExecuteReader(command, CommandType.Text, parameters))
            {
                if (reader != null && reader.Read())
                {
                    return MapToEntity(reader);
                }
            }
            return null;
        }

        public void Update(OrderRow entity)
        {
            string command = @"UPDATE ORDER_ROWS
                SET ItemId = @ItemId, Quantity = @Quantity
                WHERE Id = @Id";

            var parameters = new[]
            {
                new SqlParameter("@Id", entity.Id),
                new SqlParameter("@ItemId", entity.Item.Id),
                new SqlParameter("@Quantity", entity.Quantity)
            };

            SqlHelper.ExecuteNonQuery(command, CommandType.Text, parameters);
        }

        /// <summary>
        /// Gets all order rows belonging to a specific replacement order.
        /// </summary>
        public IEnumerable<OrderRow> GetByReplacementOrderId(Guid replacementOrderId)
        {
            string command = @"
                SELECT r.Id, r.ReplacementOrderId, r.Quantity,
                       i.Id AS ItemId, i.Name AS ItemName, i.Unit, i.IntegerUnit, i.CurrentStock,
                       c.Id AS CategoryId, c.Name AS CategoryName
                FROM ORDER_ROWS r
                INNER JOIN ITEMS i ON r.ItemId = i.Id
                INNER JOIN ITEMS_CATEGORY c ON i.ItemsCategoryId = c.Id
                WHERE r.ReplacementOrderId = @ReplacementOrderId";

            var parameters = new[] { new SqlParameter("@ReplacementOrderId", replacementOrderId) };
            var rows = new List<OrderRow>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(command, CommandType.Text, parameters))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        rows.Add(MapToEntity(reader));
                    }
                }
            }
            return rows;
        }

        private OrderRow MapToEntity(SqlDataReader reader)
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

            var orderRow = (OrderRow)Activator.CreateInstance(typeof(OrderRow), true);
            typeof(OrderRow).GetProperty("Id")?.SetValue(orderRow, reader.GetGuid(reader.GetOrdinal("Id")));
            orderRow.Item = item;
            orderRow.Quantity = reader.GetDecimal(reader.GetOrdinal("Quantity"));

            return orderRow;
        }
    }
}
