using Models.Exceptions;
using System.Diagnostics;
using Services.Interfaces;
using Models;

namespace NodesTestApp.Middlewares
{
    public class ExceptionLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILogExceptionService service)
        {
            try
            {
                await _next(context);
            }
            catch (SecureException ex)
            {
                await HandleSecureExceptionAsync(service, context, ex);
            }
            catch (Exception ex)
            {
                await HandleGenericExceptionAsync(service, context, ex);
            }
        }

        private async Task HandleSecureExceptionAsync(ILogExceptionService service, HttpContext context, SecureException ex)
        {
            var journalItem = new JournalItem
            {
                EventId = ex.HResult,
                ErrorMessage = ex.Message,
                Timestamp = DateTime.UtcNow,
                QueryParameters = context.Request.QueryString.Value,
                BodyParameters = await new StreamReader(context.Request.Body).ReadToEndAsync(),
                StackTrace = new StackTrace(ex, true).ToString()
            };

            await service.AddJournalItemAsync(journalItem);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync(new
            {
                type = "Secure",
                id = journalItem.Id,
                data = new { message = ex.Message }
            }.ToString());
        }

        private async Task HandleGenericExceptionAsync(ILogExceptionService service, HttpContext context, Exception ex)
        {
            var journalItem = new JournalItem
            {
                EventId = ex.HResult,
                ErrorMessage = ex.Message,
                Timestamp = DateTime.UtcNow,
                QueryParameters = context.Request.QueryString.Value,
                BodyParameters = await new StreamReader(context.Request.Body).ReadToEndAsync(),
                StackTrace = new StackTrace(ex, true).ToString()
            };

            await service.AddJournalItemAsync(journalItem);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync(new
            {
                type = "Exception",
                id = journalItem.Id,
                data = new { message = $"Internal server error ID = {ex.HResult}" }
            }.ToString());
        }
    }
}

