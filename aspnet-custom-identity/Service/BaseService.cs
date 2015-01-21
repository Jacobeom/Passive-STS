using aspnet_custom_identity.Model;
using aspnet_custom_identity.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aspnet_custom_identity.Service
{
    public interface IBaseService<T> where T : BaseModel
    {
        T FindById(int id);
        Task<T> FindByIdAsync(int id);
        T Add(T entity);
        void Delete(T entity);
        T Update(T entity);
        IEnumerable<T> FindAll();
        Task<List<T>> FindAllAsync();
        void Dispose();
    }

    public class BaseService<T> : IBaseService<T>, IDisposable where T : BaseModel
    {
        public virtual IBaseRepository<T> Repository { get; set; }

        public virtual T FindById(int id)
        {
            return Repository.FindById(id);
        }

        public virtual Task<T> FindByIdAsync(int id)
        {
            return Repository.FindByIdAsync(id);
        }

        public virtual T Add(T entity)
        {
            T result = Repository.Add(entity);
            
            return result;
        }
        
        public virtual void Delete(T entity)
        {
            Repository.Delete(entity);
        }

        public virtual T Update(T entity)
        {
            Repository.ForceEdit(entity);
            return entity;
        }

        public virtual IEnumerable<T> FindAll()
        {
            return Repository.FindAll();
        }

        public virtual Task<List<T>> FindAllAsync()
        {
            return Repository.FindAllAsync();
        }

        public virtual void Dispose()
        {
            Repository.Dispose();
        }
    }
}
