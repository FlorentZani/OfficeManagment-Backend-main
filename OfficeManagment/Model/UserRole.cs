namespace OfficeManagment.Model
{
    public class UserRole : BaseClass
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public Role Roles { get; set;}
        public User User { get; set; }

    }
}
