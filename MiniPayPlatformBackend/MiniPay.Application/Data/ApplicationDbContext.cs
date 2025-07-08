using Microsoft.EntityFrameworkCore;
using MiniPay.Application.Models;

namespace MiniPay.Application.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
			Database.EnsureCreated(); // Ensure the database is created on startup
        }

        public required DbSet<PaymentProvider> PaymentProviders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the Payment Provider entity
            modelBuilder.Entity<PaymentProvider>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<PaymentProvider>()
                .Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<PaymentProvider>()
                .Property(a => a.Url)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<PaymentProvider>()
                .Property(a => a.IsActive)
				.IsRequired();

			modelBuilder.Entity<PaymentProvider>()
				.Property(a => a.Currency)
				.IsRequired()
				.HasConversion<string>(); // Convert enum to string for network compatibility

			modelBuilder.Entity<PaymentProvider>()
				.Property(a => a.Description)
				.HasMaxLength(500);

			modelBuilder.Entity<PaymentProvider>()
				.Property(a => a.CreatedAt)
				.HasDefaultValueSql("CURRENT_TIMESTAMP");

			modelBuilder.Entity<PaymentProvider>()
				.Property(a => a.UpdatedAt)
				.HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
