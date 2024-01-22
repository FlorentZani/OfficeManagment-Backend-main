using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OfficeManagment.Model;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace OfficeManagment.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        //Register Method 
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserRegistration request)
        {
            if (await _context.User.AnyAsync(u => u.Username == request.Username))
            {
                return BadRequest("Username already exists. Please choose a different username.");
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

            UserRole newUserRole = new UserRole
            {
                UserId = newUser.Id,
                RoleId = Guid.Parse("34f98882-c607-41aa-e8b6-08db98b5673b"),

            };

            _context.UserRoles.Add(newUserRole);
            await _context.SaveChangesAsync();
            
            

            return Ok();
        }
        //-----------------------------------------------------------------------------------------------------------------\

        //Create passwordHash and passwordSalt
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        //----------------------------------------------------------------------------------------------------

        //method to create token
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username)
        };

            if (user.Role != null)
            {
                foreach (var userRole in user.Role)
                {
                    var Role = userRole.Roles.FullName;
                    claims.Add(new Claim(ClaimTypes.Role, Role));
                }
            }

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }


        [HttpPost("Login")]
        public async Task<ActionResult<User>> Login(UserLogin login)
        {
            
            var user = await _context.User
                                     .Include(u => u.Role)
                                     .ThenInclude(ur => ur.Roles)
                                     .FirstOrDefaultAsync(u => u.Username == login.Username);

            if (user == null)
            {
                return BadRequest("Invalid credentials");
            }

            if (!VerifyPasswordHash(login.Password, user.passwordHash, user.passwordSalt))
            {
                return BadRequest("Invalid credentials");
            }

            string token = CreateToken(user);
            return Ok(token);
        }






        private bool VerifyPasswordHash(string inputedPassword, byte[] userPasswordHash, byte[] userPasswordSalt)
        {
            using (var hmac = new HMACSHA512(userPasswordSalt))
            {
                byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(inputedPassword));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != userPasswordHash[i])
                    {
                        return false; 
                    }
                }

                return true; 
            }
        }




        

        




    }
}

