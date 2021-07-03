using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BuyScout.Domain.Interfaces
{
    public interface IRepository
    {
        Task<T> StoreAsync<T>(T entity) where T : class;

        Task<IEnumerable<T>> QueryAsync<T>(Expression<Func<T, bool>> filterPredicate) where T : class;
        
        Task<T> FirstAsync<T>(Expression<Func<T, bool>> filterPredicate) where T : class;

        Task<IEnumerable<T>> RemoveAsync<T>(Expression<Func<T, bool>> filterPredicate) where T : class;
    }
}