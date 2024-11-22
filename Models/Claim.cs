using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContractMonthlyClaimSystem4.Models
{
    public class Claim
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Hours Worked is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a valid number of hours.")]
        [Display(Name = "Hours Worked")]
        public int HoursWorked { get; set; }

        [Required(ErrorMessage = "Hourly Rate is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a valid hourly rate.")]
        [Display(Name = "Hourly Rate")]
        [DataType(DataType.Currency)]
        public decimal HourlyRate { get; set; }

        [Display(Name = "Additional Notes")]
        public string Notes { get; set; }

        public DateTime SubmissionDate { get; set; } = DateTime.Now;

        [Required]
        public ClaimStatus Status { get; set; } = ClaimStatus.Pending;

        public ICollection<SupportingDocument> SupportingDocuments { get; set; }

        // New property for final payment
        [Display(Name = "Final Payment")]
        [DataType(DataType.Currency)]
        public decimal FinalPayment { get; set; }

        // ID of the user who submitted the claim
        [Required]
        public string SubmitterId { get; set; }

        // Navigation property to the submitter
        [ForeignKey("SubmitterId")]
        public IdentityUser Submitter { get; set; }
    }

    public enum ClaimStatus
    {
        Pending,
        Approved,
        Rejected
    }
}
