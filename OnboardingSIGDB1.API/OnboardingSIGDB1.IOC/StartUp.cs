using Microsoft.Extensions.DependencyInjection;
using OnboardingSIGDB1.Data;
using OnboardingSIGDB1.Data.Repositorios;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Interfaces.Services;
using OnboardingSIGDB1.Domain.Services;

namespace OnboardingSIGDB1.IOC
{
    public static class StartUp
    {
        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IFuncionarioRepository), typeof(FuncionarioRepository));
            services.AddScoped(typeof(IEmpresaRepository), typeof(EmpresaRepository));
            
            return services;
        }

        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IEmpresaService), typeof(EmpresaService));
            services.AddScoped(typeof(IFuncionarioService), typeof(FuncionarioService));
            services.AddScoped(typeof(ICargoService), typeof(CargoService));
            services.AddScoped(typeof(IVinculacaoFuncionarioEmpresaService), typeof(VinculacaoFuncionarioEmpresaService));
            services.AddScoped(typeof(IVinculacaoFuncionarioCargosService), typeof(VinculacaoFuncionarioCargosService));
            


            return services;
        }
    }
}
