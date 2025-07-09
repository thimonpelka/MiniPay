using MiniPay.Application.Repositories;
using MiniPay.Application.Models;
using MiniPay.Application.DTOs;

namespace MiniPay.Application.Services
{
    public class PaymentProviderService : IPaymentProviderService
    {
        private readonly IPaymentProviderRepository _paymentProviderRepository;

        public PaymentProviderService(IPaymentProviderRepository paymentProviderRepository)
        {
            _paymentProviderRepository = paymentProviderRepository;
        }

        public async Task<Result<IEnumerable<PaymentProviderDto>>> GetAllAsync()
        {
            // Attempt to retrieve all payment providers from the repository
            IEnumerable<PaymentProvider> paymentProviders;
            try
            {
                paymentProviders = await _paymentProviderRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                // Return a failure result if an exception occurs
                return Result<IEnumerable<PaymentProviderDto>>.Fail($"An error occurred while retrieving payment providers: {ex.Message}", 500);
            }

            // Return a success result with the mapped DTOs
            return Result<IEnumerable<PaymentProviderDto>>.Success(paymentProviders.Select(MapPaymentProviderToDto));
        }

        public async Task<Result<PaymentProviderDto>> GetByIdAsync(int id)
        {
            // Attempt to retrieve the payment provider by ID
            PaymentProvider? paymentProvider = null;
            try
            {
                paymentProvider = await _paymentProviderRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
				// If an exception occurs, return a failure result
                return Result<PaymentProviderDto>.Fail($"An error occurred while retrieving the payment provider with ID {id}: {ex.Message}", 500);
            }

            // If the payment provider is not found, return a failure result
            if (paymentProvider == null)
            {
                return Result<PaymentProviderDto>.Fail($"Payment provider with ID {id} not found.", 201);
            }

            // Return a success result with the mapped DTO
            return Result<PaymentProviderDto>.Success(MapPaymentProviderToDto(paymentProvider));
        }

        public async Task<Result<PaymentProviderDto>> CreateAsync(CreatePaymentProviderDto createDto)
        {
            // Validate the input DTO
            if (!Enum.TryParse<Currency>(createDto.Currency.ToUpper(), out var currency))
            {
                return Result<PaymentProviderDto>.Fail($"Invalid currency: {createDto.Currency}", 422);
            }

            // Create a new PaymentProvider instance from the DTO
            var paymentProvider = new PaymentProvider
            {
                Name = createDto.Name,
                Url = createDto.Url,
                IsActive = createDto.IsActive,
                Currency = currency,
                Description = createDto.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };


            // Attempt to create the payment provider in the repository
            PaymentProvider? createdPaymentProvider = null;
            try
            {
                createdPaymentProvider = await _paymentProviderRepository.CreateAsync(paymentProvider);
            }
            catch (Exception ex)
            {
                // If an exception occurs, return a failure result
                return Result<PaymentProviderDto>.Fail($"An error occurred while creating the payment provider: {ex.Message}", 500);
            }

            return Result<PaymentProviderDto>.Success(MapPaymentProviderToDto(createdPaymentProvider));
        }

        public async Task<Result<PaymentProviderDto>> UpdateAsync(int id, UpdatePaymentProviderDto updateDto)
        {
            // Attempt to retrieve the existing payment provider by ID
            PaymentProvider? existingPaymentProvider = null;
            try
            {
                existingPaymentProvider = await _paymentProviderRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
				// If an exception occurs, return a failure result
                return Result<PaymentProviderDto>.Fail($"An error occurred while retrieving the payment provider with ID {id}: {ex.Message}", 500);
            }

            // If the payment provider does not exist, return a failure result
            if (existingPaymentProvider == null)
            {
                return Result<PaymentProviderDto>.Fail($"Payment provider with ID {id} not found.", 201);
            }

            // Validate the input DTO
            if (!Enum.TryParse<Currency>(updateDto.Currency.ToUpper(), out var currency))
            {
                return Result<PaymentProviderDto>.Fail($"Invalid currency: {updateDto.Currency}", 422);
            }

            // Create a new PaymentProvider instance with the updated values
            var paymentProvider = new PaymentProvider
            {
                Id = id,
                Name = updateDto.Name,
                Url = updateDto.Url,
                IsActive = updateDto.IsActive,
                Currency = currency,
                Description = updateDto.Description,
                CreatedAt = existingPaymentProvider.CreatedAt,
                UpdatedAt = DateTime.UtcNow
            };

            // Attempt to update the payment provider in the repository
            PaymentProvider? updatedPaymentProvider = null;
            try
            {
                updatedPaymentProvider = await _paymentProviderRepository.UpdateAsync(id, paymentProvider);
            }
            catch (Exception ex)
            {
				// If an exception occurs, return a failure result
                return Result<PaymentProviderDto>.Fail($"An error occurred while updating the payment provider with ID {id}: {ex.Message}", 500);
            }

			// If the update failed and returned null, return a failure result
            if (updatedPaymentProvider == null)
            {
                return Result<PaymentProviderDto>.Fail($"Failed to update payment provider with ID {id}.", 500);
            }

			// Return a success result with the mapped DTO
            return Result<PaymentProviderDto>.Success(MapPaymentProviderToDto(updatedPaymentProvider));
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
			// Attempt to delete the payment provider by ID
            try
            {
                bool deleted = await _paymentProviderRepository.DeleteAsync(id);
				
				if (!deleted)
				{
					// If the deletion was not successful, return a failure result
					return Result<bool>.Fail($"Payment provider with ID {id} could not be deleted.", 400);
				}

                return Result<bool>.Success(deleted);
            }
            catch (Exception ex)
            {
				// If an exception occurs, return a failure result
                return Result<bool>.Fail($"An error occurred while deleting the payment provider with ID {id}: {ex.Message}", 500);
            }
        }

        private static PaymentProviderDto MapPaymentProviderToDto(PaymentProvider paymentProvider)
        {
            return new PaymentProviderDto
            {
                Id = paymentProvider.Id,
                Name = paymentProvider.Name,
                Url = paymentProvider.Url,
                IsActive = paymentProvider.IsActive,
                Currency = paymentProvider.Currency.ToString(),

                Description = paymentProvider.Description,

                CreatedAt = paymentProvider.CreatedAt,
                UpdatedAt = paymentProvider.UpdatedAt
            };
        }
    }
}
