using FptJobBack.Models;
using Microsoft.EntityFrameworkCore;

namespace FptJobBack.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<JobPosting> JobPostings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Định cấu hình kiểu kế thừa
            modelBuilder.Entity<Users>()
                .HasDiscriminator<string>("Role")
                .HasValue<Users>("User")
                .HasValue<Admin>("Admin")
                .HasValue<Company>("Company");

            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.HasKey(e => e.ProfileId);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ContactNumber).HasMaxLength(20);
                entity.Property(e => e.Address).HasMaxLength(255);
                entity.Property(e => e.Education).HasMaxLength(255);
                entity.Property(e => e.Experience).HasMaxLength(255);
                entity.Property(e => e.Skills).HasMaxLength(255);
                entity.HasOne(e => e.User)
                      .WithOne(u => u.UserProfile)
                      .HasForeignKey<UserProfile>(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);  // Optional, depending on your cascading behavior
            });
            // Định nghĩa các quan hệ của JobPosting
            modelBuilder.Entity<JobPosting>(entity =>
{
    entity.HasKey(e => e.JobPostingId);
    entity.Property(e => e.Title).IsRequired();
    entity.Property(e => e.Description).IsRequired();
    entity.Property(e => e.PostedDate).IsRequired();

    // Configure one-to-one relationship with Category
    entity.HasOne(e => e.Category)
          .WithOne(c => c.JobPosting)
          .HasForeignKey<JobPosting>(e => e.CategoryId)
          .IsRequired();

    // Configure many-to-one relationship with Company
    entity.HasOne(e => e.User)
             .WithMany(u => u.JobPostings)
             .HasForeignKey(e => e.UserId)
             .IsRequired()
             .OnDelete(DeleteBehavior.Restrict);

    // Additional configurations like indexes, etc.
    // entity.HasIndex(e => e.Title);
});


        }
    }
}
