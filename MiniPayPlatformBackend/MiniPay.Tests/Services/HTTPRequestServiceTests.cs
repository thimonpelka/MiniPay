using Moq;
using Moq.Protected;
using MiniPay.Application.DTOs;
using MiniPay.Application.Services;
using System.Net;
using System.Text;
using System.Text.Json;

namespace MiniPay.Tests.Services
{
    public class HTTPRequestServiceTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly IHTTPRequestService _httpRequestService;

        public HTTPRequestServiceTests()
        {
			_httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            var _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _httpRequestService = new HTTPRequestService(_httpClient);
        }

        [Fact]
        public async Task SendHTTPRequest_ShouldReturnSuccessResult_WhenRequestIsValid()
        {
            // Arrange
            var requestDto = new TransactionRequestDto
            {
                PaymentProviderId = 1,
                ReferenceId = "12345",
                Amount = 100,
                Description = "Test transaction"
            };

            var paymentProvider = new PaymentProviderDto
            {
                Id = 1,
                Name = "Test Provider",
                Url = "https://api.testprovider.com",
                Currency = "USD",
                IsActive = true
            };

            var expectedResult = new TransactionResultDto
            {
                Status = "Success",
                TransactionId = "12345",
                Timestamp = DateTime.UtcNow,
                Message = "Transaction completed successfully",
                ReferenceId = "12345"
            };

            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(expectedResult), Encoding.UTF8, "application/json")
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            // Act
            var result = await _httpRequestService.sendHTTPRequest(requestDto, paymentProvider);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedResult.Status, result.Data.Status);
        }
    }
}
