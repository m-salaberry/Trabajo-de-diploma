using Services.DAL.Contracts;
using Services.Domain;
using Services.Contracts.Logs;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using DAL.Helpers;
using System.Data;

namespace Services.DAL.Implementations.Repositories
{
    /// <summary>
    /// Repository for managing Patent (atomic permissions) entities.
    /// Patents represent individual permissions that cannot be subdivided.
    /// </summary>
    public class PatentRepository : IRepository
    {
        /// <summary>
        /// Gets all patents from the database.
        /// </summary>
        /// <returns>Collection of all patents</returns>
        public IEnumerable<Patent> GetAll()
        {
            string command = "SELECT Id, Name FROM PATENTS ORDER BY Name";
            var patents = new List<Patent>();
            
            using (var reader = SqlHelper.ExecuteReader(command, CommandType.Text))
            {
                if (reader == null)
                {
                    Logger.Current.Warning("ExecuteReader returned null in PatentRepository.GetAll");
                    return Enumerable.Empty<Patent>();
                }

                while (reader.Read())
                {
                    var patent = new Patent
                    {
                        Id = (Guid)reader["Id"],
                        Name = (string)reader["Name"]
                    };
                    patents.Add(patent);
                }
            }
            
            Logger.Current.Debug($"Retrieved {patents.Count} patents from database");
            return patents;
        }

        /// <summary>
        /// Gets a patent by its unique identifier.
        /// </summary>
        /// <param name="id">Patent ID</param>
        /// <returns>Patent if found, null otherwise</returns>
        public Patent GetById(Guid id)
        {
            string command = "SELECT Id, Name FROM PATENTS WHERE Id = @Id";
            var parameters = new[] { new SqlParameter("@Id", id) };
            
            using (var reader = SqlHelper.ExecuteReader(command, CommandType.Text, parameters))
            {
                if (reader != null && reader.Read())
                {
                    return new Patent
                    {
                        Id = (Guid)reader["Id"],
                        Name = (string)reader["Name"]
                    };
                }
            }
            
            Logger.Current.Warning($"Patent with ID {id} not found");
            return null;
        }

        /// <summary>
        /// Gets a patent by its name.
        /// </summary>
        /// <param name="name">Patent name</param>
        /// <returns>Patent if found, null otherwise</returns>
        public Patent GetByName(string name)
        {
            string command = "SELECT Id, Name FROM PATENTS WHERE Name = @Name";
            var parameters = new[] { new SqlParameter("@Name", name) };
            
            using (var reader = SqlHelper.ExecuteReader(command, CommandType.Text, parameters))
            {
                if (reader != null && reader.Read())
                {
                    return new Patent
                    {
                        Id = (Guid)reader["Id"],
                        Name = (string)reader["Name"]
                    };
                }
            }
            
            Logger.Current.Warning($"Patent with name '{name}' not found");
            return null;
        }

        /// <summary>
        /// Creates a new patent in the database.
        /// </summary>
        /// <param name="patent">Patent to create</param>
        public void Create(Patent patent)
        {
            string command = "INSERT INTO PATENTS (Id, Name, Description) VALUES (@Id, @Name, @Description)";
            var parameters = new[]
            {
                new SqlParameter("@Id", patent.Id),
                new SqlParameter("@Name", patent.Name),
                new SqlParameter("@Description", DBNull.Value)
            };
            SqlHelper.ExecuteNonQuery(command, CommandType.Text, parameters);
            Logger.Current.Info($"Patent '{patent.Name}' created with ID {patent.Id}");
        }

        /// <summary>
        /// Updates an existing patent in the database.
        /// </summary>
        /// <param name="patent">Patent with updated information</param>
        public void Update(Patent patent)
        {
            string command = "UPDATE PATENTS SET Name = @Name WHERE Id = @Id";
            var parameters = new[]
            {
                new SqlParameter("@Id", patent.Id),
                new SqlParameter("@Name", patent.Name)
            };
            SqlHelper.ExecuteNonQuery(command, CommandType.Text, parameters);
            Logger.Current.Info($"Patent '{patent.Name}' updated");
        }

        /// <summary>
        /// Deletes a patent from the database.
        /// </summary>
        /// <param name="id">ID of the patent to delete</param>
        public void Delete(Guid id)
        {
            string command = "DELETE FROM PATENTS WHERE Id = @Id";
            var parameters = new[] { new SqlParameter("@Id", id) };
            SqlHelper.ExecuteNonQuery(command, CommandType.Text, parameters);
            Logger.Current.Info($"Patent with ID {id} deleted");
        }

        // ============================================
        // IRepository Generic Interface Implementation
        // ============================================

        IEnumerable<T> IRepository.GetAll<T>()
        {
            return GetAll() as IEnumerable<T>;
        }

        T IRepository.GetById<T>(Guid id)
        {
            return GetById(id) as T;
        }

        void IRepository.Create<T>(T entity)
        {
            if (entity is Patent patent)
            {
                Create(patent);
            }
            else
            {
                throw new ArgumentException($"Entity must be of type Patent, but was {entity.GetType().Name}");
            }
        }

        void IRepository.Update<T>(T entity)
        {
            if (entity is Patent patent)
            {
                Update(patent);
            }
            else
            {
                throw new ArgumentException($"Entity must be of type Patent, but was {entity.GetType().Name}");
            }
        }

        void IRepository.Delete<T>(T entity)
        {
            if (entity is Patent patent)
            {
                Delete(patent.Id);
            }
            else
            {
                throw new ArgumentException($"Entity must be of type Patent, but was {entity.GetType().Name}");
            }
        }
    }
}
