using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Abstraction
{
    public interface ICodeRepository<TEntity> where TEntity : class //Intrface for non CRUD methods
    {
        Task<TEntity?> GetByCodeAsync(string code);
        Task <bool> ExistsWithCodeAsync(string code);
    }
}
