namespace FlagExplorerApp.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Logger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, Logger<ErrorHandlingMiddleware> logger)
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
                _logger.LogError($"An error occurred: {ex.Message}", ex);

                // Return a generic error response
                httpContext.Response.StatusCode = 400;
                httpContext.Response.ContentType = "application/json";
                var errorResponse = new { message = "An unexpected error occurred." };
                await httpContext.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }
}