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

        public async Task<IEnumerable<PaymentProviderDto>> GetAllAsync()
        {
            var paymentProviders = await _paymentProviderRepository.GetAllAsync();
            return paymentProviders.Select(MapPaymentProviderToDto);
        }

        public async Task<Result<PaymentProviderDto>> GetByIdAsync(int id)
        {
            var paymentProvider = await _paymentProviderRepository.GetByIdAsync(id);

            if (paymentProvider == null)
            {
                return Result<PaymentProviderDto>.Fail($"Payment provider with ID {id} not found.");
            }

            return Result<PaymentProviderDto>.Success(MapPaymentProviderToDto(paymentProvider));
        }

        public async Task<Result<PaymentProviderDto>> CreateAsync(CreatePaymentProviderDto createDto)
        {
            if (!Enum.TryParse<Currency>(createDto.Currency.ToUpper(), out var currency))
            {
                return Result<PaymentProviderDto>.Fail($"Invalid currency: {createDto.Currency}");
            }

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

            var createdPaymentProvider = await _paymentProviderRepository.CreateAsync(paymentProvider);
            return Result<PaymentProviderDto>.Success(MapPaymentProviderToDto(createdPaymentProvider));
        }

        public async Task<Result<PaymentProviderDto>> UpdateAsync(int id, UpdatePaymentProviderDto updateDto)
        {
            var existingPaymentProvider = await _paymentProviderRepository.GetByIdAsync(id);
            if (existingPaymentProvider == null)
            {
                return Result<PaymentProviderDto>.Fail($"Payment provider with ID {id} not found.");
            }

            if (!Enum.TryParse<Currency>(updateDto.Currency.ToUpper(), out var currency))
            {
                return Result<PaymentProviderDto>.Fail($"Invalid currency: {updateDto.Currency}");
            }

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

            var updatedPaymentProvider = await _paymentProviderRepository.UpdateAsync(id, paymentProvider);

			if (updatedPaymentProvider == null)
			{
				return Result<PaymentProviderDto>.Fail($"Failed to update payment provider with ID {id}.");
			}

			return Result<PaymentProviderDto>.Success(MapPaymentProviderToDto(updatedPaymentProvider));
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _paymentProviderRepository.DeleteAsync(id);
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
