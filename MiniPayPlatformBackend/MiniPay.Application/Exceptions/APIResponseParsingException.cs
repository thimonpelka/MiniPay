namespace MiniPay.Application.Exceptions
{
	/**
	 * @brief APIResponseParsingException class represents an exception that occurs when there is an error in parsing the API response.
	 */
	[Serializable]
    public class APIResponseParsingException : Exception
    {
		/**
		 * @brief Initializes a new instance of the APIResponseParsingException class with a specified error message.
		 *
		 * @param message The error message that explains the reason for the exception.
		 * @return void
		 */
        public APIResponseParsingException(string message) : base(message)
        {
        }

		/**
		 * @brief Initializes a new instance of the APIResponseParsingException class with a specified error message and a reference to the inner exception that is the cause of this exception.
		 *
		 * @param message The error message that explains the reason for the exception.
		 * @param innerException The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
		 * @return void
		 */
        public APIResponseParsingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
