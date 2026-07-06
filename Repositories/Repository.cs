using APIKros.Data;
using APIKros.Models;
using Microsoft.EntityFrameworkCore;

namespace APIKros.Repositories;

public class Repository<T, TK> : IRepository<T, TK> where T : class, IModel where TK : IEquatable<TK>, IComparable<TK>
{
    protected readonly AppDbContext DbContext;

    public Repository(AppDbContext db)
    {
        DbContext = db;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await DbContext.Set<T>().ToListAsync();

    }

    public async Task<T?> GetByIdAsync(TK id)
    {
        return await DbContext.Set<T>().FindAsync(id);
    }

    public async Task<T> CreateAsync(T entity)
    {
        _ = await DbContext.Set<T>().AddAsync(entity);

        return entity;
    }


    public async Task UpdateAsync(T entity)
    {
        DbContext.Set<T>().Update(entity);
    }

    public async Task DeleteAsync(TK id)
    {
        var entity = await DbContext.Set<T>().FindAsync(id);

        if (entity == null)
            return;

        DbContext.Set<T>().Remove(entity);
    }

    public async Task<bool> ExistsAsync(TK id)
    {
        return await DbContext.Companies.AnyAsync(c => c.Id.Equals(id));
    }

    public async Task<bool> SaveChangesAsync()
    {
        await DbContext.SaveChangesAsync();
        return true;
    }
}