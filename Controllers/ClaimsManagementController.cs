using ContractMonthlyClaimSystem4.Data;
using ContractMonthlyClaimSystem4.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using ContractMonthlyClaimSystem4.Validators;

namespace ContractMonthlyClaimSystem4.Controllers
{
    // Only users in Coordinator or Manager roles can access this controller
    [Authorize(Roles = "Coordinator,Manager")]
    public class ClaimsManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<Claim> _claimValidator;

        public ClaimsManagementController(ApplicationDbContext context, IValidator<Claim> claimValidator)
        {
            _context = context;
            _claimValidator = claimValidator;
        }

        // GET: Displays all pending claims
        public async Task<IActionResult> Index()
        {
            var pendingClaims = await _context.Claims
                .Include(c => c.SupportingDocuments)
                .Where(c => c.Status == ClaimStatus.Pending)
                .ToListAsync();

            return View(pendingClaims);
        }

        // POST: Approve a claim
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var claim = await _context.Claims.Include(c => c.SupportingDocuments).FirstOrDefaultAsync(c => c.Id == id);
            if (claim != null)
            {
                // Validate the claim against predefined criteria
                ValidationResult result = _claimValidator.Validate(claim);

                if (result.IsValid)
                {
                    claim.Status = ClaimStatus.Approved;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // If validation fails, display errors
                    TempData["ValidationErrors"] = result.Errors.Select(e => e.ErrorMessage).ToList();
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Reject a claim
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim != null)
            {
                claim.Status = ClaimStatus.Rejected;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
