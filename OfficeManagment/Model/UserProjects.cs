namespace OfficeManagment.Model
{
    public class UserProjects : BaseClass
    {
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
        //public Guid PositionId { get; set; }
        public List<Guid> PositionIds { get; set; }
        public int WorkingHours { get; set; }
        public string ProgrammingLanguage { get; set; }

        //public Position Position { get; set; }
        public User User { get; set; }
        public Projects Projects { get; set; }
    }

}
