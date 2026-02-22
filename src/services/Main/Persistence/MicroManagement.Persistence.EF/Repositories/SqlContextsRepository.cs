using Mapster;
using MicroManagement.Core;
using MicroManagement.Persistence.Abstraction.Repositories;
using MicroManagement.Persistence.EF.Configuration;
using MicroManagement.Persistence.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace MicroManagement.Persistence.EF.Repositories;

public class SqlContextsRepository : IContextsRepository
{
    private readonly MyMicroManagementDbContext _dbContext;

    public SqlContextsRepository(MyMicroManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Context context)
    {
        var contextEntity = context.Adapt<ContextEntity>();

        _dbContext.Contexts.Add(contextEntity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Context>> GetAllAsync()
    {
        return await _dbContext
            .Contexts
            .Select(c => c.Adapt<Context>())
            .ToListAsync();
    }

    public async Task<Context> GetByIdAsync(Guid id)
    {
        var contextEntity = await _dbContext.Contexts.FindAsync(id);

        if (contextEntity == null)
            throw new KeyNotFoundException(nameof(Context.Id));

        return contextEntity.Adapt<Context>();
    }
}
