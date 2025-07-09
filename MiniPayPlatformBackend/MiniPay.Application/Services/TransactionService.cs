using MiniPay.Application.DTOs;

namespace MiniPay.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IHTTPRequestService _httpRequestService;
        private readonly IPaymentProviderService _paymentProviderService;

        public TransactionService(IHTTPRequestService httpRequestService, IPaymentProviderService paymentProviderService)
        {
            _httpRequestService = httpRequestService;
            _paymentProviderService = paymentProviderService;
        }

        public async Task<Result<TransactionResultDto>> ExecuteTransactionAsync(TransactionRequestDto requestDto)
        {
            // Get the payment provider by ID to ensure it exists
            var resultPaymentProvider = await _paymentProviderService.GetByIdAsync(requestDto.PaymentProviderId);

            // If the payment provider is not found or an error occurs, return a failure result
            if (!resultPaymentProvider.IsSuccess || resultPaymentProvider.Data == null)
            {
                return Result<TransactionResultDto>.Fail(resultPaymentProvider.ErrorMessage, resultPaymentProvider.ErrorCode);
            }

            // Send the HTTP request using the HTTP request service
            var response = await _httpRequestService.sendHTTPRequest(requestDto, resultPaymentProvider.Data);

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
