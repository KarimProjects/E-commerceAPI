using Store.Data.Entities;
using Store.Repository.Specification;
using Store.Repository.Specification.Order;

namespace Store.Repository.Interfaces
{
    public interface IGenericRepository<TEntity,TKey> where TEntity : BaseEntity<TKey>
    {
        Task<TEntity> GetByIdAsync(TKey? id);
        Task<IReadOnlyList<TEntity>> GetAllAsync();
        Task<TEntity> GetWithSpecificationByIdAsync(ISpecification<TEntity> specs);
        Task<IReadOnlyList<TEntity>> GetAllWithSpecificationAsync(ISpecification<TEntity> specs);
        Task<int> CountSpecificationAsync(ISpecification<TEntity> specs);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
