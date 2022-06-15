using AuthServer.Core.IUnitOfWork;
using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class GenericServices<T, TDto> : IGenericService<T, TDto> where TDto : class where T : class
    {
        private readonly IGenericRepository<T> _repo;
        private readonly IUnitOfWork _unitOfWork;

        public GenericServices(IGenericRepository<T> repo, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<TDto>> AddAsync(TDto Entity)
        {
           var newEntity=ObjectMapper.Mapper.Map<T>(Entity);//Gelen dto yu daha önce oluşturduğum objectmapper class ım aracılığı ile map leme işlemi yaparak normal entity e çevirdim
            await _repo.AddAsync(newEntity);
            await _unitOfWork.SaveChangesAsync();
            var returnEntity=ObjectMapper.Mapper.Map<TDto>(newEntity);
            return Response<TDto>.Success(200, returnEntity);
        }

        public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
        {
       
            var DtoEntities=ObjectMapper.Mapper.Map<List<TDto>>(await _repo.GetAllAsync());
            return  Response<IEnumerable<TDto>>.Success(200, DtoEntities);

        }
        

        public async Task<Response<TDto>> GetByIdAsync(int Id)
        {
           var Entity=await _repo.GetByIdAsync(Id);
            if (Entity==null)
            {
                return Response<TDto>.Fail(404, "Id not found", true);
            }
           var DtoEntity=ObjectMapper.Mapper.Map<TDto>(Entity);
            return Response<TDto>.Success(200, DtoEntity);

        }

        public async Task<Response<NoContentDto>> Remove(int Id)
        {
            var entityisexist = await _repo.GetByIdAsync(Id);
            if (entityisexist==null)
            {
                return Response<NoContentDto>.Fail(404,"Id not found",true);
            }
            _repo.Remove(entityisexist);
             await _unitOfWork.SaveChangesAsync();
            return Response<NoContentDto>.Success(204);
        }

        public async Task<Response<NoContentDto>>Update(TDto Entity,int Id)
        {
            var isExistEntity = await _repo.GetByIdAsync(Id);

            if (isExistEntity == null)
            {
                return Response<NoContentDto>.Fail(404,"Id not found", true);
            }

            var updateEntity = ObjectMapper.Mapper.Map<T>(Entity);

            _repo.Update(updateEntity);

            await _unitOfWork.SaveChangesAsync();
            //204 durum kodu =>  No Content  => Response body'sinde hiç bir data  olmayacak.
            return Response<NoContentDto>.Success(204);
        }

        public async Task<Response<IQueryable<TDto>>> Where(Expression<Func<T, bool>> predicate)
        {
            var Entities=_repo.Where(predicate).ToListAsync();
            if (Entities==null)
            {
                return Response<IQueryable<TDto>>.Fail(404,"wrong expression", true);
            }
            var EntitiesDtos=ObjectMapper.Mapper.Map<IQueryable<TDto>>(Entities);
            return Response<IQueryable<TDto>>.Success(200,EntitiesDtos);

        }

      
    }
}
