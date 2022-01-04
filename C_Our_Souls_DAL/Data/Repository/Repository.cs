using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace C_Our_Souls_DAL.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        private DatabaseContext _dbContext { get; }

        public Repository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public void Delete(Expression<Func<T, bool>> conditions)
        {
            var list = _dbContext.Set<T>().Where(conditions).ToList();
            _dbContext.Set<T>().RemoveRange(list);
        }

        public IEnumerable<T> Get()
        {
            return _dbContext.Set<T>().ToList();
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> conditions)
        {
            return Get(conditions, null);
        }

        public IEnumerable<T> Get(params Expression<Func<T, object>>[] includes)
        {
            return Get(null, includes);
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> conditions, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            if (conditions != null)
            {
                query = query.Where(conditions);
            }
            return query;
        }

        public void Update(T entity)
        {
            _dbContext.Entry<T>(entity).State = EntityState.Modified;
        }

        public T GetById(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }
    }
}