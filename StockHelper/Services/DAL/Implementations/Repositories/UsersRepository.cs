using Services.DAL.Contracts;
using DAL.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Domain;

namespace Services.DAL.Implementations.Repositories
{
    public class UsersRepository : IRepository
    {
        private PermissionsRepository repoPermissions;

        public UsersRepository()
        {
            repoPermissions = new PermissionsRepository();
        }

        public void Create<T>(T entity) where T : class
        {
            string command = "INSERT INTO USERS (Username, Password) VALUES (@Username, @Password)";
            var parameters = new[]
            {
                new SqlParameter("@Username", typeof(T).GetProperty("Username")?.GetValue(entity)),
                new SqlParameter("@Password", typeof(T).GetProperty("Password")?.GetValue(entity)),
                new SqlParameter("@IsActive", typeof(T).GetProperty("IsActive")?.GetValue(entity))
            };
            SqlHelper.ExecuteNonQuery(command, CommandType.Text, parameters);
        }

        public void Delete<T>(T entity) where T : class
        {
            User user = (User)(object)entity;
            // First, delete related entries in USERS_FAMILIES
            repoPermissions.FillUserFamily(user);
            foreach (var family in user.Permissions)
            {
                DeleteRelationBetweenUserAndFamily(user, (Family)family);
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
                    typeof(T).GetProperty("Id").SetValue(user, (reader["Id"]));
                    typeof(T).GetProperty("Name").SetValue(user, (reader["Name"]));
                    typeof(T).GetProperty("Password").SetValue(user, (reader["Password"]));
                    typeof(T).GetProperty("IsActive").SetValue(user, (reader["IsActive"]));
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
                    typeof(T).GetProperty("Id").SetValue(user, (reader["Id"]));
                    typeof(T).GetProperty("Name").SetValue(user, (reader["Name"]));
                    typeof(T).GetProperty("Password").SetValue(user, (reader["Password"]));
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
                        Password = (string)reader["Password"]
                    };
                    // Fill user's families
                    repoPermissions.FillUserFamily(user);
                    // Fill patents for each family
                    foreach (var family in user.Permissions.OfType<Family>())
                    {
                        repoPermissions.FillUserPatents(user, family);
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
        /// <exception cref="NotImplementedException"></exception>
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
                        IsActive = (bool)reader["IsActive"]
                    };
                    // Fill user's families
                    repoPermissions.FillUserFamily(user);
                    // Fill patents for each family
                    foreach (var family in user.Permissions.OfType<Family>())
                    {
                        repoPermissions.FillUserPatents(user, family);
                    }
                    list.Add(user);
                }
            }
            return list;
        }

        /// <summary>
        /// Saves related Family for a user.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
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
        /// </summary>
        /// <param name="user"></param>
        /// <param name="family"></param>
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
