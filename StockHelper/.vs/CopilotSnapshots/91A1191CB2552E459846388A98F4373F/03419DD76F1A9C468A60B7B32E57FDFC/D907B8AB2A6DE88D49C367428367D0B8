using BLL.Interfaces;
using DAL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Implementations
{
    public class GenericBllService<T>: IGenericBllService<T> where T : class
    {
        private readonly IRepository<T> _repository;
        
        public GenericBllService(IRepository<T> repository)
        {
            _repository = repository ?? throw new ArgumentException(nameof(repository));
        }

        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentException(nameof(entity));
            }
            _repository.Create(entity);
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentException(nameof(entity));
            }
            _repository.Update(entity);
        }

        public void Delete(Guid id)
        {
            if (id == Guid.Empty)
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

        public IEnumerable<T> GetAll()
        {
            return _repository.GetAll().ToList();
        }

        public T GetById(Guid id)
        {
            return _repository.GetById(id);
        }

        public bool Exists(Guid id)
        {
            return _repository.GetById(id) != null;
        }

    }
}
