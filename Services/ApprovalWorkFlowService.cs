using ContractMonthlyClaimSystem4.Data;
using ContractMonthlyClaimSystem4.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ContractMonthlyClaimSystem4.Services
{
    public class ApprovalWorkflowService
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<Claim> _claimValidator;

        public ApprovalWorkflowService(ApplicationDbContext context, IValidator<Claim> claimValidator)
        {
            _context = context;
            _claimValidator = claimValidator;
        }

        public async Task<ValidationResult> ProcessClaimApproval(int claimId)
        {
            var claim = await _context.Claims.Include(c => c.SupportingDocuments).FirstOrDefaultAsync(c => c.Id == claimId);
            if (claim != null)
            {
                // Validate the claim
                ValidationResult result = _claimValidator.Validate(claim);

                if (result.IsValid)
                {
                    // Approve the claim
                    claim.Status = ClaimStatus.Approved;
                }
                else
                {
                    // Reject the claim
                    claim.Status = ClaimStatus.Rejected;
                }

                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }
    }
}
