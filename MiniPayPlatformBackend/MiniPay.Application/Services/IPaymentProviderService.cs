using MiniPay.Application.DTOs;

namespace MiniPay.Application.Services {
    /**
     * @brief Interface for HTTP request services.
     */
    public interface IPaymentProviderService
    {
        Task<IEnumerable<PaymentProviderDto>> GetAllAsync(PaymentProviderQueryDto queryDto);
        Task<PaymentProviderDto> GetByIdAsync(int id);
        Task<PaymentProviderDto> CreateAsync(CreatePaymentProviderDto createDto);
        Task<PaymentProviderDto> UpdateAsync(int id, UpdatePaymentProviderDto updateDto);
        Task<bool> DeleteAsync(int id);
    }
}
