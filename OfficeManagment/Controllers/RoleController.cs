using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeManagment.DTOs;
using OfficeManagment.Model;

namespace OfficeManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly DataContext _context;
        public RoleController(DataContext context)
        {
            _context = context;
        }

        //GetAllRoles
        [HttpGet("Get All Roles")]
        public async Task<ActionResult<List<object>>> Get() 
        {
            var roles = await _context.Role.ToListAsync();
            var rolesData = roles.Select(role => new
            {
                Id = role.Id,
                Role = role.FullName,

            }).ToList();

            return Ok(rolesData);
        }
        //Get a single role
        [HttpGet("Get a single role/{id}")]
        public async Task<ActionResult<object>> GetRole(Guid id)
        {
            var role = await _context.Role.FindAsync(id);
            return new
            {
                Role = role.FullName
            };
        }

        //Add a new Role
        [HttpPost("AddNewRole")]
        public async Task<ActionResult> AddRole(RoleDTO request)
        {
            var newRole = new Role();

            newRole.FullName = request.FullName;

            await _context.Role.AddAsync(newRole);
            await _context.SaveChangesAsync();

            return Ok("Added new role!");
        }

        //Update Role
        [HttpPut("UpdateRole /{id}")]

        public async Task<ActionResult> UpdateRole (Guid id, string name)
        {
            var updateRole = await _context.Role.FindAsync(id);
            updateRole.FullName = name;

            await _context.SaveChangesAsync();
            return Ok("Role Updated!");

        }

        //Delete Role 
        [HttpDelete("DeleteRole/{id}")]
        public async Task<ActionResult> DeleteRole(Guid id)
        {
            var role = await _context.Role.FindAsync(id);
            if (role == null)
            {
                return BadRequest("Role not found!");
            }
            _context.Role.Remove(role);
            await _context.SaveChangesAsync();
            return Ok();
        }



    }
}
