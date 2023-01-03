using AutoMapper;

namespace WebApplication1.Config
{
    public class AutoMapperConfig
    {
        public MapperConfiguration MapperConfiguration { get; }

        public AutoMapperConfig()
        {
            var profiles = GetProfiles();

            this.MapperConfiguration = new MapperConfiguration(cfg =>
            {
                foreach (var profile in profiles)
                {
                    cfg.AddProfile(profile);
                }
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            IMapper mapper = MapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);
        }

        public IList<Profile> GetProfiles()
        {
            var profiles = new List<Profile>
            {
                new AutoMapperProfile()
            };

            return profiles;
        }
    }
}
