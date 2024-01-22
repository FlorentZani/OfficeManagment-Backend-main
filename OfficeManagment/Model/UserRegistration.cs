namespace OfficeManagment.Model
{
    public class UserRegistration
    {
        public string Username { get; set; }    
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
