namespace OfficeManagment.Model
{
    public class Projects: BaseClass
    {
        public string Name { get; set; }
        public string ClientName { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfDevelopers { get; set; }
        public List<UserProjects> UserProjects { get; set; }
    }
}