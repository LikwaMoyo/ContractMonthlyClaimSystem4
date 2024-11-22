using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ContractMonthlyClaimSystem4.Models;
using Microsoft.AspNetCore.Identity;

namespace ContractMonthlyClaimSystem4.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Claim> Claims { get; set; }
        public DbSet<SupportingDocument> SupportingDocuments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure one-to-many relationship between Claim and SupportingDocument
            modelBuilder.Entity<SupportingDocument>()
                .HasOne(sd => sd.Claim)
                .WithMany(c => c.SupportingDocuments)
                .HasForeignKey(sd => sd.ClaimId);
        }
    }
}
