using System;
using MicroManagement.Core;

namespace MicroManagement.Persistence.Abstraction.Repositories;

public interface IContextsRepository
{
    Task<IEnumerable<Context>> GetAllAsync();
    Task<Context> GetByIdAsync(Guid id);
    Task AddAsync(Context context);
    Task UpdateAsync(Context context);
}
