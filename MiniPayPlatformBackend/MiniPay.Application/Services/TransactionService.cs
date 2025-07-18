using MiniPay.Application.DTOs;
using MiniPay.Application.Shared;

namespace MiniPay.Application.Services
{
    /**
     * @brief Service for executing transactions with payment providers.
     */
    public class TransactionService : ITransactionService
    {
        private readonly IHTTPRequestService _httpRequestService;
        private readonly IPaymentProviderService _paymentProviderService;

        public TransactionService(IHTTPRequestService httpRequestService, IPaymentProviderService paymentProviderService)
        {
            _httpRequestService = httpRequestService;
            _paymentProviderService = paymentProviderService;
        }

        /**
         * @brief Executes a transaction with a payment provider.
         *
         * @param requestDto The transaction request data transfer object containing the necessary information for the transaction.
         * @return A result containing the transaction result data transfer object or an error message if the transaction fails.
         */
        public async Task<Result<TransactionResultDto>> ExecuteTransactionAsync(TransactionRequestDto requestDto)
        {
            // Get the payment provider by ID to ensure it exists
            var paymentProvider = await _paymentProviderService.GetByIdAsync(requestDto.PaymentProviderId);

            // Send the HTTP request using the HTTP request service
            var response = await _httpRequestService.sendHTTPRequest(requestDto, paymentProvider);

            // Check if the HTTP request was successful
            if (!response.IsSuccess || response.Data == null)
            {
                // If the HTTP request failed, return a failure result with the error message and code
                return Result<TransactionResultDto>.Fail(response.ErrorMessage, response.ErrorCode);
            }

            return Result<TransactionResultDto>.Success(response.Data);
        }
    }

}
