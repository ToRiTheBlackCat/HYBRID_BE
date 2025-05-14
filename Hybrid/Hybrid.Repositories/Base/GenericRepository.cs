using Hybrid.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Repositories.Base
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllWithIncludeAsync(params Expression<Func<T, object>>[] includeProperties);
        T? GetById(int id);
        Task<T?> GetByIdAsync(int id);
        T? GetById(string id);
        Task<T?> GetByIdAsync(string id);
        Task<T?> GetByIdWithIncludeAsync(int TId, string typeId, params Expression<Func<T, object>>[] includeProperties);

        void Create(T entity);
        Task<int> CreateAsync(T entity);
        void Update(T entity);
        Task<int> UpdateAsync(T entity);
        void Remove(T entity);
        Task<bool> RemoveAsync(T? entity);
    }

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly HybridDBContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(HybridDBContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// Get all with generic include of entity
        /// EXAMPLE HOW TO USE
        ///  var products = (await _unitOfWork.ProductRepository.GetAllWithIncludeAsync(p => p.Category)).AsQueryable();
        /// </summary>
        public async Task<IEnumerable<T>> GetAllWithIncludeAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.ToListAsync();
        }

        public T? GetById(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;
        }

        public T? GetById(string id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;
        }

        public async Task<T?> GetByIdAsync(string id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;
        }


        /// <summary>
        /// Get detail with id and with generic include of entity
        /// EXAMPLE HOW TO USE
        /// var foundCart = await _unitOfWork.CartRepository.GetByIdWithIncludeAsync(request.CartId, "CartId", cart => cart.ShoppingCartItems);
        /// </summary>
        public async Task<T?> GetByIdWithIncludeAsync(int TId, string typeId, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.FirstOrDefaultAsync(entity => EF.Property<int>(entity, typeId) == TId);
        }

        public void Create(T entity)
        {
            _dbSet.Add(entity);
        }

        public async Task<int> CreateAsync(T entity)
        {
            _dbSet.Add(entity);
            return await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public async Task<int> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return await _context.SaveChangesAsync();
        }
        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<bool> RemoveAsync(T? entity)
        {
            if (entity == null)
                return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
