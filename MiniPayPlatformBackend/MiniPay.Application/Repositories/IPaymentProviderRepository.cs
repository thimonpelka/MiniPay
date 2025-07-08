using MiniPay.Application.Models;

namespace MiniPay.Application.Repositories
{
	public interface IPaymentProviderRepository
	{
		Task<IEnumerable<PaymentProvider>> GetAllAsync();
		Task<PaymentProvider?> GetByIdAsync(int id);
		Task<PaymentProvider> CreateAsync(PaymentProvider paymentProvider);
		Task<PaymentProvider?> UpdateAsync(int id, PaymentProvider paymentProvider);
		Task<bool> DeleteAsync(int id);
		Task<bool> ExistsAsync(int id);
	}
}
