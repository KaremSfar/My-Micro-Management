using MicroManagement.Service.Abstractions;
using MicroManagement.Services.Abstraction.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MicroManagement.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContextsController : ControllerBase
    {
        private readonly IContextsService _contextsService;

        public ContextsController(IContextsService contextsService)
        {
            _contextsService = contextsService;
        }

        /// <summary>
        /// Endpoint to return all contexts
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var contexts = await _contextsService.GetAllAsync();
            return Ok(contexts);
        }

        /// <summary>
        /// Endpoint used to create a new context for the current logged-in user
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(CreateContextDTO createDto)
        {
            var createdContext = await _contextsService.CreateAsync(GetUserId(), createDto);
            return Ok(createdContext);
        }

        /// <summary>
        /// Endpoint used to update an existing context
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, UpdateContextDTO updateDto)
        {
            var updatedContext = await _contextsService.UpdateAsync(id, updateDto);
            return Ok(updatedContext);
        }

        private Guid GetUserId()
            => Guid.Parse(User.Identities.First().Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
    }
}
