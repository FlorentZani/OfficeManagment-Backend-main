using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeManagment.DTOs;
using OfficeManagment.Model;

namespace OfficeManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private readonly DataContext _context;
        public PositionController(DataContext context)
        {
            _context = context;
        }


        [HttpGet("Get All Positions")]
            public async Task<ActionResult<List<PositionDTO>>> Get()
            {
                var positions = await _context.Positions.ToListAsync();
                var positionsData = positions.Select(positions => new
                {
                    Id = positions.Id,
                    Name = positions.Name,
                }).ToList();
                return Ok(positionsData);
            }


        [HttpGet("Get a single Position/{id}")]
        public async Task<ActionResult<object>> GetPosition(Guid id)
        {
            var position = await _context.Positions.FindAsync(id);

            if (position == null)
            {
                return NotFound(); 
            }

            
            return new
            {
                id = position.Id,
                Positions = position.Name
            };
        }


        [HttpPost]
        public async Task<IActionResult> AddPosition(PositionDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newPosition = new Position
            {
                Id = Guid.NewGuid(),
                Name = request.Name
            };

            _context.Positions.Add(newPosition);
            await _context.SaveChangesAsync();

            var newPositionDto = new PositionDTO
            {
                PositionId = newPosition.Id,
                Name = newPosition.Name
            };

            return Ok(newPositionDto);
        }


        [HttpPut("UpdatePosition /{id}")]

        public async Task<ActionResult> UpdateRole(Guid id, string name)
        {
            var updatePosition = await _context.Positions.FindAsync(id);
            updatePosition.Name = name;

            await _context.SaveChangesAsync();
            return Ok("Role Updated!");

        }


        [HttpDelete("DeletePosition/{id}")]
        public async Task<ActionResult> DeletePosition(Guid id)
        {
            var role = await _context.Positions.FindAsync(id);
            if (role == null)
            {
                return BadRequest("Role not found!");
            }
            _context.Positions.Remove(role);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }


}
