using System.ComponentModel.DataAnnotations;

namespace OfficeManagment.Model
{
    public class Role: BaseClass
    {
        public string FullName { get; set; }
        public List<UserRole> Users { get; set; }


    }
}
