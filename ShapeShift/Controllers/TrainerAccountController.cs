using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShapeShift.Models;

namespace ShapeShift.Controllers
{
    public class TrainerAccountController : Controller
    {
        private readonly SignInManager<Trainer> _signInManager;
        private readonly UserManager<Trainer> _userManager;
        public TrainerAccountController(SignInManager<Trainer> signInManager, UserManager<Trainer> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;

        }



        [HttpGet]
        public IActionResult TrainerLogin()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> TrainerLogin(TrainerLoginViewModel model)


        {
            if (ModelState.IsValid)
            {
                // Check if a trainer with the provided email exists
                var trainer = await _userManager.FindByEmailAsync(model.Email);
                if (trainer != null)
                {
                    // Attempt to sign in the trainer
                    var result = await _signInManager.PasswordSignInAsync(trainer, model.Password, model.RememberMe, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        // Trainer successfully logged in
                        return RedirectToAction("TrainerDashboard");
                    }
                }

                // If no trainer with the provided email was found or login failed, show an error
                ModelState.AddModelError(string.Empty, "Invalid login attempt. Please check your email and password.");
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult ResetPassword()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the trainer by email or some other unique identifier
                var trainer = await _userManager.FindByEmailAsync(model.Email);

                if (trainer != null)
                {
                    // Reset the trainer's password
                    var resetToken = await _userManager.GeneratePasswordResetTokenAsync(trainer);
                    var result = await _userManager.ResetPasswordAsync(trainer, resetToken, model.NewPassword);

                    if (result.Succeeded)
                    {
                        // Password reset successful, you can redirect to the login page or trainer dashboard
                        return RedirectToAction("TrainerLogin");
                    }
                    else
                    {
                        // Handle password reset failure
                        ModelState.AddModelError(string.Empty, "Password reset failed.");
                    }
                }
                else
                {
                    // Trainer not found by email
                    ModelState.AddModelError(string.Empty, "Trainer not found.");
                }
            }

            // If the model is not valid or an error occurred, return to the reset password view
            return View(model);
        }

    }
}

