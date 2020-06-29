using System;

namespace FolsomiaSystem.Application.Exceptions
{
    public class FolsomiaSetupInvalidException : Exception
    {
        public FolsomiaSetupInvalidException(string message) : base(message)
        {
        }
    }
}
