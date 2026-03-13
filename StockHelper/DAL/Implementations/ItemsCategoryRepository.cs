using DAL.Contracts;
using DAL.Helpers;
using Domain;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL.Implementations
{
    public class ItemsCategoryRepository : IRepository<ItemsCategory, int>
    {
        /// <summary>
        /// Inserts a new ItemsCategory into the database and sets the generated Id.
        /// </summary>
        public void Create(ItemsCategory entity)
        {
            string command = "INSERT INTO ITEMS_CATEGORY (Name) VALUES (@Name); SELECT CAST(SCOPE_IDENTITY() AS INT)";
            var parameters = new[]
            {
                new SqlParameter("@Name", entity.Name)
            };
            
            var result = SqlHelper.ExecuteScalar(command, CommandType.Text, parameters);
            if (result != null)
            {
                entity.Id = Convert.ToInt32(result);
            }
        }

        /// <summary>
        /// Updates an existing ItemsCategory in the database.
        /// </summary>
        public void Update(ItemsCategory entity)
        {
            string command = "UPDATE ITEMS_CATEGORY SET Name = @Name WHERE Id = @Id";
            var parameters = new[]
            {
                new SqlParameter("@Id", entity.Id),
                new SqlParameter("@Name", entity.Name)
            };
            SqlHelper.ExecuteNonQuery(command, CommandType.Text, parameters);
        }

        /// <summary>
        /// Deletes an ItemsCategory from the database by its Id.
        /// </summary>
        public void Delete(ItemsCategory entity)
        {
            string command = "DELETE FROM ITEMS_CATEGORY WHERE Id = @Id";
            var parameters = new[] { new SqlParameter("@Id", entity.Id) };
            SqlHelper.ExecuteNonQuery(command, CommandType.Text, parameters);
        }

        /// <summary>
        /// Retrieves a single ItemsCategory by its unique identifier.
        /// </summary>
        public ItemsCategory GetById(int id)
        {
            string command = "SELECT Id, Name FROM ITEMS_CATEGORY WHERE Id = @Id";
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

        /// <summary>
        /// Retrieves all ItemsCategory entries from the database.
        /// </summary>
        public IEnumerable<ItemsCategory> GetAll()
        {
            string command = "SELECT Id, Name FROM ITEMS_CATEGORY";
            var categories = new List<ItemsCategory>();
            
            using (SqlDataReader reader = SqlHelper.ExecuteReader(command, CommandType.Text))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        categories.Add(MapToEntity(reader));
                    }
                }
            }
            return categories;
        }

        /// <summary>
        /// Maps a SqlDataReader row to an ItemsCategory entity.
        /// </summary>
        private ItemsCategory MapToEntity(SqlDataReader reader)
        {
            return new ItemsCategory
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name"))
            };
        }
    }
}
