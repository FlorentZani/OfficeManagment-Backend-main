using OfficeManagment.Model;

namespace OfficeManagment.DTOs
{
    public class UserProjectsDTO
    {
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
        public List<Guid> PositionIds { get; set; } 
        public int WorkingHours { get; set; }
        public string ProgrammingLanguage { get; set; }
    }

}
