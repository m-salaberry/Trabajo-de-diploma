using DAL.Contracts;
using DAL.Helpers;
using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DAL.Implementations
{
    public class ProviderRepository : IRepository<Provider, Guid>
    {
        public void Create(Provider entity)
        {
            string command = @"
                INSERT INTO PROVIDERS (Name, CUIT, CompanyName, ContactTel, Email, ItemsCategoryId)
                OUTPUT INSERTED.Id
                VALUES (@Name, @CUIT, @CompanyName, @ContactTel, @Email, @ItemsCategoryId)";

            var parameters = new[]
            {
                new SqlParameter("@Name", entity.Name),
                new SqlParameter("@CUIT", entity.CUIT),
                new SqlParameter("@CompanyName", (object)entity.CompanyName ?? DBNull.Value),
                new SqlParameter("@ContactTel", (object)entity.ContactTel ?? DBNull.Value),
                new SqlParameter("@Email", (object)entity.Email ?? DBNull.Value),
                new SqlParameter("@ItemsCategoryId", entity.Category.Id)
            };

            var result = SqlHelper.ExecuteScalar(command, CommandType.Text, parameters);
            if (result != null)
            {
                typeof(Provider).GetProperty("Id")?.SetValue(entity, (Guid)result);
            }
        }

        public void Update(Provider entity)
        {
            string command = @"UPDATE PROVIDERS 
                SET Name = @Name, CUIT = @CUIT, CompanyName = @CompanyName, 
                    ContactTel = @ContactTel, Email = @Email, ItemsCategoryId = @ItemsCategoryId,
                    ModifiedDate = GETDATE()
                WHERE Id = @Id";

            var parameters = new[]
            {
                new SqlParameter("@Id", entity.Id),
                new SqlParameter("@Name", entity.Name),
                new SqlParameter("@CUIT", entity.CUIT),
                new SqlParameter("@CompanyName", (object)entity.CompanyName ?? DBNull.Value),
                new SqlParameter("@ContactTel", (object)entity.ContactTel ?? DBNull.Value),
                new SqlParameter("@Email", (object)entity.Email ?? DBNull.Value),
                new SqlParameter("@ItemsCategoryId", entity.Category.Id)
            };

            SqlHelper.ExecuteNonQuery(command, CommandType.Text, parameters);
        }

        public void Delete(Provider entity)
        {
            string command = "DELETE FROM PROVIDERS WHERE Id = @Id";
            var parameters = new[] { new SqlParameter("@Id", entity.Id) };
            SqlHelper.ExecuteNonQuery(command, CommandType.Text, parameters);
        }

        public Provider GetById(Guid id)
        {
            string command = @"
                SELECT p.Id, p.Name, p.CUIT, p.CompanyName, p.ContactTel, p.Email,
                       c.Id AS CategoryId, c.Name AS CategoryName
                FROM PROVIDERS p
                INNER JOIN ITEMS_CATEGORY c ON p.ItemsCategoryId = c.Id
                WHERE p.Id = @Id";

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

        public IEnumerable<Provider> GetAll()
        {
            string command = @"
                SELECT p.Id, p.Name, p.CUIT, p.CompanyName, p.ContactTel, p.Email,
                       c.Id AS CategoryId, c.Name AS CategoryName
                FROM PROVIDERS p
                INNER JOIN ITEMS_CATEGORY c ON p.ItemsCategoryId = c.Id";

            var providers = new List<Provider>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(command, CommandType.Text))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        providers.Add(MapToEntity(reader));
                    }
                }
            }
            return providers;
        }

        private Provider MapToEntity(SqlDataReader reader)
        {
            var provider = (Provider)Activator.CreateInstance(typeof(Provider), true);

            typeof(Provider).GetProperty("Id")?.SetValue(provider, reader.GetGuid(reader.GetOrdinal("Id")));

            provider.CUIT = reader.GetString(reader.GetOrdinal("CUIT"));
            provider.Name = reader.GetString(reader.GetOrdinal("Name"));
            provider.CompanyName = reader.IsDBNull(reader.GetOrdinal("CompanyName")) ? null : reader.GetString(reader.GetOrdinal("CompanyName"));
            provider.ContactTel = reader.IsDBNull(reader.GetOrdinal("ContactTel")) ? null : reader.GetString(reader.GetOrdinal("ContactTel"));
            provider.Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email"));

            provider.Category = new ItemsCategory
            {
                Id = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                Name = reader.GetString(reader.GetOrdinal("CategoryName"))
            };

            return provider;
        }
    }
}

