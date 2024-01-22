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
    public class OMController : ControllerBase
    {
        private readonly DataContext _context;
        public OMController(DataContext context)
        {
            _context = context;
        }
        //Find Users By Name
        [HttpGet("FindByName/{name}")]
        public async Task<ActionResult<object>> FindByName(string name)
        {
            
            var users = await _context.User
                .Where(u => u.Name == name)
                .Include(u => u.UserProjects)
                    .ThenInclude(up => up.Projects)
                .ToListAsync();

            var positions = await _context.Positions.ToListAsync();

            
            var userData = users.Select(user => new
            {
                UserId = user.Id,
                UserName = user.Name,
                Projects = user.UserProjects.Select(up => new
                {
                    ProjectId = up.ProjectId,
                    ProjectName = up.Projects.Name,
                    Positions = up.PositionIds.Select(id =>
                    {
                        var position = positions.FirstOrDefault(p => p.Id == id);
                        return position != null ? $"Position Name: {position.Name}  | Position Id: {position.Id}" : "Position not found";
                    }).ToList()
                }).ToList()
            }).ToList();

            return Ok(userData);
        }






    }
}
