using APIKros.Models;

namespace APIKros.Repositories;


public interface IRepository<T, TK> where T : class, IModel where TK : IEquatable<TK>, IComparable<TK>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(TK id);
    Task<T> CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(TK id);

    Task<bool> ExistsAsync(TK id);
    
    Task<bool> SaveChangesAsync();
    
}