using System;

namespace DTOs
{
    public class BankSimulatorResponse
    {
        // We assume that the Bank simulator creates a record in its own database and returns the payment id/reference number
        public Nullable<Guid> BankSimlatorPaymentReferenceNumber { get; set; }

        public bool IsPaymentSuccessful { get; set; }

        public string ErrorMessage { get; set; }
    }
}
