using Models.Exceptions;
using System.Diagnostics;
using Services.Interfaces;
using Models;

namespace NodesTestApp.Middlewares
{
    public class ExceptionLoggingMiddleware
    {
        private const int ErrorDefaultStatusCode = StatusCodes.Status500InternalServerError;
        private const string ErrorDefaultContentType = "application/json";
        private const string SecureExceptionType = "Secure";
        private const string DefaultExceptionType = "Exception";
        private const string DefaultExceptionMessage = "Internal server error ID = {0}";
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
            catch (Exception ex)
            {
                await WriteException(service, context, ex);
            }
        }

        private async Task WriteException(ILogExceptionService service, HttpContext context, Exception ex)
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

            context.Response.ContentType = ErrorDefaultContentType;
            context.Response.StatusCode = ErrorDefaultStatusCode;
            string type;
            string message;
            if (ex is SecureException)
            {
                type = SecureExceptionType;
                message = ex.Message;
            }
            else
            {
                type = DefaultExceptionType;
                message = string.Format(DefaultExceptionMessage, ex.HResult);
            }

            await context.Response.WriteAsync(new
            {
                type = "Exception",
                id = journalItem.Id,
                data = new { message = message }
            }.ToString());
        }
    }
}

