using ContractMonthlyClaimSystem4.Data;
using ContractMonthlyClaimSystem4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace ContractMonthlyClaimSystem4.Controllers
{
    [Authorize(Roles = "Lecturer")]
    public class ClaimsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly long _fileSizeLimit = 10 * 1024 * 1024; // 10 MB
        private readonly string[] _permittedExtensions = { ".pdf", ".docx", ".xlsx" };

        public ClaimsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Submit()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(Claim claim, List<IFormFile> uploadedFiles)
        {
            // Get the current user
            var user = await _userManager.GetUserAsync(User);

            // Set the SubmitterId
            claim.SubmitterId = user.Id;

            // Calculate the Final Payment on the server side
            claim.FinalPayment = claim.HoursWorked * claim.HourlyRate;

            // Ensure the Status is set to Pending
            claim.Status = ClaimStatus.Pending;

            /*
            // Ensure the Status is set to Pending
            if (claim.Status == 0)
            {
                claim.Status = ClaimStatus.Pending;
            }
            */

            if (ModelState.IsValid)
            {
                _context.Claims.Add(claim);
                await _context.SaveChangesAsync();

                var uploadedFileNames = new List<string>();

                if (uploadedFiles != null && uploadedFiles.Count > 0)
                {
                    foreach (var formFile in uploadedFiles)
                    {
                        if (formFile.Length > _fileSizeLimit)
                        {
                            ModelState.AddModelError("File", $"The file {formFile.FileName} is too large.");
                            return View(claim);
                        }

                        var extension = Path.GetExtension(formFile.FileName).ToLowerInvariant();
                        if (string.IsNullOrEmpty(extension) || !_permittedExtensions.Contains(extension))
                        {
                            ModelState.AddModelError("File", $"The file type of {formFile.FileName} is invalid.");
                            return View(claim);
                        }

                        var trustedFileName = Path.GetRandomFileName() + extension;

                        var filePath = Path.Combine("wwwroot/uploads", trustedFileName);

                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }

                        var supportingDocument = new SupportingDocument
                        {
                            FileName = formFile.FileName,
                            FilePath = "/uploads/" + trustedFileName,
                            FileSize = formFile.Length,
                            ContentType = formFile.ContentType,
                            ClaimId = claim.Id
                        };

                        _context.SupportingDocuments.Add(supportingDocument);

                        uploadedFileNames.Add(formFile.FileName);
                    }

                    await _context.SaveChangesAsync();
                }

                // Set TempData for confirmation page
                TempData["UploadedFiles"] = uploadedFileNames;

                return RedirectToAction("Confirmation");
            }

            return View(claim);
        }

        public IActionResult Confirmation()
        {
            return View();
        }

        // GET: Displays the lecturer's claims
        public async Task<IActionResult> MyClaims()
        {
            // Get the current user
            var user = await _userManager.GetUserAsync(User);

            // Retrieve claims submitted by the current user
            var claims = await _context.Claims
                .Where(c => c.SubmitterId == user.Id)
                .OrderByDescending(c => c.SubmissionDate)
                .ToListAsync();

            return View(claims);
        }
    }
}
