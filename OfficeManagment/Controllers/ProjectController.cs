using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeManagment.DTOs;
using OfficeManagment.Model;


namespace OfficeManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly DataContext _context;
        public ProjectController(DataContext context)
        {
            _context = context;
        }

        //Add a project
        [HttpPost("Add a project")]
        public async Task<IActionResult> AddProject(ProjectsDTO request)
        {
            var newProject = new Projects
            {
                Name = request.Name,
                ClientName = request.ClientName,
                EndDate = request.EndDate,
                NumberOfDevelopers = request.NumberOfDevelopers,
            };

            _context.Projects.Add(newProject);
            await _context.SaveChangesAsync();
            return Ok();

        }

        //-----------------------------------------------------------------------------------------------------------------

        //Get All Projects

        [HttpGet("Get All Projects")]
        public async Task<ActionResult<List<object>>> GetAllProjects()
        {
            var projects = await _context.Projects.ToListAsync();

            var projectsData = projects.Select(p => new { 
                ProjectId = p.Id,
                Name = p.Name,
                ClientName = p.ClientName,
                EndDate = p.EndDate,
                NumberOfDevelopersWorking = p.NumberOfDevelopers
            }).ToList();

            return Ok(projectsData);
        }

        //Get project by id 
        [HttpGet("Get project by id/{id}")]
        public async Task<ActionResult<object>> GetProjectById(Guid id)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(u => u.Id == id);

            return new
            {
                Name = project.Name,
                ClientName = project.ClientName,
                EndDate = project.EndDate,
                NumberOfDevWorking = project.NumberOfDevelopers
            };

        }

        //Update UserProjects
        [HttpPut("UpdateProject/{id}")]

        public async Task<ActionResult> UpdateProject(Guid id ,ProjectsDTO updatedProject)
        {
            var project = await _context.Projects.FindAsync(id);
            if(project == null)
            {
                return BadRequest("UserProjects Not Found!");
            }
            project.ClientName = updatedProject.ClientName;
            project.EndDate = updatedProject.EndDate;
            project.Name = updatedProject.Name;
            project.NumberOfDevelopers = updatedProject.NumberOfDevelopers;

            await _context.SaveChangesAsync();

            return Ok("UserProjects Updated Succesfully!");




        }

        //Delte UserProjects
        [HttpDelete]

        public async Task<ActionResult> DeleteProject(Guid id)
        {

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return BadRequest("UserProjects not found!");
            }

            _context.Projects.Remove(project);
            _context.SaveChangesAsync();

            return Ok("UserProjects Delted Sucessfully!");

        }





    }
}
