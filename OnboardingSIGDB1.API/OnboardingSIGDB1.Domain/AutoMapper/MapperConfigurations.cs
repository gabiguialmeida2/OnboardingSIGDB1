using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace OnboardingSIGDB1.Domain.AutoMapper
{
    public static class MapperConfigurations
    {
        public static IServiceCollection ConfigureMapper(this IServiceCollection services)
        {
            var mapperConfiguration = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new DtoToEntityProfile());
                conf.AddProfile(new EntityToDtoProfile());
            });

            IMapper mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }

    }
}
