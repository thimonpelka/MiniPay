using System.ComponentModel.DataAnnotations;

namespace MiniPay.Application.DTOs {

	/*
	 * PaymentProviderDto class represents the data transfer object of a payment provider in the MiniPay application.
	 */
	public class PaymentProviderDto {
		public required int Id { get; set; }

		// Required properties
		public required string Name { get; set; }
		public required string Url { get; set; }
		public required bool IsActive { get; set; }
		public required string Currency { get; set; } // Changed to string for network compatibility

		// Optional properties
		public string? Description { get; set; }

		// Timestamps
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public DateTime UpdatedAt { get; set; } = DateTime.Now;
	}

	/*
	 * CreatePaymentProviderDto class represents the data transfer object for creating a new payment provider.
	 */
	public class CreatePaymentProviderDto {
		[Required, MinLength(3), MaxLength(100)]
		public required string Name { get; set; }

		[Required, MaxLength(200)]
		public required string Url { get; set; }

		[Required]
		public required bool IsActive { get; set; }

		[Required, MinLength(3), MaxLength(3)]
		public required string Currency { get; set; } // Changed to string for network compatibility

		[MaxLength(500)]
		public string? Description { get; set; }
	}

	/*
	 * UpdatePaymentProviderDto class represents the data transfer object for updating an existing payment provider.
	 */
	public class UpdatePaymentProviderDto {
		[Required, MinLength(3), MaxLength(100)]
		public required string Name { get; set; }

		[Required, MaxLength(200)]
		public required string Url { get; set; }

		[Required]
		public required bool IsActive { get; set; }

		[Required, MinLength(3), MaxLength(3)]
		public required string Currency { get; set; } // Changed to string for network compatibility

		[MaxLength(500)]
		public string? Description { get; set; }
	}

}
