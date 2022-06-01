using DTOs;
using Services.Interfaces;
using System;

namespace Services.BankSimulator
{
    public class BankSimulator : IBankSimulator
    {
        // In order to test the bank simulator in both cases (successful and failed payment) I implemented
        // very simple method which returns successful response in case the payment amount is <= 500 (regardless of the currency)
        public BankSimulatorResponse ProcessPayment(CreatePaymentDTO payment)
        {
            if (payment == null)
            {
                throw new ArgumentNullException();
            }

            if (payment.PaymentAmount <= 500)
            {
                return new BankSimulatorResponse
                {
                    IsPaymentSuccessful = true,
                    BankSimlatorPaymentReferenceNumber = Guid.NewGuid()
                };
            }
            else
            {
                return new BankSimulatorResponse
                {
                    IsPaymentSuccessful = false,
                    ErrorMessage = "Payment rejected. Insufficient amount in bank account."
                };
            }
        }
    }
}
