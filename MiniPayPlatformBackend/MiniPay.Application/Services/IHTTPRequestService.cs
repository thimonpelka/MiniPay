using MiniPay.Application.DTOs;

namespace MiniPay.Application.Services {
	public interface IHTTPRequestService {
		Task<Result<TransactionResultDto>> sendHTTPRequest(TransactionRequestDto requestDto, PaymentProviderDto paymentProvider);
	}
}
