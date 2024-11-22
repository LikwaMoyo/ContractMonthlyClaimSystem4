using ContractMonthlyClaimSystem4.Data;
using ContractMonthlyClaimSystem4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace ContractMonthlyClaimSystem4.Controllers
{
    [Authorize(Roles = "HR")]
    public class HRController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HRController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HR/ApprovedClaims
        public async Task<IActionResult> ApprovedClaims()
        {
            var approvedClaims = await _context.Claims
                .Include(c => c.SubmitterId)
                .Where(c => c.Status == ClaimStatus.Approved)
                .ToListAsync();

            return View(approvedClaims);
        }

        // Additional actions will be added later
    }
}
