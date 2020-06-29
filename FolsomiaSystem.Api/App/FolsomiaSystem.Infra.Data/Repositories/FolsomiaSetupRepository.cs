using FolsomiaSystem.Application.Interfaces.Repositories;
using FolsomiaSystem.Domain.Entities;

namespace FolsomiaSystem.Infra.Data.Repositories
{
    public class FolsomiaSetupRepository : RepositoryBase<FolsomiaSetup>, IFolsomiaSetupRepository
    {
        public FolsomiaSetupRepository(DefaultContext context) : base(context)
        {
        }
    }
}
