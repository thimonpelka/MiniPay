using MiniPay.Application.Shared;
using MiniPay.Application.Repositories;
using MiniPay.Application.Models;
using MiniPay.Application.DTOs;
using MiniPay.Application.Exceptions;

namespace MiniPay.Application.Services
{
    /**
     * @brief Service for managing payment providers.
     */
    public class PaymentProviderService : IPaymentProviderService
    {
        private readonly IPaymentProviderRepository _paymentProviderRepository;

        public PaymentProviderService(IPaymentProviderRepository paymentProviderRepository)
        {
            _paymentProviderRepository = paymentProviderRepository;
        }

        /**
         * @brief Retrieves all payment providers based on the provided query parameters.
         *
         * @param queryDto The query parameters for filtering.
         * @return A result containing a collection of payment provider DTOs or an error message.
         */
        public async Task<IEnumerable<PaymentProviderDto>> GetAllAsync(PaymentProviderQueryDto queryDto)
        {
            // Retrieve all payment providers from the repository
            IEnumerable<PaymentProvider> paymentProviders = await _paymentProviderRepository.GetAllAsync(queryDto);

            // Return a success result with the mapped DTOs
            return paymentProviders.Select(MapPaymentProviderToDto);
        }

        /**
         * @brief Retrieves a payment provider by its ID.
         *
         * @param id The ID of the payment provider to retrieve.
         * @return A result containing the payment provider DTO or an error message.
         */
        public async Task<PaymentProviderDto> GetByIdAsync(int id)
        {
            // Attempt to retrieve the payment provider by ID
            PaymentProvider? paymentProvider = await _paymentProviderRepository.GetByIdAsync(id);

            // If the payment provider is not found, return a failure result
            if (paymentProvider == null)
            {
                throw new RetrievalException($"Payment provider with ID {id} not found.");
            }

            // Return a success result with the mapped DTO
            return MapPaymentProviderToDto(paymentProvider);
        }

        /**
         * @brief Creates a new payment provider.
         *
         * @param createDto The DTO containing the details of the payment provider to create.
         * @return A result containing the created payment provider DTO or an error message.
         */
        public async Task<PaymentProviderDto> CreateAsync(CreatePaymentProviderDto createDto)
        {
            // Validate the input DTO
            if (!Enum.TryParse<Currency>(createDto.Currency.ToUpper(), out var currency))
            {
                throw new ValidationException($"Invalid currency: {createDto.Currency}");
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
            PaymentProvider createdPaymentProvider = await _paymentProviderRepository.CreateAsync(paymentProvider);

            return MapPaymentProviderToDto(createdPaymentProvider);
        }

        /**
         * @brief Updates an existing payment provider by its ID.
         *
         * @param id The ID of the payment provider to update.
         * @param updateDto The DTO containing the updated details of the payment provider.
         * @return A result containing the updated payment provider DTO or an error message.
         */
        public async Task<PaymentProviderDto> UpdateAsync(int id, UpdatePaymentProviderDto updateDto)
        {
            // Attempt to retrieve the existing payment provider by ID
            PaymentProvider? existingPaymentProvider = await _paymentProviderRepository.GetByIdAsync(id);

            // If the payment provider does not exist, return a failure result
            if (existingPaymentProvider == null)
            {
                throw new RetrievalException($"Payment provider with ID {id} not found.");
            }

            // Validate the input DTO
            if (!Enum.TryParse<Currency>(updateDto.Currency.ToUpper(), out var currency))
            {
                throw new ValidationException($"Invalid currency: {updateDto.Currency}");
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
            PaymentProvider? updatedPaymentProvider = await _paymentProviderRepository.UpdateAsync(id, paymentProvider);

            // If the update failed and returned null, return a failure result
            if (updatedPaymentProvider == null)
            {
                throw new WriteException($"Failed to update payment provider with ID {id}. It may not exist or the update operation failed.");
            }

            // Return a success result with the mapped DTO
            return MapPaymentProviderToDto(updatedPaymentProvider);
        }

        /**
         * @brief Deletes a payment provider by its ID.
         *
         * @param id The ID of the payment provider to delete.
         * @return A result indicating whether the deletion was successful or an error message.
         */
        public async Task<bool> DeleteAsync(int id)
        {
            // Attempt to delete the payment provider by ID
            bool deleted = await _paymentProviderRepository.DeleteAsync(id);

            if (!deleted)
            {
                // If the deletion was not successful, return a failure result
                throw new WriteException($"Payment provider with ID {id} could not be deleted. It may not exist or the deletion operation failed.");
            }

            return deleted;
        }

        /**
         * @brief Maps a PaymentProvider entity to a PaymentProviderDto.
         *
         * @param paymentProvider The PaymentProvider entity to map.
         * @return A PaymentProviderDto containing the mapped values.
         */
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
