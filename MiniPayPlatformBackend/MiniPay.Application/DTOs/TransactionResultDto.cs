namespace MiniPay.Application.DTOs {
	public class TransactionResultDto {
		public required string Status { get; set; }
		public required string TransactionId { get; set; }
		public required DateTime Timestamp { get; set; }
		public required string Message { get; set; }
		public required string ReferenceId { get; set; }
	}
}
