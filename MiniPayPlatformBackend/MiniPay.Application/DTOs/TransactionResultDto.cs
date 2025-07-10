namespace MiniPay.Application.DTOs {

	/*
	 * @brief This class represents the result of a transaction.
	 */
	public class TransactionResultDto {
		public required string Status { get; set; }
		public required string TransactionId { get; set; }
		public required DateTime Timestamp { get; set; }
		public required string Message { get; set; }
		public required string ReferenceId { get; set; }
	}
}
