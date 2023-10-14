using System;
using System.ComponentModel.DataAnnotations;

namespace ShapeShift.Models
{
    public class Shift
    {
        [Key]
        public int ShiftID { get; set; }
        public string ShiftName { get; set; }
        public DateTime ShiftTime { get; set; }
        public int TrainerID { get; set; }
        public Trainer Trainer { get; set; }
    }
}

