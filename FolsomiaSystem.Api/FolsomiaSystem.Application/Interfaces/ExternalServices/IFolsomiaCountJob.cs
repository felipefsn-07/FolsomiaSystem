using FolsomiaSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FolsomiaSystem.Application.Interfaces.ExternalServices
{
    public interface IFolsomiaCountJob
    {
       Task<FolsomiaCount> FolsomiaCountJobPython(string imgIn, string imgOut, string localPythonFolsomiaCount);
    }
}
