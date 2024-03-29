using FolsomiaSystem.Application.DTOs;
using FolsomiaSystem.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace FolsomiaSystem.Application.Interfaces
{
    public interface IFolsomiaCountUseCase : IDisposable
    {
        Task<FolsomiaCount> CountFolsomiaCandidaAsync(FolsomiaCountInput folsomiaCountInput, string folsomiaJob, string path);
    }
}