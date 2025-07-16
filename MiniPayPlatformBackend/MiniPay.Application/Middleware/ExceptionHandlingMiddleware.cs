using System.Net;
using System.Text.Json;
using MiniPay.Application.Exceptions;

namespace MiniPay.Application.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); // Call the next middleware
            }
            catch (WriteException ex) 
            {
                _logger.LogError(ex, "Write exception occurred.");

                await FormatResponse(context, (int)HttpStatusCode.InternalServerError, ex.Message);
            }
            catch (ValidationException ex) 
            {
                _logger.LogError(ex, "Validation exception occurred.");

                await FormatResponse(context, (int)HttpStatusCode.BadRequest, ex.Message);
            }
            catch (DatabaseException ex)
            {
                _logger.LogError(ex, "Database exception occurred.");

                await FormatResponse(context, (int)HttpStatusCode.InternalServerError, "A database error occurred. Please try again later.");
            }
            catch (RetrievalException ex)
            {
                _logger.LogError(ex, "Retrieval exception occurred.");

                await FormatResponse(context, (int)HttpStatusCode.NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");

                await FormatResponse(context, (int)HttpStatusCode.InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }

        private async Task FormatResponse(HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = new
            {
                success = false,
                message
            };

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}
