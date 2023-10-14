using System;
using System.ComponentModel.DataAnnotations;

namespace ShapeShift.Models
{
    public class ShiftRequest
    {
        [Key]
        public int ShiftRequestId { get; set; }

        // Other properties related to the shift request

        public int MemberID { get; set; }
        public Member Member { get; set; }

        public int ShiftID { get; set; }
        public Shift Shift { get; set; }
        [Required]
        public string MemberName { get; set; }
        public string TrainerName { get; set; }

        public DateTime RequestedDate { get; set; }
    }
}
