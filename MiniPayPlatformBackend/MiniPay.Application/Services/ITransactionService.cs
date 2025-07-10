using MiniPay.Application.DTOs;
using MiniPay.Application.Shared;

namespace MiniPay.Application.Services {
    /**
     * @brief Interface for HTTP request services.
     */
    public interface ITransactionService {
        Task<Result<TransactionResultDto>> ExecuteTransactionAsync(TransactionRequestDto requestDto);
    }
}
