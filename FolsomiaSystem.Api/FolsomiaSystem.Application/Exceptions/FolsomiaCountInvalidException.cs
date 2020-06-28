using System;

namespace FolsomiaSystem.Application.Exceptions
{
    class FolsomiaCountInvalidException : Exception
    {
      
        public FolsomiaCountInvalidException() : base() { }
        public FolsomiaCountInvalidException(string message) : base(message) { }
        public FolsomiaCountInvalidException(string message, Exception innerException) : base(message, innerException) { }
    }
}
