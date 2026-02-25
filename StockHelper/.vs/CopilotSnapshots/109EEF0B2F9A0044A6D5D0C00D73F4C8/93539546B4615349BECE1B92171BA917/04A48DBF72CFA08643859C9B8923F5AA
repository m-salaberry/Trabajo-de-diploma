using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contracts
{
    public interface IRepository
    {
        /// <summary>
        /// Generic Create method for adding an entity to the database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void Create<T>(T entity) where T : class;
        /// <summary>
        /// Generic Update method for updating an entity in the database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void Update<T>(T entity) where T : class;
        /// <summary>
        /// Generic Delete method for removing an entity from the database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void Delete<T>(T entity) where T : class;
        /// <summary>
        /// Generic GetById method for retrieving an entity by its ID from the database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetById<T>(Guid id) where T : class;
        /// <summary>
        /// Generic GetAll method for retrieving all entities of a specific type from the database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetAll<T>() where T : class;
    }
}
