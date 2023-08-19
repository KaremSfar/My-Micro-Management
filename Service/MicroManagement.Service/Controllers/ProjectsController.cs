using MicroManagement.Services.Abstraction;
using MicroManagement.Services.Abstraction.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var projects = await _projectsService.GetAll();
            return Ok(projects);
        }

        [HttpPost]
        public async Task<IActionResult> Post(ProjectDTO projectDTO)
        {
            var addedProject = await _projectsService.AddProject(projectDTO);
            return Ok(addedProject);
        }
    }
}
