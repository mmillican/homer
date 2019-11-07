using System.Linq;
using System.Threading.Tasks;

namespace Homer.Shared.Data
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Table { get; }

        Task<TEntity> GetByIdAsync(object id);

        Task CreateAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}
