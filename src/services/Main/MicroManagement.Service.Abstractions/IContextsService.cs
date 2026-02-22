using MicroManagement.Services.Abstraction.DTOs;

namespace MicroManagement.Service.Abstractions;

public interface IContextsService
{
    Task<IEnumerable<GetContextDTO>> GetAllAsync();
    Task<GetContextDTO> CreateAsync(Guid userId, CreateContextDTO createDto);
    Task<GetContextDTO> UpdateAsync(Guid contextId, UpdateContextDTO updateDto);
}
