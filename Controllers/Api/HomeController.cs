using ContractMonthlyClaimSystem4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using ContractMonthlyClaimSystem4.Data;
using System.Diagnostics;

namespace ContractMonthlyClaimSystem4.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context; // Assuming you have ApplicationDbContext for data access

        // Inject the logger and ApplicationDbContext (for database operations)
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Index action to show the claim submission form
        public IActionResult Index()
        {
            var claim = new Claim();
            return View(claim); // Return the view with a new Claim object as the model
        }

        // Submit action to handle the form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Submit(Claim claim, IFormFile[] uploadedFiles)
        {
            // Validate the model
            if (ModelState.IsValid)
            {
                // Save the claim data to the database (add more logic as needed)
                _context.Claims.Add(claim);
                _context.SaveChanges();

                // Handle file uploads
                if (uploadedFiles != null && uploadedFiles.Length > 0)
                {
                    foreach (var file in uploadedFiles)
                    {
                        // Define the file path where to store the file (adjust as needed)
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", file.FileName);

                        // Save the file to the server
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        // If you want to store file details in the database, you can create a document entity
                        // and add it to the database
                        var document = new SupportingDocument
                        {
                            FileName = file.FileName,
                            FilePath = filePath,
                            ClaimId = claim.Id
                        };

                        _context.SupportingDocuments.Add(document);
                    }

                    _context.SaveChanges();
                }

                // Redirect the user to another page (or back to Index with a success message)
                return RedirectToAction("Index"); // You could redirect to another view or show a success message
            }

            // If validation failed, return the same view with validation errors
            return View("Index", claim);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
