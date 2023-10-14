using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ShapeShift.Models
{
    public class Trainer: IdentityUser
    {
        [Key]
        public int TrainerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public ICollection<Shift> Shifts { get; set; }
        public ICollection<Member> Members { get; set; }
    }
}
