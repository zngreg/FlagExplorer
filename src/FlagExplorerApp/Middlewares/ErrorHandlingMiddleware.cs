namespace FlagExplorerApp.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                var corelationId = Guid.NewGuid().ToString();
                _logger.LogError(ex, $"An error occurred with corelationId: {corelationId}, and message: {ex.Message}");

                httpContext.Response.StatusCode = 400;
                httpContext.Response.ContentType = "application/json";
                var errorResponse = new { message = $"An unexpected error occurred. Contact administrator. CorelationId: {corelationId} " };
                await httpContext.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }
}