using Microsoft.EntityFrameworkCore;
using MiniPay.Application.Data;
using MiniPay.Application.Repositories;
using MiniPay.Application.Models;
using MiniPay.Application.DTOs;

namespace MiniPay.Tests.Repositories
{
    public class PaymentProviderRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly IPaymentProviderRepository _repository;

		// Query DTOs for testing
		private PaymentProviderQueryDto _emptyQueryDto = new PaymentProviderQueryDto {};
		private PaymentProviderQueryDto _isActiveQueryDto = new PaymentProviderQueryDto { IsActive = true };

        public PaymentProviderRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new PaymentProviderRepository(_context);

            _context.PaymentProviders.AddRange(
                new PaymentProvider
                {
                    Id = 1,
                    Name = "Provider 1",
                    Url = "https://provider1.com",
                    Currency = Currency.USD,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                },
                new PaymentProvider
                {
                    Id = 2,
                    Name = "Provider 2",
                    Url = "https://provider2.com",
                    Currency = Currency.EUR,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new PaymentProvider
                {
                    Id = 3,
                    Name = "Provider 3",
                    Url = "https://provider3.com",
                    Currency = Currency.GBP,
                    IsActive = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-3)
                }
            );
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllPaymentProviders()
        {
            // Act
            var result = await _repository.GetAllAsync(_emptyQueryDto);

            // Assert
            Assert.Equal(3, result.Count());
            Assert.Equal("Provider 1", result.First().Name);
            Assert.Equal("Provider 3", result.Last().Name);
        }

		[Fact]
		public async Task GetAllAsync_ShouldReturnActivePaymentProviders_WhenQueryIsActive()
		{
			// Act
			var result = await _repository.GetAllAsync(_isActiveQueryDto);

			// Assert
			Assert.Equal(2, result.Count());
			Assert.All(result, p => Assert.True(p.IsActive));
			Assert.Equal("Provider 1", result.First().Name);
			Assert.Equal("Provider 2", result.Last().Name);
		}

		[Fact]
		public async Task GetByIdAsync_ShouldReturnPaymentProvider_WhenExists()
		{
			// Act
			var result = await _repository.GetByIdAsync(1);

			// Assert
			Assert.NotNull(result);
			Assert.Equal("Provider 1", result.Name);
		}

		[Fact]
		public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
		{
			// Act
			var result = await _repository.GetByIdAsync(999);

			// Assert
			Assert.Null(result);
		}

		[Fact]
		public async Task CreateAsync_ShouldAddNewPaymentProvider()
		{
			// Arrange
			var newProvider = new PaymentProvider
			{
				Name = "Provider 4",
				Url = "https://provider4.com",
				Currency = Currency.USD,
				IsActive = true,
				CreatedAt = DateTime.UtcNow
			};

			// Act
			var createdProvider = await _repository.CreateAsync(newProvider);

			// Assert
			Assert.NotNull(createdProvider);
			Assert.Equal("Provider 4", createdProvider.Name);
			Assert.Equal(4, _context.PaymentProviders.Count());
		}

		[Fact]
		public async Task UpdateAsync_ShouldUpdateExistingPaymentProvider()
		{
			// Arrange
			var updateData = new PaymentProvider
			{
				Name = "Updated Provider 1",
				Url = "https://updatedprovider1.com",
				Currency = Currency.EUR,
				IsActive = false,
				Description = "Updated description"
			};

			// Act
			var updatedProvider = await _repository.UpdateAsync(1, updateData);

			// Assert
			Assert.NotNull(updatedProvider);
			Assert.Equal("Updated Provider 1", updatedProvider.Name);
			Assert.Equal("https://updatedprovider1.com", updatedProvider.Url);
			Assert.False(updatedProvider.IsActive);
		}

		[Fact]
		public async Task DeleteAsync_ShouldRemovePaymentProvider_WhenExists()
		{
			// Act
			var result = await _repository.DeleteAsync(2);

			// Assert
			Assert.True(result);
			Assert.Equal(2, _context.PaymentProviders.Count());
		}

		[Fact]
		public async Task DeleteAsync_ShouldReturnFalse_WhenNotExists()
		{
			// Act
			var result = await _repository.DeleteAsync(999);

			// Assert
			Assert.False(result);
		}
    }
}
