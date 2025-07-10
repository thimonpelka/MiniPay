using MiniPay.Application.DTOs;
using MiniPay.Application.Shared;
using MiniPay.Application.Exceptions;
using System.Text;
using System.Text.Json;

namespace MiniPay.Application.Services
{
	/**
	 * @brief HTTPRequestService class is responsible for sending HTTP requests to payment providers.
	 */
    public class HTTPRequestService : IHTTPRequestService
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public HTTPRequestService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

		/**
		 * @brief Sends an HTTP request to the payment provider's API to execute a transaction.
		 *
		 * @param requestDto The transaction request data containing details like amount, currency, description, and reference ID.
		 * @param paymentProvider The payment provider to send a request to
		 * @return Result object which, in a sucess case, contains the result of the transaction
		 */
        public async Task<Result<TransactionResultDto>> sendHTTPRequest(TransactionRequestDto requestDto, PaymentProviderDto paymentProvider)
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                // Create the request payload matching your Python API format
                var requestPayload = new
                {
                    amount = requestDto.Amount,
                    currency = paymentProvider.Currency.ToString().ToUpper(),
                    description = requestDto.Description,
                    referenceId = requestDto.ReferenceId
                };


                // Serialize to JSON
                var jsonContent = JsonSerializer.Serialize(requestPayload);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Make the POST request to the payment provider URL
                using HttpResponseMessage response = await _httpClient.PostAsync(paymentProvider.Url, content);

                // Check if the request was successful
                response.EnsureSuccessStatusCode();

                // Read the response content
                string responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize the response to match your Python API format
                var apiResponse = JsonSerializer.Deserialize<PaymentApiResponse>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Map the API response to your TransactionResultDto
                var transactionResult = MapResponseToDto.Map(apiResponse);

                return Result<TransactionResultDto>.Success(transactionResult);
            }
            catch (HttpRequestException e)
            {
                return Result<TransactionResultDto>.Fail($"Request error: {e.Message}", 500);
            }
            catch (JsonException e)
            {
                return Result<TransactionResultDto>.Fail($"JSON parsing error: {e.Message}", 500);
            }
			catch (APIResponseParsingException e)
			{
				return Result<TransactionResultDto>.Fail($"API response parsing error: {e.Message}", 500);
			}
            catch (Exception e)
            {
                return Result<TransactionResultDto>.Fail($"Unexpected error: {e.Message}", 500);
            }
        }

		/*
		 * @brief This class is responsible for mapping the API response to the TransactionResultDto.
		 * It handles null checks and throws an exception if any required field is missing.
		 */
        private static class MapResponseToDto
        {
			/**
			 * @brief Maps the response of the API to a TransactionResultDto object
			 *
			 * @param apiResponse Response of the api
			 * @return valid TransactionResultDto object or throws an APIResponseParsingException
			 */
            public static TransactionResultDto Map(PaymentApiResponse? apiResponse)
            {
                if (apiResponse == null)
                {
                    throw new APIResponseParsingException("API response is null");
                }

                return new TransactionResultDto
                {
                    Status = apiResponse.Status ?? throw new APIResponseParsingException("Status is null in API response"),
                    TransactionId = apiResponse.TransactionId ?? throw new APIResponseParsingException("TransactionId is null in API response"),
                    Timestamp = DateTime.Parse(apiResponse.Timestamp ?? throw new APIResponseParsingException("Timestamp is null in API response")),
                    Message = apiResponse.Message ?? throw new APIResponseParsingException("Message is null in API response"),
                    ReferenceId = apiResponse.ReferenceId ?? throw new APIResponseParsingException("ReferenceId is null in API response")
                };
            }
        }
    }

    // Helper class to deserialize the API response
    public class PaymentApiResponse
    {
        public string Status { get; set; } = string.Empty;
        public string? TransactionId { get; set; }
        public string? Timestamp { get; set; }
        public string? Message { get; set; }
        public string? ReferenceId { get; set; }
    }
}
