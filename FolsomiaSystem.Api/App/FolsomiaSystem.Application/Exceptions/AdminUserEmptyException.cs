using System;

namespace FolsomiaSystem.Application.Exceptions
{
    class AdminUserEmptyException : Exception
    {
        public AdminUserEmptyException(string message) : base(message)
        {

        }
    }
}
