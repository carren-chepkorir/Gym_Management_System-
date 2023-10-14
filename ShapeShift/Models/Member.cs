using System.ComponentModel.DataAnnotations;

namespace ShapeShift.Models
{
    public class Member
    {
        [Key]
        public int MemberID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        
        public int? TrainerID { get; set; }
        public Trainer Trainer { get; set; }
     
    }
}
