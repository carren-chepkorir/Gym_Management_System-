using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShapeShift.Models;
using System;
using System.Linq;

namespace ShapeShift.Models
{
    public class MembersController : Controller
    {
        private readonly GymDbContext _dbContext;

        public int ShiftID { get; private set; }

        public MembersController(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Member Dashboard
        public IActionResult MemberDashboard()
        {
            // Retrieve and display available shifts to the member
            var availableShifts = _dbContext.Shifts.Include(s=>s.Trainer)
                .Where(s => s.ShiftTime >= DateTime.Today)
                .ToList();

            return View(availableShifts);
        }

        // GET: RequestShift
        public IActionResult RequestShift(int shiftId)
        {
            // Fetch the selected shift from the database
            var selectedShift = _dbContext.Shifts.Include(t=>t.Trainer).FirstOrDefault(s => s.ShiftID == shiftId);

            if (selectedShift == null)
            {
                return NotFound();

            }
            // Prepare a view model to display shift details in the request form

            var shiftRequestViewModel = new ShiftRequestViewModel
            {
                ShiftId = selectedShift.ShiftID,
                ShiftName = selectedShift.ShiftName,
                ShiftTime = selectedShift.ShiftTime,
                TrainerName = selectedShift.Trainer!=null? selectedShift.Trainer.FirstName + "" + selectedShift.Trainer.LastName :"No Trainer Assigned"



                // Add more properties if needed
            };

            return View(shiftRequestViewModel);
        


            // Handle the case when the shift doesn't exist
        }

        // POST: RequestShift
        [HttpPost]
        public IActionResult RequestShift(ShiftRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Fetch the selected shift from the database
                var shiftRequest = new ShiftRequest
                {
                    ShiftID = model.ShiftId,

                    MemberName = model.MemberName,
                    RequestedDate = DateTime.Today,
                    MemberID = model.MemberId,
                    TrainerName= model.TrainerName
                    
                  
                    

                };

                    // Add the shift request to your data source
                    _dbContext.ShiftRequests.Add(shiftRequest);
                    _dbContext.SaveChanges();
                TempData["Success"] = "Shift successfully requested";

                    // Redirect to a confirmation page or another action
                    return RedirectToAction("MemberDashboard");
                }
            

            // If there are validation errors or the shift doesn't exist, return to the request form
            return View( "RequestShift",model);
        }


        // Shift Request Confirmation
        public IActionResult RequestConfirmation()
        {
            // Display a confirmation message after a successful shift request
            return View();
        }
    }
}
