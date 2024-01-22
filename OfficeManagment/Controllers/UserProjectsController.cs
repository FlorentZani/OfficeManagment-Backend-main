using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeManagment.DTOs;
using OfficeManagment.Model;

namespace OfficeManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProjectsController : ControllerBase
    {
        private readonly DataContext _context;
        public UserProjectsController(DataContext context)
        {
            _context = context;
        }
        //GetAllUserProjects
        [HttpGet]
        public async Task<ActionResult<List<object>>> Get()
        {
            var userProjects = await _context.UserProjects
                .Include(x => x.Projects)
                .Include(x => x.User)
                .ToListAsync();

            var userProjectsData = userProjects.Select(userProjects => new
            {
                UserProjectId = userProjects.Id,
                ProjectName = userProjects.Projects.Name,
                ProjectId = userProjects.ProjectId,
                UserId = userProjects.UserId,
                UserName = userProjects.User.Name,
                ProgrammingLanguage = userProjects.ProgrammingLanguage,
                WorkingHours = userProjects.WorkingHours,
                PositionIds = userProjects.PositionIds


            }).ToList();

            return Ok(userProjectsData);
        }

        //GetUserProjectById
        [HttpGet("GetSingleUserProject/{id}")]
        public async Task<ActionResult<object>> Get(Guid id)
        {
            var userProject = await _context.UserProjects
                .Include(x => x.Projects)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);

            var userProjectData = new
            {
                ProjectName = userProject.Projects.Name,
                ProjectId = userProject.ProjectId,
                UserId = userProject.UserId,
                UserName = userProject.User.Name,
                ProgrammingLanguage = userProject.ProgrammingLanguage,
                WorkingHours = userProject.WorkingHours,
                PositionIds = userProject.PositionIds

            };
            return Ok(userProjectData);
        }
        //Add a new UserProject
        [HttpPost]
        public async Task<ActionResult<UserProjects>> PostUserProject(UserProjectsDTO userProjectDto)
        {
            var positions = await _context.Positions.Where(p => userProjectDto.PositionIds.Contains(p.Id)).ToListAsync();

            if (positions.Count != userProjectDto.PositionIds.Count)
            {
                return BadRequest("GUID dosent match with Positions ID");
            }

            var userProject = new UserProjects
            {
                ProjectId = userProjectDto.ProjectId,
                UserId = userProjectDto.UserId,
                PositionIds = userProjectDto.PositionIds,
                WorkingHours = userProjectDto.WorkingHours,
                ProgrammingLanguage = userProjectDto.ProgrammingLanguage,

            };

            _context.UserProjects.Add(userProject);
            await _context.SaveChangesAsync();

            return Ok("UserProject Added");
        }


        //Update UserProjects
        [HttpPut("UpdateUserProject/{id}")]
        public async Task<ActionResult> Update(Guid id, UserProjectsDTO request)
        {
            var UpdatedUserProject = await _context.UserProjects.FindAsync(id);

            if (UpdatedUserProject == null)
            {
                return BadRequest("UserProjects Not Found!");
            }

            UpdatedUserProject.ProjectId = request.ProjectId;
            UpdatedUserProject.UserId = request.UserId;
            UpdatedUserProject.PositionIds = request.PositionIds;
            UpdatedUserProject.WorkingHours = request.WorkingHours;
            UpdatedUserProject.ProgrammingLanguage = request.ProgrammingLanguage;


            await _context.SaveChangesAsync();

            return Ok(UpdatedUserProject);
        }

        //Delete UserProjects
        [HttpDelete("DeleteUserProject/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var UserProject = await _context.UserProjects.FindAsync(id);
            if (UserProject == null)
            {
                return BadRequest("UserProjects Not Found!");
            }

            _context.UserProjects.Remove(UserProject);
            await _context.SaveChangesAsync();

            return Ok("UserProject Deleted");
        }

    }
}
