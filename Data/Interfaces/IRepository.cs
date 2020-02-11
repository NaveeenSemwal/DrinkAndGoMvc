using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DrinkAndGo.Data.Interfaces
{
    public interface IRepository<TEntity, TKeyType> where TEntity : class where TKeyType : struct
    {
        TEntity Get(TKeyType id);

        Task<IEnumerable<TEntity>> GetAll();

        bool Any(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> Add(TEntity entity);

        int Add(string command, SqlParameter[] param);

        IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);

        void RemoveRange(IEnumerable<TEntity> entities);

        void Update(TEntity entity);

        void Save();
    }
}
