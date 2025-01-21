using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TME.Domain.Core.Entities;


namespace TME.Data.Core.Repositories
{
    /// <summary>
    /// (FHC - 07/07/2014) Referências para inserção de métodos assíncronos nas funções repositório:
    /// - http://www.itworld.com/development/409087/generic-repository-net-entity-framework-6-async-operations
    /// - http://blogs.msdn.com/b/dotnet/archive/2012/04/03/async-in-4-5-worth-the-await.aspx
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract class RepositoryBase<TEntity, TKey>
        where TEntity : EntityBase<TEntity, Guid>, IEntity<Guid>
        where TKey : struct
    {
        public RepositoryBase() { }

        private DbContext _AppContext;
        private DbSet<TEntity> _DbSet;
        private bool _disposed = false;


        protected RepositoryBase(DbContext appContext)
        {
            _AppContext = appContext;
            _DbSet = _AppContext.Set<TEntity>();
        }

        // Constructor implementado a fim de atender especificamente aos testes.
        protected RepositoryBase(DbContext appContext, DbSet<TEntity> dbSet)
        {
            _AppContext = appContext;
            _DbSet = dbSet;
        }


        protected DbSet<TEntity> DbSet
        {
            get { return _DbSet; }
            set { _DbSet = value; }
        }

        public DbContext AppContext
        {
            get { return _AppContext; }
            protected set { _AppContext = value; }
        }


        #region Synchronous Methods

        public virtual void Create(TEntity entity)
        {
            _DbSet.Add(entity);
        }


        public virtual void Update(TEntity entity)
        {
            _DbSet.Attach(entity);
            _AppContext.Entry(entity).State = EntityState.Modified;
        }


        public virtual void LogicalExclusion(TEntity entity)
        {
            _DbSet.Attach(entity);
            _AppContext.Entry(entity).State = EntityState.Modified;
        }


        public virtual void Delete(TEntity entity)
        {
            if (_AppContext.Entry(entity).State == EntityState.Detached)
            {
                _DbSet.Attach(entity);
            }
            _DbSet.Remove(entity);
        }


        public virtual void Delete(Expression<Func<TEntity, bool>> where)
        {
            IEnumerable<TEntity> objects = _DbSet.Where<TEntity>(where).AsEnumerable();
            foreach (TEntity obj in objects)
            {
                _DbSet.Remove(obj);
            }
        }


        public virtual IEnumerable<TEntity> GetAll()
        {
            try
            {
                return _DbSet.ToList();
            }
            catch (Exception err)
            {
                Console.Write(err.Message);
            }
            return null;
        }


        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            // Testar --> return _DbSet.AsQueryable().Where(filter).ToList();
            return _DbSet.Where(filter.Compile()).ToList<TEntity>();
        }


        /// <summary>
        /// Reference:
        /// - http://blog.longle.net/2013/10/09/upgrading-to-async-with-entity-framework-mvc-odata-asyncentitysetcontroller-kendo-ui-glimpse-generic-unit-of-work-repository-framework-v2-0/
        /// - https://medium.com/@dnzcnyksl/pagination-in-c-e346a34e7984
        /// </summary>
        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includeProperties = null,
            int? page = null, int? pageSize = null)
        {
            IQueryable<TEntity> query = _DbSet;

            if (includeProperties != null){
                includeProperties.ForEach(i => query = query.Include(i));
            }

            if (filter != null){
                query = query.Where(filter);
            }

            if (orderBy != null){
                query = orderBy(query);
            }

            if (page != null || pageSize != null)
            {
                query = query
                    .Skip((page.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
            }
            return query;
        }

        #endregion



        #region Asynchronous Methods


        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await _DbSet.AsNoTracking<TEntity>()
                .Where(field => field.IsActive.Equals(true) &&
                field.IsDeleted.Equals(false)
            ).ToListAsync();
        }


        public virtual async Task<TEntity> GetByIdAsync(TKey id)
        {
            return await _DbSet.AsNoTracking<TEntity>().Where(field =>
                field.Id.Equals(id) &&
                field.IsActive.Equals(true) &&
                field.IsDeleted.Equals(false)
            ).FirstOrDefaultAsync();
        }


        public virtual async Task<TEntity> GetByIdAsync(TKey id, params string[] includes)
        {
            var query = _DbSet.Where(field => field.Id.Equals(id) &&
                field.IsActive.Equals(true) &&
                field.IsDeleted.Equals(false));

            if (includes != null)
            {
                query = LoadIncludesForQuery(query, includes);
            }
            return await query.AsNoTracking<TEntity>().FirstOrDefaultAsync();
        }


        public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _DbSet.Where(filter).ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includeProperties = null,
            int? page = null, int? pageSize = null)
        {
            return Get(filter, orderBy, includeProperties, page, pageSize).AsEnumerable().ToList<TEntity>();
        }

        #endregion



        protected IQueryable<TEntity> LoadIncludesForQuery(IQueryable<TEntity> query, params string[] includes)
        {
            if (includes == null){
                return query;
            }

            List<string> includeList = new List<string>();
            if (includes.Any())
            {
                return includes
                    .Where(x => !string.IsNullOrEmpty(x) && !includeList.Contains(x))
                    .Aggregate(query, (current, include) => current.Include(include));
            }

            IEnumerable<Microsoft.EntityFrameworkCore.Metadata.INavigation> navigationProperties =
                _AppContext.Model.FindEntityType(typeof(TEntity)).GetNavigations();
            if (navigationProperties == null)
            {
                return query;
            }

            foreach (Microsoft.EntityFrameworkCore.Metadata.INavigation navigationProperty in navigationProperties)
            {
                if (includeList.Contains(navigationProperty.Name)){
                    continue;
                }

                includeList.Add(navigationProperty.Name);
                query = query.Include(navigationProperty.Name);
            }

            return query;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing){
                    _AppContext.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
