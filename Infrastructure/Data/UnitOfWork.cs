using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _context;
        // private Hashtable _repositories;
        private Dictionary<string, object> _repositories;

        public UnitOfWork(StoreContext context)
        {
            _context = context;
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IGenericRepo<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            // if (_repositories == null) _repositories = new Hashtable();
            if (_repositories == null) _repositories = new Dictionary<string, object>();

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepo<>);
                var repositoryInstance = Activator.CreateInstance(
                    repositoryType.MakeGenericType(typeof(TEntity)), _context);
                
                _repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepo<TEntity>) _repositories[type];
        }
    }
}