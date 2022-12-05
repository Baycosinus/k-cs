using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Kompanion.Api.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Kompanion.Api.Middlewares
{
    
    public class Response<T>
    {
        public Meta meta { get; set; } = new Meta();
        public T response { get; set; }
    }

    public class Meta 
    {
        public int code { get; set; }
        public IEnumerable<Error> errors { get; set; }
    }

    public class Error 
    {
        public string key { get; set; }
        public string message { get; set; }
    }

    public class ErrorHandling
    {
        private readonly ILogger<ErrorHandling> _logger;
        private readonly RequestDelegate next;
        public ErrorHandling(RequestDelegate next, ILogger<ErrorHandling> logger)
        {
            this.next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, e.Message);
                await HandleExceptionAsync(context, e);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var result = new Response<bool>();
            result.response = false;
            
            var code = HttpStatusCode.InternalServerError;

            if (exception is KompanionException)
            {
                result.meta.code = exception.HResult;
                result.meta.errors = new List<Error>() { new Error() { message = exception.Message } };
            }
            else
            {
                result.meta.code = KompanionErrors.INTERNAL_ERROR.Item1;
                result.meta.errors = new List<Error> { new Error() { message = KompanionErrors.INTERNAL_ERROR.Item2 } };
            }

            var response = JsonConvert.SerializeObject(result);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(response);
        }
    }
}