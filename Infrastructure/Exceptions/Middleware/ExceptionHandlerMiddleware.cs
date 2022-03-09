using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Infrastructure.Exceptions.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
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
                var response = context.Response;
                response.ContentType = "application/json";

                IDictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

                if (error is ValidationException validationException)
                {
                    errors = validationException.Failures;
                    response.StatusCode = 406;
                }
                else if (error is IHandledException)
                {
                    errors.Add("handled exception", new List<string> { error.Message });
                    response.StatusCode = 406;
                }
                else
                {
                    errors.Add("unhandled exception", new List<string> { error.Message });
                    response.StatusCode = 500;
                }


                await response.WriteAsync(
                    JsonConvert.SerializeObject(ServiceResult<object>.Set(null, errors)));

            }
        }
    }
}


