using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using ShapeShift.Models;
using System.Text;


namespace ShapeShift.Models
{

    public class AdminController : Controller
    {
        private readonly GymDbContext _dbContext;
        private readonly GymDbContext _fileService;


        public AdminController(GymDbContext dbContext, GymDbContext fileService)
        {
            _dbContext = dbContext;
            _fileService = fileService;

        }

        // ...

        // Admin Dashboard
        public IActionResult AdminDashboard()
        {
            var dashboard = new AdminDashboardViewModel
            {
                TotalMembers = _dbContext.Members.Count(),
                TotalTrainers = _dbContext.Trainers.Count(),


            };
            return View(dashboard);

        }
        public async Task<IActionResult> Index()
        {
            return _dbContext.Members != null ?
                           View(await _dbContext.Members.Include(t=>t.Trainer).ToListAsync()) :
                           Problem("Entity set 'GymDbcontext.Members'  is null.");
        }




        [HttpGet]
        public IActionResult AddMemberView()
        {
           
            //select methode here is used to project the trainersvin db into annonymous type that includes Fullname and Trainerid .
            var trainers = _dbContext.Trainers.Select(t => new
            {
                TrainerID = t.TrainerID,
                FullName=t.FirstName +" "+t.LastName
            })
                .ToList();
            ViewData["Trainers"]=new SelectList(trainers, "TrainerID","FullName");
            // Display the "Add Member" form
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddMemberView(int memberId,int trainerId, string firstName, string lastName, string phone, string email)
        {
            var memberInfo = new Member
            {
                MemberID = memberId,
                FirstName = firstName,
                LastName = lastName,
                TrainerID=trainerId,
                Phone = phone,
                Email = email
            };
            _dbContext.Add(memberInfo);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("AdminDashboard");

        }
        [HttpGet]
        public async Task<IActionResult> DeleteMember(int? memberId)
        {
            if (memberId == null || _dbContext.Members == null)
            {
                return NotFound();
            }

            var member = await _dbContext.Members.FirstOrDefaultAsync(x => x.MemberID == memberId);

            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // Delete member
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMember(int memberId)
        {
            if (_dbContext.Members == null)
            {
                return Problem("Members not available now");
            }

            // Fetch the member from the database by memberId
            var memberToDelete = await _dbContext.Members.FindAsync(memberId);

            if (memberToDelete == null)
            {
                // Member not found, return NotFound
                return NotFound();
            }

            // Remove the member from the database
            _dbContext.Members.Remove(memberToDelete);

            // Save changes to the database
            await _dbContext.SaveChangesAsync();

            // Redirect to the desired action after successful deletion
            return RedirectToAction("Index,Admin");
        }


        // Modify trainer shifts

        public IActionResult AddShiftViewList()
        {
            var shifts = _dbContext.Shifts.ToList();
            return View(shifts);
        }

        [HttpGet]
        public IActionResult AddShift()
        {
            
            return View();

        }
        [HttpPost]
        public async Task< IActionResult> AddShift(int shiftId,string shiftName,DateTime shiftTime,int trainerId)
        {
            var shift = new Shift
            {
                ShiftID = shiftId, 
                ShiftName = shiftName,
                ShiftTime = shiftTime,
                TrainerID=trainerId
                

            };
            _dbContext.Shifts.Add(shift);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("AddShiftViewList");
        }
        [HttpGet]
        public IActionResult ConfirmDeleteShift( int shiftId)
        {
           
            if (shiftId == null||_dbContext.Shifts==null)
            {
                return NotFound();

            }
            var shift = _dbContext.Shifts.FirstOrDefault(s => s.ShiftID == shiftId);

            if (shift!=null)
            {
                return NotFound();

            }
            return View(shift);


        }
        [HttpPost]
        public async Task< IActionResult> Delete(int shiftId)
        {
            if (_dbContext.Shifts == null)
            {
                return Problem("Shift Not available now");
            }
            var shift = await _dbContext.Shifts.FindAsync(shiftId);
            if (shift != null)
            {
                _dbContext.Shifts.Remove(shift);



                

            }
            return RedirectToAction("AdminDashboard");


        }
        [HttpGet]
        public IActionResult EditShift( int shiftId) {
          var shifts=_dbContext.Shifts.FirstOrDefault(s=>s.ShiftID==shiftId);
            if (shiftId == null || shifts == null)
            {
                return NotFound();
            }
            
            return View(shifts);


        }
        [HttpPost]
        public IActionResult EditShift(Shift shiftData ,int shiftId)
        {
            if (ModelState.IsValid) {
                var shifts = _dbContext.Shifts.FirstOrDefault(i => i.ShiftID == shiftId);


                if (shifts != null) {
                    return NotFound();
                }


                shifts.ShiftName=shiftData.ShiftName;
                shifts.ShiftTime=shiftData.ShiftTime;
                _dbContext.SaveChanges();
                    }
            return RedirectToAction();

        }
        

        [HttpGet]
        public IActionResult ModifyTrainerShift()
        {
            // Populate ViewBag.Trainers with a list of available trainers
            ViewBag.Trainers = _dbContext.Trainers.ToList();

            // Populate ViewBag.Shifts with a list of available shifts
            ViewBag.Shifts = _dbContext.Shifts.ToList();

            // Create a new instance of ModifyTrainerShiftModel
            var model = new ModifyTrainerShiftModel();

            return View(model);
        }

        [HttpPost]
        public IActionResult ModifyTrainerShift(ModifyTrainerShiftModel model)
        {
            // Check if the model is valid (i.e., both TrainerId and NewShiftId are selected)
            if (ModelState.IsValid)
            {
                // Retrieve trainer and new shift based on model.TrainerId and model.NewShiftId
                var trainer = _dbContext.Trainers
                    .Include(t => t.Shifts)
                    .FirstOrDefault(t => t.TrainerID == model.TrainerId);

                var newShift = _dbContext.Shifts.Find(model.NewShiftId);

                if (trainer != null && newShift != null)
                {
                    // Assign the new shift to the trainer
                    trainer.Shifts.Add(newShift);
                    _dbContext.SaveChanges();
                }

                // Redirect to the admin dashboard after successfully modifying the shift
                return RedirectToAction("AdminDashboard");
            }

            // If there are validation errors, repopulate ViewBag data and return to the view
            ViewBag.Trainers = _dbContext.Trainers.ToList();
            ViewBag.Shifts = _dbContext.Shifts.ToList();

            return View(model);
        }
        public IActionResult AddTrainerViewList()
        {
            var trainers = _dbContext.Trainers.ToList();
            return View(trainers);
        }
        public IActionResult AddTrainer()
        {
            return View();
        }
        [HttpPost]
        public async  Task <IActionResult> AddTrainer(int trainerId,string firstName,string lastName,string phone,string email)
        {
            var newInfo = new Trainer
            {
                TrainerID = trainerId,
                FirstName = firstName,
                LastName = lastName,
                Phone = phone,
                Email = email


            };
            _dbContext.Add(newInfo);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("AddTrainerViewList");
           


        }
     
     
        public IActionResult DeleteTrainer(int trainerId)
        {
            var trainer = _dbContext.Trainers.Find(trainerId);

            if (trainer != null)
            {
                // Remove the trainer from the database
                _dbContext.Trainers.Remove(trainer);
                _dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        // Generate weekly attendance register
        public IActionResult GenerateWeeklyAttendanceRegisterCsv()
        {
            // Fetch attendance records for the desired week
            DateTime startDateOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            DateTime endDateOfWeek = startDateOfWeek.AddDays(6);

            var attendanceRecords = _dbContext.Attendances
                .Include(a => a.Member)
                .Include(a => a.Shift)
                .Where(a => a.AttendanceDate >= startDateOfWeek && a.AttendanceDate <= endDateOfWeek)
                .ToList();

            // Generate CSV content
            var csvContent = new StringBuilder();
            csvContent.AppendLine("MemberID,MemberFirstName,MemberLastName,ShiftName,AttendanceDate,Present");

            foreach (var record in attendanceRecords)
            {
                csvContent.AppendLine($"{record.MemberID},{record.Member.FirstName} {record.Member.LastName},{record.Shift.ShiftName},{record.AttendanceDate},{record.Present}");
            }

            // Convert CSV content to bytes
            byte[] csvBytes = Encoding.UTF8.GetBytes(csvContent.ToString());

            // Set the file name and content type
            string fileName = $"WeeklyAttendanceRegister_{startDateOfWeek:yyyyMMdd}_{endDateOfWeek:yyyyMMdd}.csv";
            string contentType = "text/csv";

            // Return the CSV file as a response
            return File(csvBytes, contentType, fileName);
        }

        // Generate Weekly Trainer Shift Report as CSV
        public IActionResult GenerateWeeklyTrainerShiftReportCsv()
        {
            // Fetch trainer shift records for the desired week
            DateTime startDateOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            DateTime endDateOfWeek = startDateOfWeek.AddDays(6);

            var shiftRecords = _dbContext.Shifts
                .Include(s => s.Trainer)
                .Where(s => s.ShiftTime >= startDateOfWeek && s.ShiftTime <= endDateOfWeek)
                .ToList();

            // Generate CSV content
            var csvContent = new StringBuilder();
            csvContent.AppendLine("TrainerID,TrainerName,ShiftName,ShiftTime");

            foreach (var record in shiftRecords)
            {
                csvContent.AppendLine($"{record.TrainerID},{record.Trainer.FirstName} {record.Trainer.LastName},{record.ShiftName},{record.ShiftTime}");
            }

            // Convert CSV content to bytes
            byte[] csvBytes = Encoding.UTF8.GetBytes(csvContent.ToString());

            // Set the file name and content type
            string fileName = $"WeeklyTrainerShiftReport_{startDateOfWeek:yyyyMMdd}_{endDateOfWeek:yyyyMMdd}.csv";
            string contentType = "text/csv";

            // Return the CSV file as a response
            return File(csvBytes, contentType, fileName);
        }


    }



}



