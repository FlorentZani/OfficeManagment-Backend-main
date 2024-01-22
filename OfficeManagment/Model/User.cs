using OfficeManagment.Model;
using System.ComponentModel.DataAnnotations;

namespace OfficeManagment.Model
{
    public class User : BaseClass
    {
        public string Username { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public byte[] passwordHash { get; set; }

        public byte[] passwordSalt { get; set; }
        public List<UserRole> Role { get; set; }
        public List<UserProjects> UserProjects { get; set; }

    }
}
