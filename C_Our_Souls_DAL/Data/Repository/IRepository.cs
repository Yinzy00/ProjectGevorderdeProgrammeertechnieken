using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace C_Our_Souls_DAL.Data.Repository
{
    public interface IRepository<T> where T : class, new()
    {
        /// <summary>
        /// Get all objects from database
        /// </summary>
        /// <returns>List of <typeparamref name="T"/></returns>
        IEnumerable<T> Get();
        /// <summary>
        /// Get all objects from database with conditions
        /// </summary>
        /// <param name="conditions">Conditions as Linq expression</param>
        /// <returns>List of <typeparamref name="T"/></returns>
        IEnumerable<T> Get(Expression<Func<T, bool>> conditions);
        /// <summary>
        /// Get all objects from database with includes
        /// </summary>
        /// <param name="includes">Includes as Linq expression</param>
        /// <returns>List of <typeparamref name="T"/></returns>
        IEnumerable<T> Get(params Expression<Func<T, object>>[] includes);
        /// <summary>
        /// Get all objects from database with conditions and includes
        /// </summary>
        /// <param name="conditions">Conditions as Linq expression</param>
        /// <param name="includes">Includes as Linq expression</param>
        /// <returns>List of <typeparamref name="T"/></returns>
        IEnumerable<T> Get(Expression<Func<T, bool>> conditions, params Expression<Func<T, object>>[] includes);
        /// <summary>
        /// Get <typeparamref name="T"/> by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>object with type <typeparamref name="T"/></returns>
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        //Delete by entity
        void Delete(T entity);
        //Delete all with conditions
        void Delete(Expression<Func<T, bool>> conditions);
    }
}
