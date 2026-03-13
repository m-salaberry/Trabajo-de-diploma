using BLL.Interfaces;
using DAL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Implementations
{
    public class GenericBllService<T, TId>: IGenericBllService<T, TId> where T : class
    {
        private readonly IRepository<T, TId> _repository;
        
        /// <summary>
        /// Initializes a new instance of the generic BLL service with the specified repository.
        /// </summary>
        public GenericBllService(IRepository<T, TId> repository)
        {
            _repository = repository ?? throw new ArgumentException(nameof(repository));
        }

        /// <summary>
        /// Inserts a new entity after validating it is not null.
        /// </summary>
        public virtual void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentException(nameof(entity));
            }
            _repository.Create(entity);
        }

        /// <summary>
        /// Updates an existing entity after validating it is not null.
        /// </summary>
        public virtual void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentException(nameof(entity));
            }
            _repository.Update(entity);
        }

        /// <summary>
        /// Deletes an entity by its identifier after verifying it exists.
        /// </summary>
        public virtual void Delete(TId id)
        {
            if (id == null || (id is Guid guid && guid == Guid.Empty) || (id is int intId && intId == 0))
            {
                throw new ArgumentException(nameof(id));
            }
            T entity = _repository.GetById(id);
            if (entity == null)
            {
                throw new InvalidOperationException($"Entity with id {id} does not exist.");
            }
            _repository.Delete(entity);
        }

        /// <summary>
        /// Returns all entities from the repository.
        /// </summary>
        public virtual IEnumerable<T> GetAll()
        {
            return _repository.GetAll().ToList();
        }

        /// <summary>
        /// Returns a single entity by its identifier.
        /// </summary>
        public virtual T GetById(TId id)
        {
            return _repository.GetById(id);
        }

        /// <summary>
        /// Returns true if an entity with the given identifier exists.
        /// </summary>
        public virtual bool Exists(TId id)
        {
            return _repository.GetById(id) != null;
        }

    }
}
