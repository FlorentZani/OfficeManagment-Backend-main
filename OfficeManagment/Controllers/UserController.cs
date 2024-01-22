using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeManagment.Model;
using OfficeManagment.DTOs;
using OfficeManagment.Data;
using Azure.Core;
using System.Security.Cryptography;

namespace OfficeManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        //Get All Users  Method for Users
        [HttpGet("Get All Users")]
        public async Task<ActionResult<List<UserDTO>>> GetAllUsers()
        {
            var users = await _context.User.ToListAsync();
            List<UserDTO> usersDTO = users.Select(user => new UserDTO
            {
                Username = user.Username,
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
            }).ToList();

            return Ok(usersDTO);
        }

        //-------------------------------------------------------------------------------------------------------------------
        //Get a single user using ID 

        [HttpGet("Get User By Id/{id}")]
        public async Task<ActionResult<UserDTO>> GetByID(Guid id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return BadRequest("User Not Found!");
            }
            var userDTO = new UserDTO
            {
                Username = user.Username,
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,

            };

            return Ok(userDTO);
        }

        //---------------------------------------------------------------------------------------------------------------------
        //Update user inforamtion except his GUID 

        [HttpPut("Update User/{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, UserUpdateDTO updatedUser)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
                return NotFound();


            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(updatedUser.Password, out passwordHash, out passwordSalt);

            user.Name = updatedUser.Name;
            user.LastName = updatedUser.LastName;
            user.Username = updatedUser.Username;
            user.passwordHash = passwordHash;
            user.passwordSalt = passwordSalt;


            await _context.SaveChangesAsync();



            return Ok("Update Gone Successfully");
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------

        //Delete User 
        [HttpDelete("Delete User/{id}")]

        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return BadRequest("User not found!");
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return Ok("User delted successfully!");
        }


        //------------------------------------------------------------------------------------------------------------
        //Add new user 
        [HttpPost("Add new user")]
        public async Task<ActionResult<User>> Register(UserUpdateDTO request)
        {
            if (await _context.User.AnyAsync(u => u.Username == request.Username))
            {
                return BadRequest("Name already exists. Please choose a different name.");
            }

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(request.Password, out passwordHash, out passwordSalt);

            User newUser = new User
            {
                Username = request.Username,
                Name = request.Name,
                LastName = request.LastName,
                passwordSalt = passwordSalt,
                passwordHash = passwordHash
            };

            _context.User.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok();
        }





    }
}
