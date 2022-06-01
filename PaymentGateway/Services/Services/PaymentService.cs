using DbAccess.Interfaces;
using DTOs;
using Model.Enums;
using Model.Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Services
{
    public class PaymentService : IPaymentService
    {
        IAllRepositories _allRepositories;
        IBankSimulator _bankSimulator;
        IPaymentValidationService _paymentValidationService;

        public PaymentService(IAllRepositories allRepositories, IBankSimulator bankSimulator, IPaymentValidationService paymentValidationService)
        {
            _allRepositories = allRepositories;
            _bankSimulator = bankSimulator;
            _paymentValidationService = paymentValidationService;
        }

        /* TODO: Consider case when payment insert and Bank simulator transaction pass but there is an error during the update
         of the payment in the Payment Gateway database. Should all actions be wrapped in a transaction?
        but we do not control the bank simulator rollback so we should use some other approach. */
        public PostPutResponse CreatePayment(CreatePaymentDTO paymentDTO)
        {
            PostPutResponse response = new PostPutResponse();

            if (_paymentValidationService.IsPaymentValid(paymentDTO, out List<KeyValuePair<string, string>> validationErrorMessages))
            {
                // Save a Payment record into the Payment gateway database
                response.RecordId = CreatePayment(paymentDTO, out List<KeyValuePair<string, string>> errorMessages);
                response.ErrorMessages.AddRange(errorMessages);

                // If the Payment was successfully saved in the Payment gateway's DB, forward the payment request
                // to the CKO Bank simulator, which is going to execute the actual transfer of money
                if (response.RecordId != null && response.ErrorMessages.Count == 0)
                {
                    BankSimulatorResponse bankPaymentResponse = _bankSimulator.ProcessPayment(paymentDTO);

                    // Update the Payment record after receiving a response from the CKO Bank simulator
                    UpdatePaymentStatusAndReferenceNumber(response.RecordId.Value, bankPaymentResponse);

                    // Set the value of the response based on the result from calling the "CreatePayment" method of the CKO Bank simulator
                    response.IsPaymentProcessedSuccessfully = bankPaymentResponse.IsPaymentSuccessful;

                    // TODO: If there is time, think of a better way to return errors from the Bank simulator.
                    if (!string.IsNullOrWhiteSpace(bankPaymentResponse.ErrorMessage))
                    {
                        List<KeyValuePair<string, string>> bankSimulatorErrorMessages = new List<KeyValuePair<string, string>>();
                        bankSimulatorErrorMessages.Add(new KeyValuePair<string, string>("payment transaction", bankPaymentResponse.ErrorMessage));
                        response.ErrorMessages.AddRange(bankSimulatorErrorMessages);
                    }

                }
            }
            else
            {
                response.ErrorMessages.AddRange(validationErrorMessages);
                response.IsPaymentProcessedSuccessfully = false;
            }

            return response;
        }

        /// <summary>
        /// Creates a Payment record and saves it to the database.
        /// </summary>
        /// <param name="paymentDTO">The data transfer object passed to the controller method which calls this service method.</param>
        /// <param name="errorMessages">Error messages which might occur during the validation and saving of a Payment record.</param>
        /// <returns>The Id of the newly created Payment record.</returns>
        private Nullable<Guid> CreatePayment(CreatePaymentDTO paymentDTO, out List<KeyValuePair<string, string>> errorMessages)
        {
            errorMessages = new List<KeyValuePair<string, string>>();

            Payment payment = MapCreatePaymentDTOToPayment(paymentDTO);

            _allRepositories.PaymentRepository.Add(payment);
            _allRepositories.PaymentRepository.SaveChanges();

            return payment.Id;
        }

        /// <summary>
        /// Assigns a status to a payment. If the status is "PaymentSuccessful", it also assigns a value to the
        /// payment's property "BankSimlatorPaymentReferenceNumber".
        /// </summary>
        /// <param name="paymentId">The Id of the Payment record from the Payment Gateway's database.</param>
        /// <param name="status">A payment status based on the result returned from the Bank Simulator.</param>
        /// <param name="bankSimulatorPaymentReferenceNumber">A reference number/id of the Payment record created in the Bank Simulator's own database when the bank transaction was executed.</param>
        private void UpdatePaymentStatusAndReferenceNumber(Guid paymentId, BankSimulatorResponse bankPaymentResponse)
        {
            // Retrieve the payment to be updated from the DB.
            Payment payment = _allRepositories.PaymentRepository.Find(paymentId);

            if (payment == null)
            {
                throw new ArgumentException($"A payment with this id {paymentId} was not found.");
            }

            // If the payment was successful, assign the corresponding status to the Payment record and assign a value for the "BankSimlatorPaymentReferenceNumber" property.
            if (bankPaymentResponse.IsPaymentSuccessful)
            {
                payment.BankSimlatorPaymentReferenceNumber = bankPaymentResponse.BankSimlatorPaymentReferenceNumber;
                payment.Status = StatusesEnum.PaymentSuccessful;
            }
            else
            {
                // If thte payment failed, assign the corresponding status.
                payment.Status = StatusesEnum.PaymentFailed;
            }

            _allRepositories.PaymentRepository.Update(payment);
            _allRepositories.PaymentRepository.SaveChanges();
        }

        // TODO: If there is time, use automapper
        private Payment MapCreatePaymentDTOToPayment(CreatePaymentDTO paymentDTO)
        {
            Enum.TryParse(paymentDTO.CurrencyCode, out CurrenciesEnum resultingEnum);

            return new Payment
            {
                Id = Guid.NewGuid(),
                CardNumber = paymentDTO.CardNumber,
                CardHolderNames = paymentDTO.CardHolderNames,
                CardExpirationMonth = paymentDTO.CardExpirationMonth,
                CardExpirationYear = paymentDTO.CardExpirationYear,
                // Firs we create the Payment record in status "Pending"
                Status = StatusesEnum.PaymentPending,
                Currency = resultingEnum,
                PaymentAmount = paymentDTO.PaymentAmount,
                CreatedAt = DateTime.UtcNow,
                LastModifiedAt = DateTime.UtcNow
            };
        }

        public PaymentDetailsDTO GetPaymentById(Guid paymentId)
        {
            // Retrieve the payment from the database
            Payment payment = _allRepositories.PaymentRepository.Find(paymentId);

            if (payment != null)
            {
                // TODO: Use automapper
                PaymentDetailsDTO result = new PaymentDetailsDTO
                {
                    Id = payment.Id,
                    MaskedCardNumber = MaskCardNumberString(payment.CardNumber),
                    CardHolderNames = payment.CardHolderNames,
                    CardExpirationMonth = payment.CardExpirationMonth,
                    CardExpirationYear = payment.CardExpirationYear,
                    Status = payment.Status.ToString(),
                    Currency = payment.Currency.ToString(),
                    PaymentAmount = payment.PaymentAmount,
                    CreatedAt = payment.CreatedAt,
                    LastModifiedAt = payment.LastModifiedAt 
                };

                return result;
            }

            return null;
        }

        private string MaskCardNumberString(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
            {
                throw new ArgumentException("Card number is empty - nothing to mask.");
            }

            // Due to time contraints I will only consider masking a Visa card number which is 16 characteres long.
            if (cardNumber.Length < 16)
            {
                throw new ArgumentException("Card number length is invalid.");
            }

            StringBuilder strBuilder = new StringBuilder(cardNumber);
            strBuilder.Remove(3, 8);
            strBuilder.Insert(3, "********");

            return strBuilder.ToString();
        }
    }
}
