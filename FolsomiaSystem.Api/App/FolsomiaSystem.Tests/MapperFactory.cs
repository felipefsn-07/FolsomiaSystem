using AutoMapper;
using FolsomiaSystem.Application;

namespace FolsomiaSystem.Tests.Application
{
    public static class MapperFactory
    {
        public static IMapper Create()
        {
            var profiles = new Profile[] { new DomainMappingProfile()};
            var configuration = new MapperConfiguration(cfg => cfg.AddProfiles(profiles));
            return new Mapper(configuration);
        }
    }
}


