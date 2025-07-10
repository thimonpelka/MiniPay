using MiniPay.Application.Models;
using MiniPay.Application.DTOs;

namespace MiniPay.Application.Repositories
{
	/**
	 * @brief IPaymentProviderRepository interface defines the contract for payment provider repository operations.
	 */
	public interface IPaymentProviderRepository
	{
		Task<IEnumerable<PaymentProvider>> GetAllAsync(PaymentProviderQueryDto queryDto);
		Task<PaymentProvider?> GetByIdAsync(int id);
		Task<PaymentProvider> CreateAsync(PaymentProvider paymentProvider);
		Task<PaymentProvider?> UpdateAsync(int id, PaymentProvider paymentProvider);
		Task<bool> DeleteAsync(int id);
		Task<bool> ExistsAsync(int id);
	}
}
