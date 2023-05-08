namespace GeometricFigures.Exceptions
{
    public class FigureModelException : Exception
    {
        private const string ErrorMessageText = "operation-not-allowed";
        private int _statusCode;
        private string _errorMessage;

        public int StatusCode
        {
            get => _statusCode;

            protected set
            {
                if (value is < 500 and >= 400)
                {
                    _statusCode = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(StatusCode),
                        $"Status code {value} is out of range [400-499].");
                }
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;

            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(nameof(ErrorMessage),
                        $"Error message should be specified.");
                }

                _errorMessage = value;
            }
        }

        public FigureModelException(int statusCode = 400, string errorMessage = ErrorMessageText)
        {
            StatusCode = statusCode;
            _errorMessage = errorMessage;
            ErrorMessage = errorMessage;
        }

        public FigureModelException(Exception exception,
            int statusCode = 400,
            string errorMessage = ErrorMessageText) : base(errorMessage, exception)
        {
            StatusCode = statusCode;
            _errorMessage = errorMessage;
            ErrorMessage = errorMessage;
        }
    }
}