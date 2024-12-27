namespace Reserves.Domain.Exceptions
{
    public class DataValidationException : Exception
    {
        public string ErrorCode { get; set; }

        public DataValidationException()
        {
        }

      
        public DataValidationException(string message)
            : base(message)
        {
        }

       
        public DataValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

       
        public DataValidationException(string message, string errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}

