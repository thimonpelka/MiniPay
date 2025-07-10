using MiniPay.Application.DTOs;

namespace MiniPay.Application.Services {
	public interface IPaymentProviderService
	{
		Task<Result<IEnumerable<PaymentProviderDto>>> GetAllAsync(PaymentProviderQueryDto queryDto);
		Task<Result<PaymentProviderDto>> GetByIdAsync(int id);
		Task<Result<PaymentProviderDto>> CreateAsync(CreatePaymentProviderDto createDto);
		Task<Result<PaymentProviderDto>> UpdateAsync(int id, UpdatePaymentProviderDto updateDto);
		Task<Result<bool>> DeleteAsync(int id);
	}
}
