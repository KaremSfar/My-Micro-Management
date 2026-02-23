using System;
using MicroManagement.Core;
using MicroManagement.Persistence.Abstraction.Repositories;
using MicroManagement.Service.Abstractions;
using MicroManagement.Services.Abstraction.DTOs;

namespace MicroManagement.Service.WebAPI.Services;

public class ContextService : IContextsService
{
    private readonly IContextsRepository _contextsRepository;

    public ContextService(IContextsRepository contextsRepository)
    {
        _contextsRepository = contextsRepository;
    }

    public async Task<IEnumerable<GetContextDTO>> GetAllAsync(Guid userId)
    {
        var contexts = await _contextsRepository.GetAllAsync(userId);

        return contexts.Select(c => new GetContextDTO
        {
            Id = c.Id,
            Name = c.Name,
            Icon = c.Icon
        }).OrderBy(c => c.Name == "Default" ? 0 : 1);
    }

    public async Task<GetContextDTO> CreateAsync(Guid userId, CreateContextDTO createDto)
    {
        var context = new Context
        {
            Id = Guid.NewGuid(),
            Name = createDto.Name!,
            Icon = createDto.Icon!,
            UserId = userId
        };

        await _contextsRepository.AddAsync(context);

        return new GetContextDTO
        {
            Id = context.Id,
            Name = context.Name,
            Icon = context.Icon
        };
    }

    public async Task<GetContextDTO> UpdateAsync(Guid contextId, UpdateContextDTO updateDto)
    {
        var context = new Context
        {
            Id = contextId,
            Name = updateDto.Name!,
            Icon = updateDto.Icon!
        };

        await _contextsRepository.UpdateAsync(context);

        return new GetContextDTO
        {
            Id = context.Id,
            Name = context.Name,
            Icon = context.Icon
        };
    }
}
