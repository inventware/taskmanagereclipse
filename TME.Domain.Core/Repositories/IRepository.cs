using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TME.Domain.Core.Entities;


namespace TME.Domain.Core.Repositories
{
    public interface IRepository<TEntity, TKey>
                where TEntity : EntityBase<TEntity, Guid>, IEntity<Guid>
                where TKey : struct
    {
        DbContext AppContext { get; }

        void Create(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        void LogicalExclusion(TEntity entity);

        void Delete(Expression<Func<TEntity, bool>> where);

        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> where);


        #region Asynchronous Methods

        Task<List<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(TKey id);

        Task<TEntity> GetByIdAsync(TKey id, params string[] includes);

        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter);

        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includeProperties = null,
            int? page = null, int? pageSize = null);

        #endregion


        void Dispose();
    }
}
