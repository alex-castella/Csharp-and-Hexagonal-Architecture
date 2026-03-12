using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Abstraction
{
    public interface IRepository<TEntity, TId> where TEntity : class // Make sure TEntity is a clas
    {
        Task<TEntity?> GetByIdAsync(TId id); // ? Nullable; Task Async
        Task<IEnumerable<TEntity>>  GetAllAsync(); //IEnumerable == Iterable Java
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<int> SaveChangesAsync();// In case ORM of .NET is not use to specicy method??

    }
}
