using MiniPay.Application.DTOs;

namespace MiniPay.Application.Services {
	public interface ITransactionService {
		Task<Result<TransactionResultDto>> ExecuteTransactionAsync(TransactionRequestDto requestDto);
	}
}
