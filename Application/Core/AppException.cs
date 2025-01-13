namespace Application.Core
{
    public class AppException
    {
        public int StatusCode { get; }
        public string Message { get; }
        public string Details { get; }
        public AppException(int statusCode, string message, string details = null)
        {
            Details = details;
            Message = message;
            StatusCode = statusCode;
        }
    }
}