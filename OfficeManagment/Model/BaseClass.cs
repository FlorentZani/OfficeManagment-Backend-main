using System.ComponentModel.DataAnnotations;

namespace OfficeManagment.Model
{
    public class BaseClass
    {
        [Key]
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
