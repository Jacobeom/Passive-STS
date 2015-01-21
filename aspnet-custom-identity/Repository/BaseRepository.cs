using aspnet_custom_identity.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aspnet_custom_identity.Repository
{
    public interface IBaseRepository<T> where T : BaseModel
    {
        T FindById(int id);
        Task<T> FindByIdAsync(int id);
        T Add(T entity);
        void Delete(T entity);
        void ForceEdit(T entity);
        IEnumerable<T> FindAll();
        Task<List<T>> FindAllAsync();
        int SaveChanges();
        Task<int> SaveChangesAsync();
        void Dispose();

    }

    public class BaseRepository<T> : IBaseRepository<T>, IDisposable where T : BaseModel
    {

        public CustomDbContext CustomDbContext { get; set; }
        protected IDbSet<T> DbSet
        {
            get
            {
                return CustomDbContext.Set<T>();
            }
        }

        public virtual T FindById(int id)
        {
            return DbSet.FirstOrDefault(x => (x.PrimaryId.Equals(id) && x.AuditInfo.IsDeleted.Equals(false)));
            
        }

        public virtual Task<T> FindByIdAsync(int id)
        {
            return DbSet.FirstOrDefaultAsync<T>(x => (x.PrimaryId.Equals(id) && x.AuditInfo.IsDeleted.Equals(false)));

        }

        public virtual T Add(T entity)
        {
            return DbSet.Add(entity);
        }


        public virtual void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        public virtual void ForceEdit(T entity)
        {
            CustomDbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        }

        public virtual IEnumerable<T> FindAll()
        {
            return DbSet.Where(x => x.AuditInfo.IsDeleted.Equals(false));
        }

        public virtual Task<List<T>> FindAllAsync()
        {
            return DbSet.Where(x => x.AuditInfo.IsDeleted.Equals(false)).ToListAsync();
        }

        public virtual int SaveChanges()
        {
            return CustomDbContext.SaveChanges();
        }

        public virtual Task<int> SaveChangesAsync()
        {
             return CustomDbContext.SaveChangesAsync();
        }

        public virtual void Dispose()
        {
            CustomDbContext.Dispose();
        }

    }
}
