using AuthServer.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private  readonly DbContext _context;
        private  readonly DbSet<T> _dbset;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbset=_context.Set<T>();
        }
        public async Task AddAsync(T Entity)
        {
            await _dbset.AddAsync(Entity);
            
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbset.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int Id)
        {
            var entity=await _dbset.FindAsync(Id);
            if (entity!=null)
            {
            _context.Entry(entity).State = EntityState.Detached;
            }
            return entity;
        }

        public void Remove(T Entity)
        {
            _dbset.Remove(Entity);
        }

        public void Update(T Entity)
        {
            _context.Entry(Entity).State = EntityState.Modified;
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return _dbset.AsNoTracking().Where(expression);
        }
    }
}
