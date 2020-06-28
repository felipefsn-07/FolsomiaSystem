using System;

namespace FolsomiaSystem.Application.Exceptions
{
    class FolsomiaAdminUserAlterPasswordInvalid : Exception
    {
        public FolsomiaAdminUserAlterPasswordInvalid(string message) : base(message)
        {
        }
    }
}
