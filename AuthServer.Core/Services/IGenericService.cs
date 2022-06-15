using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface IGenericService<T,TDto> where T : class where TDto : class
    {
        Task<Response<IEnumerable<TDto>>> GetAllAsync();
        Task<Response<TDto>> GetByIdAsync(int Id);
        Task<Response<IQueryable<TDto>>> Where(Expression<Func<T, bool>> predicate);
        Task<Response<TDto>> AddAsync(TDto Entity);
        Task<Response<NoContentDto>> Update(TDto Entity, int Id);
        Task<Response<NoContentDto>> Remove(int Id);
     

    }
}
