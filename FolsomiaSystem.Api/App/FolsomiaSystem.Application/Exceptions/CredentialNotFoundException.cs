using System;

namespace FolsomiaSystem.Application.Exceptions
{
    public class CredentialNotFoundException : Exception
    {
        public CredentialNotFoundException() : base() { }
        public CredentialNotFoundException(string message) : base(message) { }
        public CredentialNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}