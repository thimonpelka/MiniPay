using Microsoft.EntityFrameworkCore;
using MiniPay.Application.Data;
using MiniPay.Application.Models;

namespace MiniPay.Application.Repositories
{
	public class PaymentProviderRepository : IPaymentProviderRepository
	{
		private readonly ApplicationDbContext _context;

		public PaymentProviderRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		/*
		 * Retrieves all payment providers from the database, ordered by creation date.
		 */
		public async Task<IEnumerable<PaymentProvider>> GetAllAsync()
		{
			return await _context.PaymentProviders
				.OrderByDescending(p => p.CreatedAt)
				.ToListAsync();
		}

		public async Task<PaymentProvider?> GetByIdAsync(int id)
		{
			return await _context.PaymentProviders.FirstOrDefaultAsync(p => p.Id == id);
		}

		public async Task<PaymentProvider> CreateAsync(PaymentProvider paymentProvider)
		{
			paymentProvider.CreatedAt = DateTime.UtcNow;

			_context.PaymentProviders.Add(paymentProvider);
			await _context.SaveChangesAsync();

			return paymentProvider;
		}

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
