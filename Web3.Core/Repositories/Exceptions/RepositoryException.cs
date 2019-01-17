using System;
using Web3.Core.Exceptions;

namespace Web3.Core.Repositories.Exceptions
{
    public class RepositoryException : AppException
    {
        public RepositoryException(string message)
            : base(message)
        {
        }

        public RepositoryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
