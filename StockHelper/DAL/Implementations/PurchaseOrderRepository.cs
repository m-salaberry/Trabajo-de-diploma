using DAL.Contracts;
using DAL.Helpers;
using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DAL.Implementations
{
    public class PurchaseOrderRepository : IRepository<PurchaseOrder, Guid>
    {
        private readonly ReplacementOrderRepository _replacementOrderRepo = new ReplacementOrderRepository();

        /// <summary>
        /// Inserts a new PurchaseOrder into the database and sets the generated Id.
        /// </summary>
        public void Create(PurchaseOrder entity)
        {
            string command = @"
                INSERT INTO PURCHASE_ORDERS (ReplacementOrderId, Status, BillFilePath, TotalAmount, IssuedDate)
                OUTPUT INSERTED.Id
                VALUES (@ReplacementOrderId, @Status, @BillFilePath, @TotalAmount, @IssuedDate)";

            var parameters = new[]
            {
                new SqlParameter("@ReplacementOrderId", entity.ReplacementOrder.Id),
                new SqlParameter("@Status", entity.Status),
                new SqlParameter("@BillFilePath", (object)entity.BillFilePath ?? DBNull.Value),
                new SqlParameter("@TotalAmount", entity.TotalAmount),
                new SqlParameter("@IssuedDate", entity.IssuedDate)
            };

            var result = SqlHelper.ExecuteScalar(command, CommandType.Text, parameters);
            if (result != null)
            {
                typeof(PurchaseOrder).GetProperty("Id")?.SetValue(entity, (Guid)result);
            }
        }

        /// <summary>
        /// Deletes a PurchaseOrder from the database by its Id.
        /// </summary>
        public void Delete(PurchaseOrder entity)
        {
            string command = "DELETE FROM PURCHASE_ORDERS WHERE Id = @Id";
            var parameters = new[] { new SqlParameter("@Id", entity.Id) };
            SqlHelper.ExecuteNonQuery(command, CommandType.Text, parameters);
        }

        /// <summary>
        /// Retrieves all PurchaseOrders from the database with their associated ReplacementOrders and Providers.
        /// </summary>
        public IEnumerable<PurchaseOrder> GetAll()
        {
            string command = @"
                SELECT po.Id, po.Status, po.BillFilePath, po.TotalAmount, po.IssuedDate,
                       ro.Id AS ReplacementOrderId, ro.ReplacementOrderNumber,
                       p.Id AS ProviderId, p.Name AS ProviderName, p.CUIT,
                       p.CompanyName, p.ContactTel, p.Email,
                       c.Id AS CategoryId, c.Name AS CategoryName
                FROM PURCHASE_ORDERS po
                INNER JOIN REPLACEMENT_ORDERS ro ON po.ReplacementOrderId = ro.Id
                INNER JOIN PROVIDERS p ON ro.ProviderId = p.Id
                INNER JOIN ITEMS_CATEGORY c ON p.ItemsCategoryId = c.Id";

            var orders = new List<PurchaseOrder>();

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

            foreach (var po in orders)
                po.ReplacementOrder.OrderRows = _replacementOrderRepo.LoadOrderRows(po.ReplacementOrder.Id);

            return orders;
        }

        /// <summary>
        /// Retrieves a single PurchaseOrder by its unique identifier.
        /// </summary>
        public PurchaseOrder GetById(Guid id)
        {
            string command = @"
                SELECT po.Id, po.Status, po.BillFilePath, po.TotalAmount, po.IssuedDate,
                       ro.Id AS ReplacementOrderId, ro.ReplacementOrderNumber,
                       p.Id AS ProviderId, p.Name AS ProviderName, p.CUIT,
                       p.CompanyName, p.ContactTel, p.Email,
                       c.Id AS CategoryId, c.Name AS CategoryName
                FROM PURCHASE_ORDERS po
                INNER JOIN REPLACEMENT_ORDERS ro ON po.ReplacementOrderId = ro.Id
                INNER JOIN PROVIDERS p ON ro.ProviderId = p.Id
                INNER JOIN ITEMS_CATEGORY c ON p.ItemsCategoryId = c.Id
                WHERE po.Id = @Id";

            var parameters = new[] { new SqlParameter("@Id", id) };

            using (SqlDataReader reader = SqlHelper.ExecuteReader(command, CommandType.Text, parameters))
            {
                if (reader != null && reader.Read())
                {
                    var po = MapToEntity(reader);
                    po.ReplacementOrder.OrderRows = _replacementOrderRepo.LoadOrderRows(po.ReplacementOrder.Id);
                    return po;
                }
            }
            return null;
        }

        /// <summary>
        /// Updates an existing PurchaseOrder in the database.
        /// </summary>
        public void Update(PurchaseOrder entity)
        {
            string command = @"UPDATE PURCHASE_ORDERS
                SET ReplacementOrderId = @ReplacementOrderId,
                    Status = @Status,
                    BillFilePath = @BillFilePath,
                    TotalAmount = @TotalAmount,
                    IssuedDate = @IssuedDate
                WHERE Id = @Id";

            var parameters = new[]
            {
                new SqlParameter("@Id", entity.Id),
                new SqlParameter("@ReplacementOrderId", entity.ReplacementOrder.Id),
                new SqlParameter("@Status", entity.Status),
                new SqlParameter("@BillFilePath", (object)entity.BillFilePath ?? DBNull.Value),
                new SqlParameter("@TotalAmount", entity.TotalAmount),
                new SqlParameter("@IssuedDate", entity.IssuedDate)
            };

            SqlHelper.ExecuteNonQuery(command, CommandType.Text, parameters);
        }

        /// <summary>
        /// Maps a SqlDataReader row to a PurchaseOrder entity with its ReplacementOrder and Provider.
        /// </summary>
        private PurchaseOrder MapToEntity(SqlDataReader reader)
        {
            // Map Provider
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

            // Map ReplacementOrder
            var replacementOrder = new ReplacementOrder(provider);
            typeof(ReplacementOrder).GetProperty("Id")?.SetValue(replacementOrder, reader.GetGuid(reader.GetOrdinal("ReplacementOrderId")));
            replacementOrder.ReplacementOrderNumber = reader.GetString(reader.GetOrdinal("ReplacementOrderNumber"));

            // Map PurchaseOrder
            var purchaseOrder = (PurchaseOrder)Activator.CreateInstance(typeof(PurchaseOrder), true);
            typeof(PurchaseOrder).GetProperty("Id")?.SetValue(purchaseOrder, reader.GetGuid(reader.GetOrdinal("Id")));
            purchaseOrder.ReplacementOrder = replacementOrder;
            purchaseOrder.Status = reader.GetString(reader.GetOrdinal("Status"));
            purchaseOrder.BillFilePath = reader.IsDBNull(reader.GetOrdinal("BillFilePath")) ? null : reader.GetString(reader.GetOrdinal("BillFilePath"));
            purchaseOrder.TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount"));
            purchaseOrder.IssuedDate = reader.GetDateTime(reader.GetOrdinal("IssuedDate"));

            return purchaseOrder;
        }
    }
}
