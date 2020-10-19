using Microsoft.Extensions.DependencyInjection;
using OnboardingSIGDB1.Data;
using OnboardingSIGDB1.Data.Repositorios;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Interfaces.Services;
using OnboardingSIGDB1.Domain.Services.FuncionarioServices;
using OnboardingSIGDB1.Domain._Base;
using OnboardingSIGDB1.Data.Empresas.Repositorios;
using OnboardingSIGDB1.Domain.Empresas.Services;
using OnboardingSIGDB1.Domain.Empresas.Validators;
using OnboardingSIGDB1.Domain.Cargos.Services;
using OnboardingSIGDB1.Domain.Cargos.Validators;

namespace OnboardingSIGDB1.IOC
{
    public static class StartUp
    {
        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IConsultaBase<,>), typeof(ConsultaBase<,>));
            services.AddScoped(typeof(IFuncionarioRepository), typeof(FuncionarioRepository));
            services.AddScoped(typeof(IEmpresaRepository), typeof(EmpresaRepository));
            services.AddScoped(typeof(ICargoRepository), typeof(CargoRepository));

            return services;
        }

        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(ArmazenadorDeEmpresa));
            services.AddScoped(typeof(ExclusaoDeEmpresa));
            services.AddScoped(typeof(IValidadorDeCnpj), typeof(ValidadorDeCnpj));
            services.AddScoped(typeof(IValidadorDeEmpresaDuplicada), typeof(ValidadorDeEmpresaDuplicada));
            services.AddScoped(typeof(IValidadorDeEmpresaExistente), typeof(ValidadorDeEmpresaExistente));
            services.AddScoped(typeof(IValidadorDeEmpresaComFuncionarios), typeof(ValidadorDeEmpresaComFuncionarios));
            


            services.AddScoped(typeof(IFuncionarioService), typeof(FuncionarioService));
            services.AddScoped(typeof(IFuncionarioConsultaService), typeof(FuncionarioConsultaService));
            services.AddScoped(typeof(IFuncionarioDeleteService), typeof(FuncionarioDeleteService));
            services.AddScoped(typeof(IVinculacaoFuncionarioEmpresaService), typeof(VinculacaoFuncionarioEmpresaService));
            services.AddScoped(typeof(IVinculacaoFuncionarioCargosService), typeof(VinculacaoFuncionarioCargosService));

            services.AddScoped(typeof(ArmazenadorDeCargo));
            services.AddScoped(typeof(ExclusaoDeCargo));
            services.AddScoped(typeof(IValidadorDeCargoComFuncionarios), typeof(ValidadorDeCargoComFuncionarios));
            services.AddScoped(typeof(IValidadorDeCargoExistente), typeof(ValidadorDeCargoExistente));

            return services;
        }
    }
}
