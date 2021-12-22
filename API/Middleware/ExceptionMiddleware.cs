using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<Exception> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<Exception> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (HttpRequestException e)
            {
                var statusCode = (int) e.StatusCode.GetValueOrDefault(HttpStatusCode.InternalServerError);
                var json = GetJsonResponse(context, e, statusCode);
                await context.Response.WriteAsync(json);
            }
            catch (Exception e)
            {
                var json = GetJsonResponse(context, e, 500);
                await context.Response.WriteAsync(json);
            }
        }

        private string GetJsonResponse(HttpContext context, Exception e, int statusCode)
        {
            _logger.LogError(e, e.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new ProblemDetails
            {
                Status = statusCode,
                Detail = _env.IsDevelopment() && statusCode == 500 ? e.StackTrace : null,
                Title = e.Message
            };

            var options = new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
            var json = JsonSerializer.Serialize(response, options);

            return json;
        }
    }
}