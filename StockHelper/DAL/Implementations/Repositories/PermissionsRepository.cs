using DAL.Contracts;
using Services.Domain;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Helpers;
using System.Data;

namespace DAL.Implementations.Repositories
{
    public class PermissionsRepository : IRepository
    {
        public void Create<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public void Delete<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll<T>() where T : class
        {
            string command = "SELECT Id, Name FROM PATENTS";
            var patents = new List<Patent>();
            using (var reader = SqlHelper.ExecuteReader(command, CommandType.Text))
            {
                while (reader.Read())
                {
                    var patent = new Patent
                    {
                        Name = (string)reader["Name"],
                        Perm = (PermissionTypes)Enum.GetValues(typeof(PermissionTypes)).GetValue((int)reader["Permission"])
                    };
                    patents.Add(patent);
                }
            }
            return patents as IEnumerable<T>;
        }

        public T GetById<T>(Guid id) where T : class
        {
            throw new NotImplementedException();
        }

        public void Update<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Fills the user's families based on their user ID.
        /// </summary>
        /// <param name="user"></param>
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
        /// Fills the user's family patents based on their family ID.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="family"></param>
        public void FillUserPatents(User user, Family family)
        {
            string command = "SELECT p.Name, p.Permission FROM PATENTS p" +
                             "JOIN PATENTS_FAMILIES pf ON p.Id = pf.PatentId" +
                             "WHERE pf.FamilyId = @FamilyId";
            SqlParameter[] parameters = {
                new SqlParameter("@FamilyId", family.Id)
                };
            using (var reader = SqlHelper.ExecuteReader(command, CommandType.Text, parameters))
            {
                while (reader.Read())
                {
                    var patent = new Patent
                    {
                        Name = (string)reader["Name"],
                        Perm = (PermissionTypes)Enum.GetValues(typeof(PermissionTypes)).GetValue((int)reader["Permission"])
                    };
                    family.Children.Add(patent);
                }
            }
        }
    }
}
