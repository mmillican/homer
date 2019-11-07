using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Homer.Shared.Data
{
    public class EfRepository<TEntity> : IRepository<TEntity> where TEntity : class, new() // BaseEntity
    {
        private readonly IDbContext _dbContext;
        private DbSet<TEntity> _entities;

        protected DbSet<TEntity> Entities
        {
            get
            {
                if (_entities == null)
                {
                    _entities = _dbContext.Set<TEntity>();
                }
                return _entities;
            }
        }

        public IQueryable<TEntity> Table
        {
            get { return Entities; }
        }

        public EfRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TEntity> GetByIdAsync(object id) => await Entities.FindAsync(id);

        public async Task CreateAsync(TEntity entity)
        {
            try
            {
                if (entity == null) throw new ArgumentNullException(nameof(entity));

                Entities.Add(entity);

                await _dbContext.SaveChangesAsync();
            }
            catch(DbUpdateException dbEx)
            {
                throw new Exception(GetErrorText(dbEx), dbEx);
            }
        }

        public async Task UpdateAsync(TEntity entity)
        {
            try
            {
                if (entity == null) throw new ArgumentNullException(nameof(entity));

                Entities.Update(entity);

                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception(GetErrorText(dbEx), dbEx);
            }
        }

        public async Task DeleteAsync(TEntity entity)
        {
            try
            {
                if (entity == null) throw new ArgumentNullException(nameof(entity));

                Entities.Remove(entity);

                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception(GetErrorText(dbEx), dbEx);
            }
        }

        private string GetErrorText(DbUpdateException dbEx) => dbEx.Message;
    }
}
