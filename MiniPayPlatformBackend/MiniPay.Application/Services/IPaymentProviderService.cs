using MiniPay.Application.DTOs;
using MiniPay.Application.Shared;

namespace MiniPay.Application.Services {
	/**
	 * @brief Interface for HTTP request services.
	 */
	public interface IPaymentProviderService
	{
		Task<Result<IEnumerable<PaymentProviderDto>>> GetAllAsync(PaymentProviderQueryDto queryDto);
		Task<Result<PaymentProviderDto>> GetByIdAsync(int id);
		Task<Result<PaymentProviderDto>> CreateAsync(CreatePaymentProviderDto createDto);
		Task<Result<PaymentProviderDto>> UpdateAsync(int id, UpdatePaymentProviderDto updateDto);
		Task<Result<bool>> DeleteAsync(int id);
	}
}
