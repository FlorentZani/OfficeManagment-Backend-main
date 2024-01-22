using OfficeManagment.Model;
using System.Text.Json.Serialization;

namespace OfficeManagment.DTOs
{
    public class UserDTO : BaseClass
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }




    }
}
