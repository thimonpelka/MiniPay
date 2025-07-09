using Microsoft.AspNetCore.Mvc;
using MiniPay.Application.Services;
using MiniPay.Application.DTOs;

namespace MiniPay.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentProviderController : ControllerBase
    {
        private readonly IPaymentProviderService _paymentProviderService;
        private readonly ILogger<PaymentProviderController> _logger;

        public PaymentProviderController(IPaymentProviderService paymentProviderService, ILogger<PaymentProviderController> logger)
        {
            _paymentProviderService = paymentProviderService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentProviderDto>>> GetAllAsync()
        {
            _logger.LogInformation("Getting all payment providers");

            // Call the service to get all payment providers
            var result = await _paymentProviderService.GetAllAsync();

			if (!result.IsSuccess || result.Data == null)
			{
				return StatusCode(result.ErrorCode, result.ErrorMessage);
			}

            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentProviderDto>> GetByIdAsync(int id)
        {
            _logger.LogInformation($"Getting payment provider with ID: {id}");

            var result = await _paymentProviderService.GetByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
				return StatusCode(result.ErrorCode, result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<ActionResult<PaymentProviderDto>> CreateAsync(CreatePaymentProviderDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating a new payment provider");

            var result = await _paymentProviderService.CreateAsync(createDto);

            if (!result.IsSuccess || result.Data == null)
            {
				return StatusCode(result.ErrorCode, result.ErrorMessage);
            }

            // return CreatedAtAction(nameof(GetByIdAsync), new { id = result.Data.Id }, result.Data);
			return StatusCode(201, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PaymentProviderDto>> UpdateAsync(int id, UpdatePaymentProviderDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation($"Updating payment provider with ID: {id}");

            var result = await _paymentProviderService.UpdateAsync(id, updateDto);

            if (!result.IsSuccess || result.Data == null)
            {
				return StatusCode(result.ErrorCode, result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            _logger.LogInformation($"Deleting payment provider with ID: {id}");

            var result = await _paymentProviderService.DeleteAsync(id);

            if (!result.IsSuccess)
            {
				return StatusCode(result.ErrorCode, result.ErrorMessage);
            }

            return NoContent();
        }
    }
}
