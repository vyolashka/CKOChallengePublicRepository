using DTOs;
using Services.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace Services.Tests
{
    public class PaymentValidationServiceTests
    {
        [Fact]
        public void IsPaymentValid_InvalidExpirationMonth_ShouldReturnFalse()
        {
            // Arrange
            var paymentValidationService = new PaymentValidationService();

            CreatePaymentDTO payment = new CreatePaymentDTO
            {
                CardNumber = "4111111111111111",
                CardHolderNames = "Test Tester",
                CardExpirationMonth = 1,
                CardExpirationYear = 22,
                CurrencyCode = "GBP",
                PaymentAmount = 500,
                CVV = "111"
            };

            // Act
            bool isPaymentValid = paymentValidationService.IsPaymentValid(payment, out List<KeyValuePair<string, string>> errorMessages);

            // Assert
            Assert.False(isPaymentValid);
            Assert.Equal("expirationMonth", errorMessages[0].Key);
            Assert.Equal("The expiration month is invalid.", errorMessages[0].Value);
        }

        [Fact]
        public void IsPaymentValid_InvalidExpirationYear_ShouldReturnFalse()
        {
            // Arrange
            var paymentValidationService = new PaymentValidationService();

            CreatePaymentDTO payment = new CreatePaymentDTO
            {
                CardNumber = "4111111111111111",
                CardHolderNames = "Test Tester",
                CardExpirationMonth = 12,
                CardExpirationYear = 21,
                CurrencyCode = "GBP",
                PaymentAmount = 500,
                CVV = "111"
            };

            // Act
            bool isPaymentValid = paymentValidationService.IsPaymentValid(payment, out List<KeyValuePair<string, string>> errorMessages);

            // Assert
            Assert.False(isPaymentValid);
            Assert.Equal("expirationYear", errorMessages[0].Key);
            Assert.Equal("The expiration year is invalid.", errorMessages[0].Value);
        }
    }
}
