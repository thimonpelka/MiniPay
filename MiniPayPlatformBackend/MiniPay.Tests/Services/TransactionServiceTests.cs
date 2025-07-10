using Moq;
using MiniPay.Application.DTOs;
using MiniPay.Application.Services;
using MiniPay.Application.Shared;

namespace MiniPay.Tests.Services
{
    public class TransactionServiceTests
    {
        private readonly Mock<IHTTPRequestService> _httpRequestServiceMock;
        private readonly Mock<IPaymentProviderService> _paymentProviderServiceMock;
        private readonly ITransactionService _transactionService;

        private List<PaymentProviderDto> _mockPaymentProviders;

        public TransactionServiceTests()
        {
            _httpRequestServiceMock = new Mock<IHTTPRequestService>();
            _paymentProviderServiceMock = new Mock<IPaymentProviderService>();
            _transactionService = new TransactionService(_httpRequestServiceMock.Object, _paymentProviderServiceMock.Object);

            // Generate mock data for payment providers
            _mockPaymentProviders = new List<PaymentProviderDto>{
                new PaymentProviderDto {
                    Id = 1,
                    Name = "Provider 1",
                    Url = "https://provider1.com",
                    Currency = "USD",
                    IsActive = true
                },
                new PaymentProviderDto {
                    Id = 2,
                    Name = "Provider 2",
                    Url = "https://provider2.com",
                    Currency = "EUR",
                    IsActive = true
                },
                new PaymentProviderDto {
                    Id = 3,
                    Name = "Provider 3",
                    Url = "https://provider1.com",
                    Currency = "GBP",
                    IsActive = false
                },
            };
        }

        [Fact]
        public async Task ExecuteTransactionAsync_ShouldReturnSuccess_WhenValidData()
        {
            TransactionRequestDto requestDto = new TransactionRequestDto
            {
                PaymentProviderId = 1,
                Amount = 100.00m,
                ReferenceId = "123456",
                Description = "Test Transaction"
            };

            _paymentProviderServiceMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(
                Result<PaymentProviderDto>.Success(_mockPaymentProviders[0])
            );

            _httpRequestServiceMock.Setup(service => service.sendHTTPRequest(requestDto, _mockPaymentProviders[0])).ReturnsAsync(
                Result<TransactionResultDto>.Success(new TransactionResultDto
                {
                    Status = "Success",
                    TransactionId = "TX123456",
                    Timestamp = DateTime.UtcNow,
                    Message = "Transaction completed successfully",
                    ReferenceId = requestDto.ReferenceId
                }
            ));

			// Act
            var result = await _transactionService.ExecuteTransactionAsync(requestDto);

			// Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal("Success", result.Data.Status);
            Assert.Equal("TX123456", result.Data.TransactionId);
            Assert.Equal(requestDto.ReferenceId, result.Data.ReferenceId);
            Assert.Equal("Transaction completed successfully", result.Data.Message);
        }

        [Fact]
        public async Task ExecuteTransactionAsync_ShouldReturnFailure_WhenPaymentProviderNotFound()
        {
            TransactionRequestDto requestDto = new TransactionRequestDto
            {
                PaymentProviderId = 999, // Non-existent provider
                Amount = 100.00m,
                ReferenceId = "123456",
                Description = "Test Transaction"
            };

            _paymentProviderServiceMock.Setup(repo => repo.GetByIdAsync(999)).ReturnsAsync(
                Result<PaymentProviderDto>.Fail("Payment provider with ID 999 not found.", 404)
                );

			// Act
            var result = await _transactionService.ExecuteTransactionAsync(requestDto);

			// Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Equal("Payment provider with ID 999 not found.", result.ErrorMessage);
            Assert.Equal(404, result.ErrorCode);
        }

        [Fact]
        public async Task ExecuteTransactionAsync_ShouldReturnFailure_WhenHTTPRequestFails()
        {
            TransactionRequestDto requestDto = new TransactionRequestDto
            {
                PaymentProviderId = 1,
                Amount = 100.00m,
                ReferenceId = "123456",
                Description = "Test Transaction"
            };
            _paymentProviderServiceMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(
                Result<PaymentProviderDto>.Success(_mockPaymentProviders[0])
                );

            _httpRequestServiceMock.Setup(service => service.sendHTTPRequest(requestDto, _mockPaymentProviders[0])).ReturnsAsync(
                Result<TransactionResultDto>.Fail("HTTP request failed.", 500)
                );

            // Act
            var result = await _transactionService.ExecuteTransactionAsync(requestDto);

			// Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Equal("HTTP request failed.", result.ErrorMessage);
            Assert.Equal(500, result.ErrorCode);
        }
    }
}
