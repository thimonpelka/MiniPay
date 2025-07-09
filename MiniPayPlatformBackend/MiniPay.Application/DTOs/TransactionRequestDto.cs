namespace MiniPay.Application.DTOs {
	/*
	 * This class represents a request to initiate a transaction.
	 */
	public class TransactionRequestDto {
		public required int PaymentProviderId { get; set; }
		public required decimal Amount { get; set; }
		public required string Description { get; set; }
		public required string ReferenceId { get; set; }
	}
}
