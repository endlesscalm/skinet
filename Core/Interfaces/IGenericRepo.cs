using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface IGenericRepo<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<T> GetEntityWithSpec(ISpec<T> spec);
        Task<IReadOnlyList<T>> ListAsync(ISpec<T> spec);
        Task<int> CountAsync(ISpec<T> spec);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}