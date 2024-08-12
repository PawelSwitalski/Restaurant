using Restaurant.Data.Data;
using Restaurant.Data.Entities;
using Restaurant.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Restaurant.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected RestaurantDbContext context { get; private set; }

        public Repository(RestaurantDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(TEntity entity)
        {
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public void Delete(TEntity entity)
        {
            context.Remove(entity);
            context.SaveChanges();
        }

        public async Task DeleteByIdAsync(int id)
        {
            TEntity entity = await context.FindAsync<TEntity>(id);
            context.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await context.FindAsync<TEntity>(id);
        }

        public void Update(TEntity entity)
        {
            context.Update(entity);
            context.SaveChanges();
        }
    }
}
