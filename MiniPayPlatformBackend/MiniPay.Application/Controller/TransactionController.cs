using Microsoft.AspNetCore.Mvc;
using MiniPay.Application.DTOs;
using MiniPay.Application.Services;

namespace MiniPay.Application.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class TransactionController : ControllerBase {
		private readonly ITransactionService _transactionService;
		private readonly ILogger<TransactionController> _logger;

		public TransactionController(ITransactionService transactionService, ILogger<TransactionController> logger) {
			_transactionService = transactionService;
			_logger = logger;
		}

		[HttpPost]
		public async Task<ActionResult<TransactionResultDto>> ExecuteTransactionAsync(TransactionRequestDto createDto) {
			if (!ModelState.IsValid) {
				return BadRequest(ModelState);
			}

			_logger.LogInformation("Creating a new transaction");

			var result = await _transactionService.ExecuteTransactionAsync(createDto);

			if (!result.IsSuccess || result.Data == null) {
				return StatusCode(result.ErrorCode, result.ErrorMessage);
			}

			return Ok(result.Data);
		}
	}
}
