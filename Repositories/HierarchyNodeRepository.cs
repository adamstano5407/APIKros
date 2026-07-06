using System.Linq.Expressions;
using APIKros.Data;
using APIKros.Models;
using Microsoft.EntityFrameworkCore;

namespace APIKros.Repositories;

public abstract class HierarchyNodeRepository<T, TK>
    : Repository<T, TK>, IHierarchyNodeRepository<T, TK>
    where T : HierarchyNode
    where TK : IEquatable<TK>, IComparable<TK>
{
    
    protected HierarchyNodeRepository(AppDbContext db)
        : base(db)
    {
    }
    
    public virtual async Task UnassignManagerAsync(TK id)
    {
        await DbContext.Set<T>().AsNoTracking().Where(x => x.Id.Equals(id)).ExecuteUpdateAsync(setters => setters.SetProperty(x => x.ManagerId, _ => null ));
    }
    
    public virtual async Task<Employee?> GetManagerOfNodeAsync(TK id)
    {
        return await DbContext.Set<T>().AsNoTracking().Where(x => x.Id.Equals(id)).Select(x => x.Manager)
            .FirstOrDefaultAsync();
    }
    
}