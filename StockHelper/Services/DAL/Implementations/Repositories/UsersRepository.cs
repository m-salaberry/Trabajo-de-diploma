using Services.DAL.Contracts;
using DAL.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using Services.Domain;

namespace Services.DAL.Implementations.Repositories
{
    public class UsersRepository : IRepository
    {
        private FamilyRepository _familyRepository;

        public UsersRepository()
        {
            _familyRepository = new FamilyRepository();
        }

        public void Create<T>(T entity) where T : class
        {
            string command = "INSERT INTO USERS (Id, Name, Password, IsActive, Role) VALUES (@Id, @Name, @Password, @IsActive, @Role)";
            var parameters = new[]
            {
                new SqlParameter("@Id", typeof(T).GetProperty("Id")?.GetValue(entity)),
                new SqlParameter("@Name", typeof(T).GetProperty("Name")?.GetValue(entity)),
                new SqlParameter("@Password", typeof(T).GetProperty("Password")?.GetValue(entity)),
                new SqlParameter("@IsActive", typeof(T).GetProperty("IsActive")?.GetValue(entity)),
                new SqlParameter("@Role", typeof(T).GetProperty("Role")?.GetValue(entity) ?? DBNull.Value)
            };
            SqlHelper.ExecuteNonQuery(command, CommandType.Text, parameters);
        }

        public void Delete<T>(T entity) where T : class
        {
            User user = (User)(object)entity;
            // First, delete related entries in USERS_FAMILIES
            FillUserFamily(user);
            foreach (var family in user.Permissions.OfType<Family>())
            {
                DeleteRelationBetweenUserAndFamily(user, family);
            }
            // Then, delete the user
            string command = "DELETE FROM USERS WHERE Id = @Id";
            var parameters = new[]
            {
                new SqlParameter("@Id", typeof(T).GetProperty("Id")?.GetValue(entity))
            };
            SqlHelper.ExecuteNonQuery(command, CommandType.Text, parameters);
        }

        /// <summary>
        /// Public method to get all users from the database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>A List of Users</returns>
        public IEnumerable<T> GetAll<T>() where T : class
        {
            var list = new List<T>();

            using (var reader = SqlHelper.ExecuteReader("SELECT * FROM USERS", CommandType.Text))
            {
                while (reader.Read())
                {
                    var user = Activator.CreateInstance(typeof(T));
                    typeof(T).GetProperty("Id")?.SetValue(user, reader["Id"]);
                    typeof(T).GetProperty("Name")?.SetValue(user, reader["Name"]);
                    typeof(T).GetProperty("Password")?.SetValue(user, reader["Password"]);
                    typeof(T).GetProperty("IsActive")?.SetValue(user, reader["IsActive"]);
                    typeof(T).GetProperty("Role")?.SetValue(user, reader["Role"] != DBNull.Value ? reader["Role"] : null);
                    list.Add((T)user);
                }
            }
            return list;
        }

        /// <summary>
        /// Public method to get a user by Id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns>A specific User based in the id sent by param</returns>
        public T GetById<T>(Guid id) where T : class
        {
            string command = "SELECT * FROM USERS WHERE Id = @Id";
            var parameters = new[]
            {
                new SqlParameter("@Id", id)
            };
            using (var reader = SqlHelper.ExecuteReader(command, CommandType.Text, parameters))
            {
                if (reader.Read())
                {
                    var user = Activator.CreateInstance(typeof(T));
                    typeof(T).GetProperty("Id")?.SetValue(user, reader["Id"]);
                    typeof(T).GetProperty("Name")?.SetValue(user, reader["Name"]);
                    typeof(T).GetProperty("Password")?.SetValue(user, reader["Password"]);
                    typeof(T).GetProperty("IsActive")?.SetValue(user, reader["IsActive"]);
                    typeof(T).GetProperty("Role")?.SetValue(user, reader["Role"] != DBNull.Value ? reader["Role"] : null);
                    return (T)user!;
                }
                else
                {
                    return null;
                }
            }
        }

        public void Update<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves a user by their name from the database.
        /// </summary>
        /// <remarks>The returned user object includes associated families and patents. This method
        /// performs a database query and may impact performance if called frequently.</remarks>
        /// <param name="name">The name of the user to retrieve. Cannot be null or empty.</param>
        /// <returns>A <see cref="User"/> object representing the user with the specified name, or <see langword="null"/> if no
        /// matching user is found.</returns>
        public User GetByName(string name)
        {
            string command = "SELECT * FROM USERS WHERE Name = @Name";
            var parameters = new[]
            {
                new SqlParameter("@Name", name)
            };
            using (var reader = SqlHelper.ExecuteReader(command, CommandType.Text, parameters))
            {
                if (reader.Read())
                {
                    var user = new User
                    {
                        Id = (Guid)reader["Id"],
                        Name = (string)reader["Name"],
                        Password = (string)reader["Password"],
                        IsActive = reader["IsActive"] != DBNull.Value ? (bool)reader["IsActive"] : false,
                        Role = reader["Role"] != DBNull.Value ? (string)reader["Role"] : null
                    };
                    // Fill user's families
                    FillUserFamily(user);
                    // Fill patents for each family
                    foreach (var family in user.Permissions.OfType<Family>())
                    {
                        _familyRepository.FillFamilyPatents(family);
                    }
                    return user;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Public method to get all active users from the database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<User> GetAllActive<T>() where T : class
        {
            string command = "SELECT * FROM USERS WHERE IsActive = 1";
            var list = new List<User>();
            using (var reader = SqlHelper.ExecuteReader(command, CommandType.Text))
            {
                while (reader.Read())
                {
                    var user = new User
                    {
                        Id = (Guid)reader["Id"],
                        Name = (string)reader["Name"],
                        Password = (string)reader["Password"],
                        IsActive = (bool)reader["IsActive"],
                        Role = reader["Role"] != DBNull.Value ? (string)reader["Role"] : null
                    };
                    // Fill user's families
                    FillUserFamily(user);
                    // Fill patents for each family
                    foreach (var family in user.Permissions.OfType<Family>())
                    {
                        _familyRepository.FillFamilyPatents(family);
                    }
                    list.Add(user);
                }
            }
            return list;
        }

        /// <summary>
        /// Fills the user's families (roles) based on their user ID.
        /// </summary>
        /// <param name="user">User to fill with families</param>
        public void FillUserFamily(User user)
        {
            string command = "SELECT f.Id, f.Name FROM FAMILIES f " +
                             "JOIN USERS_FAMILIES uf ON f.Id = uf.FamilyId " +
                             "WHERE uf.UserId = @UserId";
            SqlParameter[] parameters = {
                new SqlParameter("@UserId", user.Id)
            };
            using (var reader = SqlHelper.ExecuteReader(command, CommandType.Text, parameters))
            {
                while (reader.Read())
                {
                    var family = new Family
                    {
                        Id = (Guid)reader["Id"],
                        Name = (string)reader["Name"]
                    };
                    user.Permissions.Add(family);
                }
            }
        }

        /// <summary>
        /// Saves related Family for a user.
        /// Assigns a role (family) to a user.
        /// </summary>
        /// <param name="user">User to assign role to</param>
        /// <param name="family">Role (family) to assign</param>
        public void SaveRelatedFamilyOfUser(User user, Family family)
        {
            string command = "INSERT INTO USERS_FAMILIES (UserId, FamilyId) VALUES (@UserId, @FamilyId)";
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@UserId", user.Id),
                new SqlParameter("@FamilyId", family.Id)
            };
            SqlHelper.ExecuteNonQuery(command, CommandType.Text, parameters);
        }

        /// <summary>
        /// Deletes the relation between a user and a family.
        /// Removes a role (family) from a user.
        /// </summary>
        /// <param name="user">User to remove role from</param>
        /// <param name="family">Role (family) to remove</param>
        public void DeleteRelationBetweenUserAndFamily(User user, Family family)
        {
            string command = "DELETE FROM USERS_FAMILIES WHERE UserId = @UserId AND FamilyId = @FamilyId";
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@UserId", user.Id),
                new SqlParameter("@FamilyId", family.Id)
            };
            SqlHelper.ExecuteNonQuery(command, CommandType.Text, parameters);
        }
    }
}
