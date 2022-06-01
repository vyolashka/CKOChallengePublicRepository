using DTOs;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Services.Services
{
    public class PaymentValidationService : IPaymentValidationService
    {
        public bool IsPaymentValid(CreatePaymentDTO payment, out List<KeyValuePair<string, string>> errorMessages)
        {
            bool result = true;
            errorMessages = new List<KeyValuePair<string, string>>();

            // Due to time contraints I am implementing only the validation oof the expiration month/year
            // The rest of the validations are either done with data annotations or they are listed below as "TODO".
            if (!ValidateCardExpirationMonthAndYear(payment.CardExpirationMonth, payment.CardExpirationYear, out KeyValuePair<string, string> expirationDateErrorMessages))
            {
                result = false;
                errorMessages.Add(expirationDateErrorMessages);
            }

            // TODO: Validate Currency Code

            // TODO: Validate CVV

            return result;
        }
                
        private bool ValidateCardExpirationMonthAndYear(int expirationMonth, int expirationYear, out KeyValuePair<string, string> errorMessage)
        {
            errorMessage = new KeyValuePair<string, string>();

            // Set the two digit max year so that "30" is not interpreted as "1930" but as "2030".
            CultureInfo ci = new CultureInfo(CultureInfo.CurrentCulture.LCID);
            ci.Calendar.TwoDigitYearMax = 2100;

            string expirationMonthAsString = expirationMonth < 10 ? "0" + expirationMonth.ToString() : expirationMonth.ToString();
            var parsedExpirationDate = DateTime.ParseExact(expirationMonthAsString + "/" + expirationYear.ToString(), "MM/yy", ci);
            
            // TODO if there is time: Validate separately the year and the month (in two different methods) and then validate them together,
            // comparing them to the current date in this method.
            if (parsedExpirationDate.Year < DateTime.UtcNow.Year)
            {
                errorMessage = new KeyValuePair<string, string>("expirationYear", "The expiration year is invalid.");
                return false;                
            }
            else if (parsedExpirationDate < DateTime.UtcNow)
            {
                errorMessage = new KeyValuePair<string, string>("expirationMonth", "The expiration month is invalid.");
                return false;
            }
            
            return true;
        }
    }
}
