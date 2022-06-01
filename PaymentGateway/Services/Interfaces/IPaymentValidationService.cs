using DTOs;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IPaymentValidationService
    {
        bool IsPaymentValid(CreatePaymentDTO payment, out List<KeyValuePair<string, string>> errorMessages);
    }
}
