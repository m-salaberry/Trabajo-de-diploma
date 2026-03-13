using DAL.Contracts;
using DAL.Helpers;
using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DAL.Implementations
{
    public class ReplacementOrderRepository : IRepository<ReplacementOrder, Guid>
    {
        public void Create(ReplacementOrder entity)
        {
            string command = @"
                INSERT INTO REPLACEMENT_ORDERS (ReplacementOrderNumber, ProviderId)
                OUTPUT INSERTED.Id
                VALUES (@ReplacementOrderNumber, @ProviderId)";

            var parameters = new[]
            {
                new SqlParameter("@ReplacementOrderNumber", entity.ReplacementOrderNumber),
                new SqlParameter("@ProviderId", entity.Provider.Id)
            };

            var result = SqlHelper.ExecuteScalar(command, CommandType.Text, parameters);
            if (result != null)
            {
                typeof(ReplacementOrder).GetProperty("Id")?.SetValue(entity, (Guid)result);
            }
        }

        public void Delete(ReplacementOrder entity)
        {
            string command = "DELETE FROM REPLACEMENT_ORDERS WHERE Id = @Id";
            var parameters = new[] { new SqlParameter("@Id", entity.Id) };
            SqlHelper.ExecuteNonQuery(command, CommandType.Text, parameters);
        }

        public IEnumerable<ReplacementOrder> GetAll()
        {
            string command = @"
                SELECT ro.Id, ro.ReplacementOrderNumber,
                       p.Id AS ProviderId, p.Name AS ProviderName, p.CUIT,
                       p.CompanyName, p.ContactTel, p.Email,
                       c.Id AS CategoryId, c.Name AS CategoryName
                FROM REPLACEMENT_ORDERS ro
                INNER JOIN PROVIDERS p ON ro.ProviderId = p.Id
                INNER JOIN ITEMS_CATEGORY c ON p.ItemsCategoryId = c.Id";

            var orders = new List<ReplacementOrder>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(command, CommandType.Text))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        orders.Add(MapToEntity(reader));
                    }
                }
            }

            foreach (var order in orders)
                order.OrderRows = LoadOrderRows(order.Id);

            return orders;
        }

        public ReplacementOrder GetById(Guid id)
        {
            string command = @"
                SELECT ro.Id, ro.ReplacementOrderNumber,
                       p.Id AS ProviderId, p.Name AS ProviderName, p.CUIT,
                       p.CompanyName, p.ContactTel, p.Email,
                       c.Id AS CategoryId, c.Name AS CategoryName
                FROM REPLACEMENT_ORDERS ro
                INNER JOIN PROVIDERS p ON ro.ProviderId = p.Id
                INNER JOIN ITEMS_CATEGORY c ON p.ItemsCategoryId = c.Id
                WHERE ro.Id = @Id";

            var parameters = new[] { new SqlParameter("@Id", id) };

            using (SqlDataReader reader = SqlHelper.ExecuteReader(command, CommandType.Text, parameters))
            {
                if (reader != null && reader.Read())
                {
                    var order = MapToEntity(reader);
                    order.OrderRows = LoadOrderRows(order.Id);
                    return order;
                }
            }
            return null;
        }

        public void Update(ReplacementOrder entity)
        {
            string command = @"UPDATE REPLACEMENT_ORDERS
                SET ReplacementOrderNumber = @ReplacementOrderNumber,
                    ProviderId = @ProviderId
                WHERE Id = @Id";

            var parameters = new[]
            {
                new SqlParameter("@Id", entity.Id),
                new SqlParameter("@ReplacementOrderNumber", entity.ReplacementOrderNumber),
                new SqlParameter("@ProviderId", entity.Provider.Id)
            };

            SqlHelper.ExecuteNonQuery(command, CommandType.Text, parameters);
        }

        /// <summary>
        /// Gets the next sequential number for a provider's replacement orders in the current month.
        /// Used to generate the ReplacementOrderNumber with format REP-YYYYMM-CUITBODY-XXX.
        /// </summary>
        public int GetNextSequentialNumber(Guid providerId)
        {
            string prefix = $"REP-{DateTime.Now:yyyyMM}";

            string command = @"
                SELECT COUNT(*) + 1
                FROM REPLACEMENT_ORDERS
                WHERE ProviderId = @ProviderId
                  AND ReplacementOrderNumber LIKE @Prefix + '%'";

            var parameters = new[]
            {
                new SqlParameter("@ProviderId", providerId),
                new SqlParameter("@Prefix", prefix)
            };

            var result = SqlHelper.ExecuteScalar(command, CommandType.Text, parameters);
            return result != null ? Convert.ToInt32(result) : 1;
        }

        internal List<OrderRow> LoadOrderRows(Guid replacementOrderId)
        {
            string command = @"
                SELECT r.Id, r.Quantity,
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

                        rows.Add(orderRow);
                    }
                }
            }

            return rows;
        }

        private ReplacementOrder MapToEntity(SqlDataReader reader)
        {
            var provider = (Provider)Activator.CreateInstance(typeof(Provider), true);
            typeof(Provider).GetProperty("Id")?.SetValue(provider, reader.GetGuid(reader.GetOrdinal("ProviderId")));
            provider.Name = reader.GetString(reader.GetOrdinal("ProviderName"));
            provider.CUIT = reader.GetString(reader.GetOrdinal("CUIT"));
            provider.CompanyName = reader.IsDBNull(reader.GetOrdinal("CompanyName")) ? null : reader.GetString(reader.GetOrdinal("CompanyName"));
            provider.ContactTel = reader.IsDBNull(reader.GetOrdinal("ContactTel")) ? null : reader.GetString(reader.GetOrdinal("ContactTel"));
            provider.Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email"));
            provider.Category = new ItemsCategory
            {
                Id = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                Name = reader.GetString(reader.GetOrdinal("CategoryName"))
            };

            var order = new ReplacementOrder(provider);
            typeof(ReplacementOrder).GetProperty("Id")?.SetValue(order, reader.GetGuid(reader.GetOrdinal("Id")));
            order.ReplacementOrderNumber = reader.GetString(reader.GetOrdinal("ReplacementOrderNumber"));

            return order;
        }
    }
}
