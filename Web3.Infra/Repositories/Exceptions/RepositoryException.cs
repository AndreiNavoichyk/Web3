using System;
using Web3.Infra.Exceptions;

namespace Web3.Infra.Repositories.Exceptions
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
