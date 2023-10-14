namespace ShapeShift.Models
{
    public class TrainerDashboardViewModel
    {
        public int TrainerId { get; set; }
        public List<Shift> UpcomingShifts { get; set; }
        public List<Attendance> AttendanceRecords { get; set; }
    }
}
