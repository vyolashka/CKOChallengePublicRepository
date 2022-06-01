using System;

namespace DTOs
{
    public class PaymentDetailsDTO
    {
        public Guid Id { get; set; }

        public string MaskedCardNumber { get; set; }

        public string CardHolderNames { get; set; }

        public int CardExpirationMonth { get; set; }

        public int CardExpirationYear { get; set; }

        public string Status { get; set; }

        public string Currency { get; set; }

        public decimal PaymentAmount { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastModifiedAt { get; set; }
    }
}
