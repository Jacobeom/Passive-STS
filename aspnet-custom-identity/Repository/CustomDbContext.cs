using aspnet_custom_identity.Model;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace aspnet_custom_identity.Repository
{
    public class CustomDbContext : DbContext
    {
        ILog log = LogManager.GetLogger(typeof(CustomDbContext));
        private Dictionary<Type, EntitySetBase> _mappingCache;


        public CustomDbContext(string connectionString)
            : base(connectionString)
        {
            this.Database.Log = log.Info;
            this._mappingCache = new Dictionary<Type, EntitySetBase>();
        }

        private IDbSet<User> MobileConnectUserMappings { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
              .Map<TwitterUser>(m => m.Requires("UserType").HasValue("TwitterUser"))
              .Map<LinkedInUser>(m => m.Requires("UserType").HasValue("LinkedInUser"))
              .Map<GitHubUser>(m => m.Requires("UserType").HasValue("GitHubUser"))
              .Map<MobileConnectUser>(m => m.Requires("UserType").HasValue("MobileConnectUser"))
              .Map<GoogleUser>(m => m.Requires("UserType").HasValue("GoogleUser"));

            base.OnModelCreating(modelBuilder);
        }


        public override int SaveChanges()
        {
            foreach (DbEntityEntry entry in ChangeTracker.Entries().Where(p => p.State == EntityState.Deleted))
                SoftDelete(entry);
            foreach (DbEntityEntry entry in ChangeTracker.Entries().Where(p => p.State == EntityState.Modified))
                Edit(entry);
            foreach (DbEntityEntry entry in ChangeTracker.Entries().Where(p => p.State == EntityState.Added))
                Add(entry);

            return base.SaveChanges();
        
        }

        public override System.Threading.Tasks.Task<int> SaveChangesAsync()
        {
            foreach (DbEntityEntry entry in ChangeTracker.Entries().Where(p => p.State == EntityState.Deleted))
                SoftDelete(entry);
            foreach (DbEntityEntry entry in ChangeTracker.Entries().Where(p => p.State == EntityState.Modified))
                Edit(entry);
            foreach (DbEntityEntry entry in ChangeTracker.Entries().Where(p => p.State == EntityState.Added))
                Add(entry);

            return base.SaveChangesAsync();
        }

        private string GetTableName(Type type)
        {
            EntitySetBase es = GetEntitySet(type);
            return string.Format("[{0}].[{1}]", es.MetadataProperties["Schema"].Value, es.MetadataProperties["Table"].Value);
        }

        private string GetPrimaryKeyName(Type type)
        {
            EntitySetBase es = GetEntitySet(type);
            return es.ElementType.KeyMembers[0].Name;
        }

        private EntitySetBase GetEntitySet(Type type)
        {
            if (!_mappingCache.ContainsKey(type))
            {
                ObjectContext octx = ((IObjectContextAdapter)this).ObjectContext;

                string typeName = ObjectContext.GetObjectType(type).Name;

                var es = octx.MetadataWorkspace.GetItemCollection(DataSpace.SSpace).GetItems<EntityContainer>()
                                .SelectMany(c => c.BaseEntitySets.Where(e => e.Name == typeName))
                                .FirstOrDefault();

                if (es == null)
                    throw new ArgumentException("Entity type not found in GetTableName", typeName);

                _mappingCache.Add(type, es);
            }

            return _mappingCache[type];
        }

        private void SoftDelete(DbEntityEntry entry)
        {
            if (entry.Entity is IAuditable)
            {
                Type entryEntityType = entry.Entity.GetType();

                string tableName = GetTableName(entryEntityType);
                string primaryKeyName = GetPrimaryKeyName(entryEntityType);

                string sql = string.Format("UPDATE {0} SET IsDeleted = 1 WHERE {1} = @id", tableName, primaryKeyName);

                Database.ExecuteSqlCommandAsync(sql, new SqlParameter("@id", entry.OriginalValues[primaryKeyName]));
                entry.State = EntityState.Detached;
            }
        }

        private void Edit(DbEntityEntry entry)
        {
            if (entry.Entity is IAuditable)
            {
                Type entryEntityType = entry.Entity.GetType();
                DbPropertyValues auditInfo = (DbPropertyValues)entry.CurrentValues["AuditInfo"];
                auditInfo["ModificationDate"] = DateTime.UtcNow;
            }
        }

        private void Add(DbEntityEntry entry)
        {
            if (entry.Entity is IAuditable)
            {
                Type entryEntityType = entry.Entity.GetType();
                DbPropertyValues auditInfo = (DbPropertyValues)entry.CurrentValues["AuditInfo"];
                DateTime now = DateTime.UtcNow;
                auditInfo["CreationDate"] = now;
                auditInfo["ModificationDate"] = now;
            }
        }
    }
}
