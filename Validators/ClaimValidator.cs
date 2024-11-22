using ContractMonthlyClaimSystem4.Models;
using FluentValidation;

namespace ContractMonthlyClaimSystem4.Validators
{
    public class ClaimValidator : AbstractValidator<Claim>
    {
        public ClaimValidator()
        {
            // Maximum Hours Worked (e.g., 40 hours per week)
            RuleFor(claim => claim.HoursWorked)
                .InclusiveBetween(1, 40)
                .WithMessage("Hours Worked must be between 1 and 40.");

            // Valid Hourly Rates (e.g., between $20 and $100)
            RuleFor(claim => claim.HourlyRate)
                .InclusiveBetween(20, 100)
                .WithMessage("Hourly Rate must be between $20 and $100.");

            // Final Payment Calculation Check
            RuleFor(claim => claim.FinalPayment)
                .Equal(claim => claim.HoursWorked * claim.HourlyRate)
                .WithMessage("Final Payment does not match Hours Worked multiplied by Hourly Rate.");

            // Require Supporting Documents if Hours Worked > 30
            When(claim => claim.HoursWorked > 30, () =>
            {
                RuleFor(claim => claim.SupportingDocuments)
                    .NotEmpty()
                    .WithMessage("Supporting documents are required for claims exceeding 30 hours.");
            });
        }
    }
}
