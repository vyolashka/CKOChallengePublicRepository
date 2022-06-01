using Model.Enums;
using System;

namespace Model.Models
{
    public class Payment
    {
        public Guid Id { get; set; }

        public string CardNumber { get; set; }

        public string CardHolderNames { get; set; }

        public int CardExpirationMonth { get; set; }

        public int CardExpirationYear { get; set; }
                
        public StatusesEnum Status { get; set; }

        public CurrenciesEnum Currency { get; set; }

        public decimal PaymentAmount { get; set; }

        public Nullable<Guid> BankSimlatorPaymentReferenceNumber { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastModifiedAt { get; set; }
    }
}
