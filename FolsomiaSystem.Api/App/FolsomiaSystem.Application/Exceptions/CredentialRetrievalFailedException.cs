using System;

namespace FolsomiaSystem.Application.Exceptions
{
    public class CredentialRetrievalFailedException : Exception
    {
        public CredentialRetrievalFailedException() : base() { }
        public CredentialRetrievalFailedException(string message) : base(message) { }
        public CredentialRetrievalFailedException(string message, Exception innerException) : base(message, innerException) { }
    }
}