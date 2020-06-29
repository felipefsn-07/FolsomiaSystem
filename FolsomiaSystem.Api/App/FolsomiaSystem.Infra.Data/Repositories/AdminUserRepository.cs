using FolsomiaSystem.Application.Interfaces.Repositories;
using FolsomiaSystem.Domain.Entities;
using System.Collections.Generic;

namespace FolsomiaSystem.Infra.Data.Repositories
{
    public class AdminUserRepository : RepositoryBase<AdminUser>, IAdminUserRepository
    {
        public AdminUserRepository(DefaultContext context) : base(context)
        {
        }

    }
}
