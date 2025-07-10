using Microsoft.EntityFrameworkCore;
using MiniPay.Application.Models;

namespace MiniPay.Application.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            try {
                Database.EnsureCreated(); // Ensure the database is created on startup
            } catch (Exception ex) {
                Console.WriteLine($"An error occurred while ensuring the database is created: {ex.Message}");
            }
        }

        public DbSet<PaymentProvider> PaymentProviders { get; set; }

        /**
         * @brief Configures the model for the application database context.
         *
         * @param modelBuilder The model builder used to configure the entity types.
         * @return void
         */
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
