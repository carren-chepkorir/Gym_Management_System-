using System;
using System.ComponentModel.DataAnnotations;

namespace ShapeShift.Models
{
    public class ShiftRequestViewModel
    {
        public int ShiftId { get; set; }
        public string ShiftName { get; set; }
        public string TrainerName { get; set; }

        public string MemberName { get; set; }
        public int  MemberId { get; set; }
        public DateTime ShiftTime { get; set; }

      
    }
}
