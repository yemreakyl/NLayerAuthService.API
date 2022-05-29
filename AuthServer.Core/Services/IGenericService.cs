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
        Task<Response<IQueryable<TDto>>> GetAllAsync();
        Task<Response<TDto>> GetByIdAsync(int Id);
        Task<Response<IQueryable<TDto>>> Where(Expression<Func<T, bool>> expression);
        Task<Response<TDto>> AddAsync(T Entity);
        Task<NoContentDto> Update(T Entity);
        Task<NoContentDto> Remove(T Entity);
    }
}
