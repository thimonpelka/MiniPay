using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniPay.Application.DTOs;
using MiniPay.Application.Services;
using MiniPay.Application.Controllers;

namespace MiniPay.Tests.Services
{
    public class PaymentProviderControllerTests
    {
        private readonly Mock<IPaymentProviderService> _paymentProviderServiceMock;
        private readonly PaymentProviderController _controller;

        private List<PaymentProviderDto> _mockPaymentProviders;

        public PaymentProviderControllerTests()
        {
            _paymentProviderServiceMock = new Mock<IPaymentProviderService>();
            var loggerMock = new Mock<ILogger<PaymentProviderController>>();
            _controller = new PaymentProviderController(_paymentProviderServiceMock.Object, loggerMock.Object);

            _mockPaymentProviders = new List<PaymentProviderDto>
            {
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
					Url = "https://provider3.com",
					Currency = "GBP",
					IsActive = false
				}
            };
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkResult_WhenProvidersExist()
        {
            // Arrange
            _paymentProviderServiceMock
                .Setup(s => s.GetAllAsync())
                .ReturnsAsync(Result<IEnumerable<PaymentProviderDto>>
                .Success(_mockPaymentProviders));

            // Act
            var result = await _controller.GetAllAsync();

            // Assert
            Assert.IsType<ActionResult<IEnumerable<PaymentProviderDto>>>(result);

            // Extract the actual result
            var actionResult = result.Result as OkObjectResult;
            Assert.NotNull(actionResult);
            Assert.Equal(200, actionResult.StatusCode);
            var returnedProviders = actionResult.Value as IEnumerable<PaymentProviderDto>;
            Assert.NotNull(returnedProviders);
            Assert.Equal(_mockPaymentProviders, returnedProviders);
        }

        [Fact]
        public async Task GetById_ShouldReturnOkResult_WhenProviderExists()
        {
            // Arrange
            _paymentProviderServiceMock
                .Setup(s => s.GetByIdAsync(1))
                .ReturnsAsync(Result<PaymentProviderDto>
                .Success(_mockPaymentProviders[0]));

            // Act
            var result = await _controller.GetByIdAsync(1);

            // Assert
            Assert.IsType<ActionResult<PaymentProviderDto>>(result);

            // Extract the actual result
            var actionResult = result.Result as OkObjectResult;
            Assert.NotNull(actionResult);
            Assert.Equal(200, actionResult.StatusCode);
            var returnedProvider = actionResult.Value as PaymentProviderDto;
            Assert.NotNull(returnedProvider);
            Assert.Equal(_mockPaymentProviders[0], returnedProvider);
        }

        [Fact]
        public async Task GetById_ShouldReturnStatusCode_WhenProviderNotFound()
        {
            // Arrange
            _paymentProviderServiceMock
                .Setup(s => s.GetByIdAsync(999))
                .ReturnsAsync(Result<PaymentProviderDto>
                .Fail("Provider not found", 404));

            // Act
            var result = await _controller.GetByIdAsync(999);

            // Assert
            Assert.IsType<ActionResult<PaymentProviderDto>>(result);
            var actionResult = result.Result as ObjectResult;
            Assert.NotNull(actionResult);
            Assert.Equal(404, actionResult.StatusCode);
            Assert.Equal("Provider not found", actionResult.Value);
        }

        [Fact]
        public async Task GetAll_ShouldReturnStatusCode_WhenServiceFails()
        {
            // Arrange
            _paymentProviderServiceMock
                .Setup(s => s.GetAllAsync())
                .ReturnsAsync(Result<IEnumerable<PaymentProviderDto>>
                .Fail("Internal server error", 500));

            // Act
            var result = await _controller.GetAllAsync();

            // Assert
            Assert.IsType<ActionResult<IEnumerable<PaymentProviderDto>>>(result);
            var actionResult = result.Result as ObjectResult;
            Assert.NotNull(actionResult);
            Assert.Equal(500, actionResult.StatusCode);
            Assert.Equal("Internal server error", actionResult.Value);
        }

		[Fact]
        public async Task CreateAsync_ShouldReturnCreated_WhenValidData()
        {
            // Arrange
            var createDto = new CreatePaymentProviderDto
            {
                Name = "New Provider",
                Url = "https://newprovider.com",
                Currency = "USD",
                IsActive = true
            };

            var createdProvider = new PaymentProviderDto
            {
                Id = 4,
                Name = "New Provider",
                Url = "https://newprovider.com",
                Currency = "USD",
                IsActive = true
            };

            _paymentProviderServiceMock
                .Setup(s => s.CreateAsync(createDto))
                .ReturnsAsync(Result<PaymentProviderDto>.Success(createdProvider));

            // Act
            var result = await _controller.CreateAsync(createDto);

            // Assert
            Assert.IsType<ActionResult<PaymentProviderDto>>(result);
            
            var actionResult = result.Result as ObjectResult;
            Assert.NotNull(actionResult);
            Assert.Equal(201, actionResult.StatusCode);
            
            var returnedProvider = actionResult.Value as PaymentProviderDto;
            Assert.NotNull(returnedProvider);
            Assert.Equal(createdProvider, returnedProvider);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnStatusCode_WhenServiceFails()
        {
            // Arrange
            var createDto = new CreatePaymentProviderDto
            {
                Name = "New Provider",
                Url = "https://newprovider.com",
                Currency = "USD",
                IsActive = true
            };

            _paymentProviderServiceMock
                .Setup(s => s.CreateAsync(createDto))
                .ReturnsAsync(Result<PaymentProviderDto>.Fail("Provider already exists", 400));

            // Act
            var result = await _controller.CreateAsync(createDto);

            // Assert
            Assert.IsType<ActionResult<PaymentProviderDto>>(result);
            
            var actionResult = result.Result as ObjectResult;
            Assert.NotNull(actionResult);
            Assert.Equal(400, actionResult.StatusCode);
            Assert.Equal("Provider already exists", actionResult.Value);
        }

        // UpdateAsync tests
        [Fact]
        public async Task UpdateAsync_ShouldReturnOk_WhenValidData()
        {
            // Arrange
            var updateDto = new UpdatePaymentProviderDto
            {
                Name = "Updated Provider",
                Url = "https://updatedprovider.com",
                Currency = "EUR",
                IsActive = false
            };

            var updatedProvider = new PaymentProviderDto
            {
                Id = 1,
                Name = "Updated Provider",
                Url = "https://updatedprovider.com",
                Currency = "EUR",
                IsActive = false
            };

            _paymentProviderServiceMock
                .Setup(s => s.UpdateAsync(1, updateDto))
                .ReturnsAsync(Result<PaymentProviderDto>.Success(updatedProvider));

            // Act
            var result = await _controller.UpdateAsync(1, updateDto);

            // Assert
            Assert.IsType<ActionResult<PaymentProviderDto>>(result);
            
            var actionResult = result.Result as OkObjectResult;
            Assert.NotNull(actionResult);
            Assert.Equal(200, actionResult.StatusCode);
            
            var returnedProvider = actionResult.Value as PaymentProviderDto;
            Assert.NotNull(returnedProvider);
            Assert.Equal(updatedProvider, returnedProvider);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnStatusCode_WhenProviderNotFound()
        {
            // Arrange
            var updateDto = new UpdatePaymentProviderDto
            {
                Name = "Updated Provider",
                Url = "https://updatedprovider.com",
                Currency = "EUR",
                IsActive = false
            };

            _paymentProviderServiceMock
                .Setup(s => s.UpdateAsync(999, updateDto))
                .ReturnsAsync(Result<PaymentProviderDto>.Fail("Provider not found", 404));

            // Act
            var result = await _controller.UpdateAsync(999, updateDto);

            // Assert
            Assert.IsType<ActionResult<PaymentProviderDto>>(result);
            
            var actionResult = result.Result as ObjectResult;
            Assert.NotNull(actionResult);
            Assert.Equal(404, actionResult.StatusCode);
            Assert.Equal("Provider not found", actionResult.Value);
        }

        // DeleteAsync tests
        [Fact]
        public async Task DeleteAsync_ShouldReturnNoContent_WhenSuccessful()
        {
            // Arrange
            _paymentProviderServiceMock
                .Setup(s => s.DeleteAsync(1))
                .ReturnsAsync(Result<bool>.Success(true)); // or whatever your delete method returns

            // Act
            var result = await _controller.DeleteAsync(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            var noContentResult = result as NoContentResult;
            Assert.NotNull(noContentResult);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnStatusCode_WhenProviderNotFound()
        {
            // Arrange
            _paymentProviderServiceMock
                .Setup(s => s.DeleteAsync(999))
                .ReturnsAsync(Result<bool>.Fail("Provider not found", 404));

            // Act
            var result = await _controller.DeleteAsync(999);

            // Assert
            Assert.IsType<ObjectResult>(result);
            
            var actionResult = result as ObjectResult;
            Assert.NotNull(actionResult);
            Assert.Equal(404, actionResult.StatusCode);
            Assert.Equal("Provider not found", actionResult.Value);
        }
    }
}
