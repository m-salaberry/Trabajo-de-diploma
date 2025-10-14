using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IGenericService<T> where T : class
    {
        /// <summary>
        /// Lists all entities of type T.
        /// </summary>
        /// <returns></returns>
        List<T> GetAll();
        /// <summary>
        /// Gets an entity by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetById(Guid id);
        /// <summary>
        /// Inserts a new entity of type T.
        /// </summary>
        /// <param name="entity"></param>
        void Insert(T entity);
        /// <summary>
        /// Updates an existing entity of type T.
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);
        /// <summary>
        /// Deletes an entity by its ID.
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);
        /// <summary>
        /// Checks if an entity exists by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Exists(Guid id);
    }
}
