using Microsoft.Extensions.DependencyInjection;
using OnboardingSIGDB1.Data;
using OnboardingSIGDB1.Data.Repositorios;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Interfaces.Services;
using OnboardingSIGDB1.Domain.Services.CargoServices;
using OnboardingSIGDB1.Domain.Services.EmpresaServices;
using OnboardingSIGDB1.Domain.Services.FuncionarioServices;

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
            services.AddScoped(typeof(ICargoRepository), typeof(CargoRepository));
            
            return services;
        }

        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IEmpresaService), typeof(EmpresaService));
            services.AddScoped(typeof(IEmpresaConsultaService), typeof(EmpresaConsultaService));
            services.AddScoped(typeof(IEmpresaDeleteService), typeof(EmpresaDeleteService));

            services.AddScoped(typeof(IFuncionarioService), typeof(FuncionarioService));
            services.AddScoped(typeof(IFuncionarioConsultaService), typeof(FuncionarioConsultaService));
            services.AddScoped(typeof(IFuncionarioDeleteService), typeof(FuncionarioDeleteService));
            services.AddScoped(typeof(IVinculacaoFuncionarioEmpresaService), typeof(VinculacaoFuncionarioEmpresaService));
            services.AddScoped(typeof(IVinculacaoFuncionarioCargosService), typeof(VinculacaoFuncionarioCargosService));
            
            services.AddScoped(typeof(ICargoService), typeof(CargoService));
            services.AddScoped(typeof(ICargoConsultaService), typeof(CargoConsultaService));
            services.AddScoped(typeof(ICargoDeleteService), typeof(CargoDeleteService));


            return services;
        }
    }
}
