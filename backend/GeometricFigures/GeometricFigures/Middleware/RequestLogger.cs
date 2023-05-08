namespace GeometricFigures.Middleware
{
    public class RequestLogger
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestLogger(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<RequestLogger>();
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);
            _logger.LogInformation("Request");
        }
    }
}
