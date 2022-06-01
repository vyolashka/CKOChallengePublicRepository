using DTOs;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PaymentGateway.CustomExceptionMiddleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _genericErrorMessage = "An unexpected error has occurred.";

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                // TODO: Log the error
                var response = context.Response;
                response.ContentType = "application/json";

                response.StatusCode = error switch
                {
                    ArgumentException e => (int)HttpStatusCode.BadRequest, // custom application error
                    KeyNotFoundException e => (int)HttpStatusCode.NotFound, // not found error
                    _ => (int)HttpStatusCode.InternalServerError, // unhandled error
                };

                var result = JsonConvert.SerializeObject(new ErrorDetails
                {
                    StatusCode = response.StatusCode,
                    Message = _genericErrorMessage
                });

                await response.WriteAsync(result);
            }
        }
    }
}
