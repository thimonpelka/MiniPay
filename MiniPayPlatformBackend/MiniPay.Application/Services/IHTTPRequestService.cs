using MiniPay.Application.DTOs;
using MiniPay.Application.Shared;

namespace MiniPay.Application.Services {
	/**
	 * @brief Interface for HTTP request services.
	 */
	public interface IHTTPRequestService {
		Task<Result<TransactionResultDto>> sendHTTPRequest(TransactionRequestDto requestDto, PaymentProviderDto paymentProvider);
	}
}
