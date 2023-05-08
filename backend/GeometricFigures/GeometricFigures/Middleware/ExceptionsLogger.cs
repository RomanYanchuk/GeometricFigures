using GeometricFigures.Exceptions;

namespace GeometricFigures.Middleware
{
    public class ExceptionsLogger
    {
        private readonly RequestDelegate _next;

        public ExceptionsLogger(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception exception)
            {
                context.Response.TransformExceptionToResponse(exception);
                var logger = context.RequestServices.GetService<ILogger<ExceptionsLogger>>();

                if (exception is FigureModelException serviceException)
                {
                    logger.LogWarning(serviceException, serviceException.Message);
                    context.Response.Headers.Add("x-error-message", serviceException.ErrorMessage);
                }
                else
                {
                    logger.LogError(exception, exception.Message);
                }
            }
        }
    }
}