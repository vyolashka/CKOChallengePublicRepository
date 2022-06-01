using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Interfaces;
using System;
using System.Linq;

namespace PaymentGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;

        private readonly IPaymentService _paymentService;

        public PaymentController(ILogger<PaymentController> logger, IPaymentService paymentService)
        {
            _logger = logger;
            _paymentService = paymentService;
        }

        [HttpPost]
        public IActionResult CreateEvent(CreatePaymentDTO model)
        {
            if (ModelState.IsValid)
            {
                PostPutResponse response = _paymentService.CreatePayment(model);

                // TODO: Return the appropriate status code, i.e. 201 (created), 400 (bad request), 500 (internal server error), etc.
                // Due to time constraints for this challenge I only return Ok or BadRequest.
                if (response.ErrorMessages.Count() == 0)
                {
                    return Ok(response);
                }

                return BadRequest(response);
            }

            return BadRequest();
        }

        [HttpGet("{id}")]
        public ActionResult<PaymentDetailsDTO> Get(Guid id)
        {
            PaymentDetailsDTO result = _paymentService.GetPaymentById(id);

            if (result == null)
                return NotFound();

            return result;
        }
    }
}
