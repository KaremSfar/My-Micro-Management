using MicroManagement.Services.Abstraction;
using MicroManagement.Services.Abstraction.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MicroManagement.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectsService _projectsService;

        public ProjectsController(IProjectsService projectsService)
        {
            _projectsService = projectsService;
        }

        /// <summary>
        /// Endpoint to return all project for the current logged-in user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var projects = await _projectsService.GetAll(GetUserId());
            return Ok(projects);
        }

        /// <summary>
        /// Endpoint used to add a project to the current logged-in user's collection 
        /// </summary>
        /// <param name="projectDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(CreateProjectDTO projectDTO)
        {
            var addedProject = await _projectsService.AddProject(GetUserId(), projectDTO);
            return Ok(addedProject);
        }

        private Guid GetUserId()
            => Guid.Parse(User.Identities.First().Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
    }
}
