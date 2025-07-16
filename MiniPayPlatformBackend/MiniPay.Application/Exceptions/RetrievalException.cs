namespace MiniPay.Application.Exceptions
{

    /**
     * @brief DatabaseException class represents an exception that occurs during database operations.
     */
    [Serializable]
    public class RetrievalException : Exception
    {
        /**
         * @brief Initializes a new instance of the DatabaseException class with a specified error message.
         *
         * @param message The error message that explains the reason for the exception.
         * @return void
         */
        public RetrievalException(string message) : base(message)
        {
        }
    }
}
