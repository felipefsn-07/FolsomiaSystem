using System;

namespace FolsomiaSystem.Application.Exceptions
{
    public class ConfigurationMissingException : Exception
    {
        public ConfigurationMissingException() : base() { }
        public ConfigurationMissingException(string message) : base(message) { }
        public ConfigurationMissingException(string message, Exception innerException) : base(message, innerException) { }
    }
}
