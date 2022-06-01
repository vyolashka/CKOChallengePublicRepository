using DTOs;
using System;

namespace Services.Interfaces
{
    public interface IPaymentService
    {
        PostPutResponse CreatePayment(CreatePaymentDTO paymentDTO);

        PaymentDetailsDTO GetPaymentById(Guid paymentId);
    }
}
