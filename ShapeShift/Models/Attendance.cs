using System.ComponentModel.DataAnnotations;

namespace ShapeShift.Models
{
    public class Attendance
    {
        [Key]
        public int AttendanceID { get; set; }
        public int MemberID { get; set; }
        public Member Member { get; set; }
        public int ShiftID { get; set; }
        public Shift Shift { get; set; }
        public DateTime AttendanceDate { get; set; }
        public bool Present { get; set; }
    }
}
