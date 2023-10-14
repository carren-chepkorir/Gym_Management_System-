using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShapeShift.Models
{
    public class TrainerLoginViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Display(Name ="Remember Me")]
        public bool RememberMe { get; set; }
    }
}
