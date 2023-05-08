using System.Net;
using GeometricFigures.Exceptions;

namespace GeometricFigures.Middleware
{
    public static class HttpResponseExtensions
    {
        public static void TransformExceptionToResponse(this HttpResponse response, Exception exception)
        {
            response.StatusCode = GetStatusCode(exception);
        }


        private static int GetStatusCode(Exception exception)
        {
            if (exception is FigureModelException serviceException)
            {
                return serviceException.StatusCode;
            }

            return (int)HttpStatusCode.InternalServerError;
        }
    }
}
