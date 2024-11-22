using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ContractMonthlyClaimSystem4.Controllers
{
    // Handles user authentication
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        // Constructor with dependency injection
        public AccountController(UserManager<IdentityUser> userManager,
                                 SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Display login form
        public IActionResult Login()
        {
            return View();
        }

        // POST: Process login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, false, false);

            if (result.Succeeded)
            {
                // Get the user
                var user = await _userManager.FindByEmailAsync(email);
                // Get user roles
                var roles = await _userManager.GetRolesAsync(user);

                // Redirect based on role
                if (roles.Contains("Lecturer"))
                {
                    return RedirectToAction("Submit", "Claims");
                }
                else if (roles.Contains("Coordinator") || roles.Contains("Manager"))
                {
                    return RedirectToAction("Index", "ClaimsManagement");
                }
                else
                {
                    // Default redirection
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View();
        }

        // POST: Logout
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Get the current user before signing out.
            var user = await _userManager.GetUserAsync(User);

            // Check if the user is authenticated.
            if (user != null)
            {
                // Get the roles of the current user.
                var roles = await _userManager.GetRolesAsync(user);

                // Sign out the user.
                await _signInManager.SignOutAsync();

                // Check if the user was a Lecturer or Coordinator.
                if (roles.Contains("Lecturer") || roles.Contains("Coordinator"))
                {
                    // Redirect to the Login page.
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    // Redirect to the Home page or another page for other roles.
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                // User is not authenticated, redirect to Login page.
                return RedirectToAction("Login", "Account");
            }
        }
    }
}