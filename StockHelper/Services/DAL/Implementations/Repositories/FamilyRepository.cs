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
    /// Repository for managing Family (roles) entities.
    /// Families represent groups of patents (permissions) that form roles.
    /// </summary>
    public class FamilyRepository : IRepository
    {
        /// <summary>
        /// Gets all families (roles) from the database.
        /// </summary>
        /// <returns>Collection of all families</returns>
        public IEnumerable<Family> GetAll()
        {
            string command = "SELECT Id, Name, Description FROM FAMILIES ORDER BY Name";
            var families = new List<Family>();
            
            using (var reader = SqlHelper.ExecuteReader(command, CommandType.Text))
            {
                if (reader == null)
                {
                    Logger.Current.Warning("ExecuteReader returned null in FamilyRepository.GetAll");
                    return Enumerable.Empty<Family>();
                }

                while (reader.Read())
                {
                    var family = new Family
                    {
                        Id = (Guid)reader["Id"],
                        Name = (string)reader["Name"]
                    };
                    families.Add(family);
                }
            }
            
            Logger.Current.Debug($"Retrieved {families.Count} families from database");
            return families;
        }

        /// <summary>
        /// Gets a family by its unique identifier.
        /// Includes all patents assigned to the family.
        /// </summary>
        /// <param name="id">Family ID</param>
        /// <returns>Family if found, null otherwise</returns>
        public Family GetById(Guid id)
        {
            string command = "SELECT Id, Name FROM FAMILIES WHERE Id = @Id";
            var parameters = new[] { new SqlParameter("@Id", id) };
            
            using (var reader = SqlHelper.ExecuteReader(command, CommandType.Text, parameters))
            {
                if (reader != null && reader.Read())
                {
                    var family = new Family
                    {
                        Id = (Guid)reader["Id"],
                        Name = (string)reader["Name"]
                    };
                    
                    // Fill patents for this family
                    FillFamilyPatents(family);
                    
                    return family;
                }
            }
            
            Logger.Current.Warning($"Family with ID {id} not found");
            return null;
        }

        /// <summary>
        /// Gets a family by its name.
        /// Includes all patents assigned to the family.
        /// </summary>
        /// <param name="name">Family name</param>
        /// <returns>Family if found, null otherwise</returns>
        public Family GetByName(string name)
        {
            string command = "SELECT Id, Name FROM FAMILIES WHERE Name = @Name";
            var parameters = new[] { new SqlParameter("@Name", name) };
            
            using (var reader = SqlHelper.ExecuteReader(command, CommandType.Text, parameters))
            {
                if (reader != null && reader.Read())
                {
                    var family = new Family
                    {
                        Id = (Guid)reader["Id"],
                        Name = (string)reader["Name"]
                    };
                    
                    // Fill patents for this family
                    FillFamilyPatents(family);
                    
                    return family;
                }
            }
            
            Logger.Current.Warning($"Family with name '{name}' not found");
            return null;
        }

        /// <summary>
        /// Fills a family's patents (permissions) based on the family ID.
        /// This method populates the family's Children collection with all assigned patents.
        /// </summary>
        /// <param name="family">Family to fill with patents</param>
        public void FillFamilyPatents(Family family)
        {
            string command = "SELECT p.Id, p.Name FROM PATENTS p " +
                             "JOIN PATENTS_FAMILIES pf ON p.Id = pf.PatentId " +
                             "WHERE pf.FamilyId = @FamilyId " +
                             "ORDER BY p.Name";
            SqlParameter[] parameters = {
                new SqlParameter("@FamilyId", family.Id)
            };
            
            using (var reader = SqlHelper.ExecuteReader(command, CommandType.Text, parameters))
            {
                while (reader.Read())
                {
                    var patent = new Patent
                    {
                        Id = (Guid)reader["Id"],
                        Name = (string)reader["Name"]
                    };
                    family.AddChild(patent);
                }
            }
            
            Logger.Current.Debug($"Filled {family.Children.Count} patents for family '{family.Name}'");
        }

        /// <summary>
        /// Creates a new family in the database.
        /// </summary>
        /// <param name="family">Family to create</param>
        public void Create(Family family)
        {
            string command = "INSERT INTO FAMILIES (Id, Name, Description, CreatedDate) VALUES (@Id, @Name, @Description, @CreatedDate)";
            var parameters = new[]
            {
                new SqlParameter("@Id", family.Id),
                new SqlParameter("@Name", family.Name),
                new SqlParameter("@Description", DBNull.Value),
                new SqlParameter("@CreatedDate", DateTime.UtcNow)
            };
            int result = SqlHelper.ExecuteNonQuery(command, CommandType.Text, parameters);
            
            if (result == -1)
            {
                Logger.Current.Error($"Failed to create family '{family.Name}' with ID {family.Id}");
                throw new Exception($"Failed to create family '{family.Name}'.");
            }
            
            Logger.Current.Info($"Family '{family.Name}' created with ID {family.Id}");
        }

        /// <summary>
        /// Updates an existing family in the database.
        /// </summary>
        /// <param name="family">Family with updated information</param>
        public void Update(Family family)
        {
            string command = "UPDATE FAMILIES SET Name = @Name WHERE Id = @Id";
            var parameters = new[]
            {
                new SqlParameter("@Id", family.Id),
                new SqlParameter("@Name", family.Name),
                new SqlParameter("@ModifiedDate", DateTime.UtcNow)
            };
            int result = SqlHelper.ExecuteNonQuery(command, CommandType.Text, parameters);
            
            if (result == -1)
            {
                Logger.Current.Error($"Failed to update family '{family.Name}' with ID {family.Id}");
                throw new Exception($"Failed to update family '{family.Name}'.");
            }
            
            Logger.Current.Info($"Family '{family.Name}' updated");
        }

        /// <summary>
        /// Deletes a family from the database.
        /// </summary>
        /// <param name="id">ID of the family to delete</param>
        public void Delete(Guid id)
        {
            string command = "DELETE FROM FAMILIES WHERE Id = @Id";
            var parameters = new[] { new SqlParameter("@Id", id) };
            int result = SqlHelper.ExecuteNonQuery(command, CommandType.Text, parameters);
            
            if (result == -1)
            {
                Logger.Current.Error($"Failed to delete family with ID {id}");
                throw new Exception($"Failed to delete family with ID {id}.");
            }
            
            Logger.Current.Info($"Family with ID {id} deleted");
        }

        /// <summary>
        /// Assigns a patent (permission) to a family (role).
        /// Creates a relationship in the PATENTS_FAMILIES table.
        /// </summary>
        /// <param name="patentId">ID of the patent to assign</param>
        /// <param name="familyId">ID of the family to receive the patent</param>
        public void AssignPatentToFamily(Guid patentId, Guid familyId)
        {
            // Check if relationship already exists
            string checkCommand = "SELECT COUNT(*) FROM PATENTS_FAMILIES WHERE PatentId = @PatentId AND FamilyId = @FamilyId";
            var checkParams = new[]
            {
                new SqlParameter("@PatentId", patentId),
                new SqlParameter("@FamilyId", familyId)
            };
            
            using (var reader = SqlHelper.ExecuteReader(checkCommand, CommandType.Text, checkParams))
            {
                if (reader != null && reader.Read() && reader.GetInt32(0) > 0)
                {
                    Logger.Current.Warning($"Patent {patentId} is already assigned to family {familyId}");
                    return;
                }
            }

            // Insert new relationship
            string command = "INSERT INTO PATENTS_FAMILIES (PatentId, FamilyId, AssignedDate) VALUES (@PatentId, @FamilyId, @AssignedDate)";
            var parameters = new[]
            {
                new SqlParameter("@PatentId", patentId),
                new SqlParameter("@FamilyId", familyId),
                new SqlParameter("@AssignedDate", DateTime.UtcNow)
            };
            int result = SqlHelper.ExecuteNonQuery(command, CommandType.Text, parameters);
            
            if (result == -1)
            {
                Logger.Current.Error($"Failed to assign patent {patentId} to family {familyId}");
                throw new Exception("Failed to assign patent to family.");
            }

            Logger.Current.Info($"Patent {patentId} assigned to family {familyId}");
        }

        /// <summary>
        /// Removes a patent (permission) from a family (role).
        /// Deletes the relationship from the PATENTS_FAMILIES table.
        /// </summary>
        /// <param name="patentId">ID of the patent to remove</param>
        /// <param name="familyId">ID of the family to remove from</param>
        public void RemovePatentFromFamily(Guid patentId, Guid familyId)
        {
            string command = "DELETE FROM PATENTS_FAMILIES WHERE PatentId = @PatentId AND FamilyId = @FamilyId";
            var parameters = new[]
            {
                new SqlParameter("@PatentId", patentId),
                new SqlParameter("@FamilyId", familyId)
            };
            int result = SqlHelper.ExecuteNonQuery(command, CommandType.Text, parameters);
            
            if (result == -1)
            {
                Logger.Current.Error($"Failed to remove patent {patentId} from family {familyId}");
                throw new Exception($"Failed to remove patent from family.");
            }
            
            Logger.Current.Info($"Patent {patentId} removed from family {familyId}");
        }

        /// <summary>
        /// Gets all patents assigned to a specific family.
        /// </summary>
        /// <param name="familyId">Family ID</param>
        /// <returns>Collection of patents</returns>
        public IEnumerable<Patent> GetFamilyPatents(Guid familyId)
        {
            string command = "SELECT p.Id, p.Name FROM PATENTS p " +
                             "JOIN PATENTS_FAMILIES pf ON p.Id = pf.PatentId " +
                             "WHERE pf.FamilyId = @FamilyId " +
                             "ORDER BY p.Name";
            var patents = new List<Patent>();
            SqlParameter[] parameters = {
                new SqlParameter("@FamilyId", familyId)
            };
            
            using (var reader = SqlHelper.ExecuteReader(command, CommandType.Text, parameters))
            {
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
            
            return patents;
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
            if (entity is Family family)
            {
                Create(family);
            }
            else
            {
                throw new ArgumentException($"Entity must be of type Family, but was {entity.GetType().Name}");
            }
        }

        void IRepository.Update<T>(T entity)
        {
            if (entity is Family family)
            {
                Update(family);
            }
            else
            {
                throw new ArgumentException($"Entity must be of type Family, but was {entity.GetType().Name}");
            }
        }

        void IRepository.Delete<T>(T entity)
        {
            if (entity is Family family)
            {
                Delete(family.Id);
            }
            else
            {
                throw new ArgumentException($"Entity must be of type Family, but was {entity.GetType().Name}");
            }
        }
    }
}
