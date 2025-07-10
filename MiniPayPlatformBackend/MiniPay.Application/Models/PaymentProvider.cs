namespace MiniPay.Application.Models {

    /*
     * @brief PaymentProvider class represents a payment provider in the MiniPay application.
     */
    public class PaymentProvider {
        public int Id { get; set; }

        // Required properties
        public required string Name { get; set; }
        public required string Url { get; set; }
        public required bool IsActive { get; set; }
        public required Currency Currency { get; set; } // Change to string?

        // Optional properties
        public string? Description { get; set; }

        // Timestamps
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    /*
     * @brief Currency enum represents the supported currencies in the MiniPay application.
     *
     * TODO: Add more currencies as needed.
     */
    public enum Currency {
        USD,
        EUR,
        GBP,
        JPY,
        AUD,
        CAD,
        CNY,
        INR,
        RUB,
    }
}
