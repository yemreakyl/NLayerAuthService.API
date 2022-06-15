using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int Id);
        IQueryable<T> Where(Expression<Func<T,bool>> expression);
        Task AddAsync(T Entity);
        void Update(T Entity);
        void Remove(T Entity);
    }
}
