namespace MiniPay.Application.Exceptions
{

    /// <summary>
    /// Exception thrown when database operations fail.
    /// </summary>
	[Serializable]
    public class DatabaseException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the DatabaseException Class.
        /// </summary>
        public DatabaseException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the DatabaseException Class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public DatabaseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
