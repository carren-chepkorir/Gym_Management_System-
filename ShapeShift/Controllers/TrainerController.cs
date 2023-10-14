using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShapeShift.Models;

namespace ShapeShift.Controllers
{
    public class TrainerController : Controller
    {
        private readonly GymDbContext _dbContext;
        private int trainerId;

        public TrainerController(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [Authorize(Policy ="RequireTrainerRole")]
        public IActionResult TrainerDashboard()
        {
            // Assuming you have a way to identify the currently logged-in trainer, e.g., through authentication
             // Implement this method as per your authentication mechanism

            // Retrieve upcoming shifts for the trainer
            var upcomingShifts = _dbContext.Shifts
                .Where(s => s.TrainerID == trainerId && s.ShiftTime >= DateTime.Now)
                .OrderBy(s => s.ShiftTime)
                .ToList();

            // Retrieve attendance records for the trainer's shifts
        var attendanceRecords = _dbContext.Attendances
    .Where(a => a.AttendanceDate >= DateTime.Today)
    .Join(
        _dbContext.Shifts,
        attendance => attendance.ShiftID,
        shift => shift.ShiftID,
        (attendance, shift) => new { Attendance = attendance, Shift = shift }
    )
    .Where(joinResult => joinResult.Shift.TrainerID == trainerId)
    .Select(joinResult => joinResult.Attendance)
    .ToList();


            // Create a view model to pass data to the view
            var viewModel = new TrainerDashboardViewModel
            {
                TrainerId = trainerId,
                UpcomingShifts = upcomingShifts,
                AttendanceRecords = attendanceRecords // Use the correct variable name here
            };

            return View(viewModel);
        }


        // Display the attendance form for a specific shift
        public IActionResult MarkAttendance(int shiftId)
        {
            // Retrieve the shift by ID
            var shift = _dbContext.Shifts.FirstOrDefault(s => s.ShiftID == shiftId);

            if (shift == null)
            {
                return NotFound(); // Shift not found
            }

            // Create a view model to pass data to the view (e.g., members in the shift)
            var viewModel = new MarkAttendanceViewModel
            {
                Shift = shift,
                MembersInShift = _dbContext.Members.Where(m => m.TrainerID == shift.TrainerID).ToList()
            };

            return View(viewModel);
        }

        // Handle the submission of attendance
        [HttpPost]
        public IActionResult MarkAttendance(MarkAttendanceViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Loop through selected member IDs and mark them as present
                foreach (int memberId in viewModel.SelectedMemberIds)
                {
                    var attendanceRecord = new Attendance
                    {
                        MemberID = memberId,
                        ShiftID = viewModel.Shift.ShiftID,
                        AttendanceDate = DateTime.Today,
                        Present = true // You can set this based on user input
                    };

                    _dbContext.Attendances.Add(attendanceRecord);
                }

                _dbContext.SaveChanges();

                // Redirect back to the trainer's dashboard or another appropriate page
                return RedirectToAction("TrainerDashboard");
            }

            // If there are validation errors, return to the attendance form with errors
            return View(viewModel);
        }
       
    }
}
