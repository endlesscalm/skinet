using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class GenericRepo<T> : IGenericRepo<T> where T : BaseEntity
    {
        private readonly StoreContext _context;

        public GenericRepo(StoreContext context)
        {
            _context = context;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _context.Set<T>().AsNoTracking()
                .ToListAsync();
        }

        public async Task<T> GetEntityWithSpec(ISpec<T> spec)
        {
            return await ApplySpec(spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpec<T> spec)
        {
            return await ApplySpec(spec).ToListAsync();
        }

        private IQueryable<T> ApplySpec(ISpec<T> spec)
        {
            return SpecEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
        }
    }
}