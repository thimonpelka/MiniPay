using Moq;
using MiniPay.Application.Models;
using MiniPay.Application.DTOs;
using MiniPay.Application.Services;
using MiniPay.Application.Repositories;
using MiniPay.Application.Exceptions;

namespace MiniPay.Tests.Services
{
    public class PaymentProviderServiceTests
    {
        private readonly Mock<IPaymentProviderRepository> _paymentProviderRepositoryMock;
        private readonly IPaymentProviderService _paymentProviderService;

        private List<PaymentProvider> _mockPaymentProviders;

        // Query DTOs for testing
        private PaymentProviderQueryDto _emptyQueryDto = new PaymentProviderQueryDto { };
        private PaymentProviderQueryDto _isActiveQueryDto = new PaymentProviderQueryDto { IsActive = true };

        public PaymentProviderServiceTests()
        {
            _paymentProviderRepositoryMock = new Mock<IPaymentProviderRepository>();
            _paymentProviderService = new PaymentProviderService(_paymentProviderRepositoryMock.Object);

            // For every test a new Instance of the test class is generated, therefore we can 
            // just define all the mock data here without having to worry about previous tests 
            // affecting the current one.
            _mockPaymentProviders = new List<PaymentProvider>{
                new PaymentProvider {
                    Id = 1,
                    Name = "Provider 1",
                    Url = "https://provider1.com",
                    Currency = Currency.USD,
                    IsActive = true
                },
                new PaymentProvider {
                    Id = 2,
                    Name = "Provider 2",
                    Url = "https://provider2.com",
                    Currency = Currency.EUR,
                    IsActive = true
                },
                new PaymentProvider {
                    Id = 3,
                    Name = "Provider 3",
                    Url = "https://provider1.com",
                    Currency = Currency.GBP,
                    IsActive = false
                },
            };
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllPaymentProviders()
        {
            _paymentProviderRepositoryMock.Setup(repo => repo.GetAllAsync(_emptyQueryDto)).ReturnsAsync(_mockPaymentProviders);

            // Act
            var result = await _paymentProviderService.GetAllAsync(_emptyQueryDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(3, result.Data.Count());
            Assert.Equal("Provider 1", result.Data.First().Name);
            Assert.Equal("Provider 3", result.Data.Last().Name);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnActivePaymentProviders_WhenQueryIsActive()
        {
            var activeProviders = _mockPaymentProviders.Where(p => p.IsActive).ToList();
            _paymentProviderRepositoryMock.Setup(repo => repo.GetAllAsync(_isActiveQueryDto)).ReturnsAsync(activeProviders);

            // Act
            var result = await _paymentProviderService.GetAllAsync(_isActiveQueryDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count());
            Assert.All(result.Data, p => Assert.True(p.IsActive));
            Assert.Equal("Provider 1", result.Data.First().Name);
            Assert.Equal("Provider 2", result.Data.Last().Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnPaymentProvider_WhenExists()
        {
            _paymentProviderRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(_mockPaymentProviders[0]);

            // Act
            var result = await _paymentProviderService.GetByIdAsync(1);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal("Provider 1", result.Data.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnFailure_WhenNotExists()
        {
            _paymentProviderRepositoryMock.Setup(repo => repo.GetByIdAsync(999)).ReturnsAsync((PaymentProvider?)null);

            // Act
            try
            {
                var result = await _paymentProviderService.GetByIdAsync(999);

                // If no exception is thrown, fail the test
                Assert.Fail("Expected RetrievalException was not thrown.");
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsType<RetrievalException>(ex);
                Assert.Equal("Payment provider with ID 999 not found.", ex.Message);
            }
        }

        [Fact]
        public async Task CreateAsync_ShouldCreatePaymentProvider_WhenValidData()
        {
            var createDto = new CreatePaymentProviderDto
            {
                Name = "New Provider",
                Url = "https://newprovider.com",
                Currency = "USD",
                IsActive = true
            };

            _paymentProviderRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<PaymentProvider>()))
                .ReturnsAsync(new PaymentProvider
                {
                    Id = 4,
                    Name = createDto.Name,
                    Url = createDto.Url,
                    Currency = Currency.USD,
                    IsActive = createDto.IsActive
                });

            // Act
            var result = await _paymentProviderService.CreateAsync(createDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal("New Provider", result.Data.Name);
            Assert.Equal("https://newprovider.com", result.Data.Url);
            Assert.Equal("USD", result.Data.Currency);
            Assert.True(result.Data.IsActive);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnFailure_WhenInvalidCurrency()
        {
            var createDto = new CreatePaymentProviderDto
            {
                Name = "Invalid Currency Provider",
                Url = "https://invalidcurrency.com",
                Currency = "INVALID",
                IsActive = true
            };

            // Act
            try
            {

                var result = await _paymentProviderService.CreateAsync(createDto);

                // If no exception is thrown, fail the test
                Assert.Fail("Expected ValidationException was not thrown.");
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsType<ValidationException>(ex);
                Assert.Equal("Invalid currency: INVALID", ex.Message);
            }
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdatePaymentProvider_WhenValidData()
        {
            var updateDto = new UpdatePaymentProviderDto
            {
                Name = "Updated Provider",
                Url = "https://updatedprovider.com",
                Currency = "EUR",
                IsActive = false
            };

            _paymentProviderRepositoryMock.Setup(repo => repo.GetByIdAsync(100))
                .ReturnsAsync(new PaymentProvider
                {
                    Id = 100,
                    Name = "Old Provider",
                    Url = "https://oldprovider.com",
                    Currency = Currency.USD,
                    IsActive = true
                });

            _paymentProviderRepositoryMock.Setup(repo => repo.UpdateAsync(100, It.IsAny<PaymentProvider>()))
                .ReturnsAsync(new PaymentProvider
                {
                    Id = 100,
                    Name = updateDto.Name,
                    Url = updateDto.Url,
                    Currency = Currency.EUR,
                    IsActive = updateDto.IsActive
                });

            // Act
            var result = await _paymentProviderService.UpdateAsync(100, updateDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal("Updated Provider", result.Data.Name);
            Assert.Equal("https://updatedprovider.com", result.Data.Url);
            Assert.Equal("EUR", result.Data.Currency);
            Assert.False(result.Data.IsActive);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeletePaymentProvider_WhenExists()
        {
            _paymentProviderRepositoryMock.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _paymentProviderService.DeleteAsync(1);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFailure_WhenNotExists()
        {
            _paymentProviderRepositoryMock.Setup(repo => repo.DeleteAsync(999)).ReturnsAsync(false);

            // Act
            try
            {
                var result = await _paymentProviderService.DeleteAsync(999);

                // If no exception is thrown, fail the test
                Assert.Fail("Expected WriteException was not thrown.");
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsType<WriteException>(ex);
                Assert.Equal("Payment provider with ID 999 could not be deleted. It may not exist or the deletion operation failed.", ex.Message);
            }
        }
    }
}
