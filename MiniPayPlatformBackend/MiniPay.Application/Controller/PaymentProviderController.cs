using Microsoft.AspNetCore.Mvc;
using MiniPay.Application.Services;
using MiniPay.Application.DTOs;

namespace MiniPay.Application.Controllers
{
	/**
	* @brief PaymentProviderController handles HTTP requests related to payment providers.
	*/
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

        /**
         * @brief Returns all Payment Providers
         *
         * @param PaymentProviderQueryDto query Parameter which gets automatically assigned by ASP.NET
         * @return A list of 0+ payment providers
         */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentProviderDto>>> GetAllAsync([FromQuery] PaymentProviderQueryDto queryParams)
        {
            _logger.LogInformation("Getting all payment providers");

            // Call the service to get all payment providers
            var result = await _paymentProviderService.GetAllAsync(queryParams);

            if (!result.IsSuccess || result.Data == null)
            {
                return StatusCode(result.ErrorCode, result.ErrorMessage);
            }

            return Ok(result.Data);
        }

		/**
		 * @brief Returns a Payment Provider By Id
		 *
		 * @param id of payment provider to query
		 * @return A single Payment Provider or null
		 */
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

		/**
		 * @brief Creates a new Payment Provider
		 *
		 * @param createDto Information to include in new payment provider
		 * @return Newly created object
		 */
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

		/**
		 * @brief Updates a Payment Provider
		 *
		 * @param id id of payment provider to update
		 * @param updateDto new information to set payment provider data to
		 * @return 
		 */
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

		/**
		 * @brief Deletes a Payment Provider by Id
		 *
		 * @param id id of provider to delete
		 * @return No Content & 204 if succesful
		 */
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
