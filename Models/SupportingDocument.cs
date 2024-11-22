using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContractMonthlyClaimSystem4.Models
{
    // Represents a supporting document uploaded with a claim.
    public class SupportingDocument
    {
        [Key]
        public int Id { get; set; }

        // Original file name
        [Required]
        public string FileName { get; set; }

        // Stored file path
        [Required]
        public string FilePath { get; set; }

        // Size of the file in bytes
        public long FileSize { get; set; }

        // MIME type of the file
        public string ContentType { get; set; }

        // Foreign key to the Claim
        [ForeignKey("Claim")]
        public int ClaimId { get; set; }

        public Claim Claim { get; set; }
    }
}