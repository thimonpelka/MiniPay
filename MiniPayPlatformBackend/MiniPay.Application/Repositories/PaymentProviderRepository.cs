using Microsoft.EntityFrameworkCore;
using MiniPay.Application.DTOs;
using MiniPay.Application.Data;
using MiniPay.Application.Models;

namespace MiniPay.Application.Repositories
{
    /**
     * @brief PaymentProviderRepository class implements the IPaymentProviderRepository interface
     */
    public class PaymentProviderRepository : IPaymentProviderRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentProviderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /**
         * @brief Retrieves all payment providers based on the provided query parameters.
         *
         * @param queryDto The query parameters to filter the payment providers.
         * @return A collection of payment providers that match the query parameters.
         */
        public async Task<IEnumerable<PaymentProvider>> GetAllAsync(PaymentProviderQueryDto queryDto)
        {
            var query = _context.PaymentProviders.AsQueryable();

            if (queryDto.IsActive.HasValue)
            {
                query = query.Where(p => p.IsActive == queryDto.IsActive.Value);
            }

            return await query
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        /**
         * @brief Retrieves a payment provider by its ID.
         *
         * @param id The ID of the payment provider to retrieve.
         * @return A payment provider object if found, otherwise null.
         */
        public async Task<PaymentProvider?> GetByIdAsync(int id)
        {
            return await _context.PaymentProviders.FirstOrDefaultAsync(p => p.Id == id);
        }

        /**
         * @brief Creates a new payment provider in the database.
         *
         * @param paymentProvider The payment provider object to create.
         * @return The created payment provider object with its ID and creation timestamp.
         */
        public async Task<PaymentProvider> CreateAsync(PaymentProvider paymentProvider)
        {
            paymentProvider.CreatedAt = DateTime.UtcNow;

            _context.PaymentProviders.Add(paymentProvider);
            await _context.SaveChangesAsync();

            return paymentProvider;
        }

        /**
         * @brief Updates an existing payment provider in the database.
         *
         * @param id The ID of the payment provider to update.
         * @param paymentProvider The payment provider object containing updated information.
         * @return The updated payment provider object if found and updated, otherwise null.
         */
        public async Task<PaymentProvider?> UpdateAsync(int id, PaymentProvider paymentProvider)
        {
            var existingProvider = await _context.PaymentProviders.FirstOrDefaultAsync(p => p.Id == id);

            if (existingProvider == null) return null;

            existingProvider.Name = paymentProvider.Name;
            existingProvider.Url = paymentProvider.Url;
            existingProvider.IsActive = paymentProvider.IsActive;
            existingProvider.Currency = paymentProvider.Currency;
            existingProvider.Description = paymentProvider.Description;
            existingProvider.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return existingProvider;
        }

        /**
         * @brief Deletes a payment provider by its ID.
         *
         * @param id The ID of the payment provider to delete.
         * @return True if the provider was deleted, otherwise false.
         */
        public async Task<bool> DeleteAsync(int id)
        {
            var provider = await _context.PaymentProviders.FirstOrDefaultAsync(p => p.Id == id);

            if (provider == null) return false;

            _context.PaymentProviders.Remove(provider);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.PaymentProviders.AnyAsync(p => p.Id == id);
        }
    }
}
