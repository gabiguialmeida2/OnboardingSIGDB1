using Microsoft.Extensions.DependencyInjection;
using OnboardingSIGDB1.Data;
using OnboardingSIGDB1.Data.Repositorios;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain._Base;
using OnboardingSIGDB1.Data.Empresas.Repositorios;
using OnboardingSIGDB1.Domain.Empresas.Services;
using OnboardingSIGDB1.Domain.Empresas.Validators;
using OnboardingSIGDB1.Domain.Cargos.Services;
using OnboardingSIGDB1.Domain.Cargos.Validators;
using OnboardingSIGDB1.Domain.Funcionarios.Services;
using OnboardingSIGDB1.Domain.Funcionarios.Validators;

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

            services.AddScoped(typeof(ArmazenadorDeFuncionario));
            services.AddScoped(typeof(ExclusaoDeFuncionario));
            services.AddScoped(typeof(VinculadorDeFuncionarioCargo));
            services.AddScoped(typeof(VinculadorDeFuncionarioEmpresa));
            services.AddScoped(typeof(IValidadorDeCpf), typeof(ValidadorDeCpf));
            services.AddScoped(typeof(IValidadorDeFuncionarioComCargoExistente), typeof(ValidadorDeFuncionarioComCargoExistente));
            services.AddScoped(typeof(IValidadorDeFuncionarioDuplicado), typeof(ValidadorDeFuncionarioDuplicado));
            services.AddScoped(typeof(IValidadorDeFuncionarioExistente), typeof(ValidadorDeFuncionarioExistente));
            services.AddScoped(typeof(IValidadorFuncionarioPossuiAlgumaEmpresaVinculada), typeof(ValidadorFuncionarioPossuiAlgumaEmpresaVinculada));
            services.AddScoped(typeof(IValidadorFuncionarioVinculadoAEmpresa), typeof(ValidadorFuncionarioVinculadoAEmpresa));

            services.AddScoped(typeof(ArmazenadorDeCargo));
            services.AddScoped(typeof(ExclusaoDeCargo));
            services.AddScoped(typeof(IValidadorDeCargoComFuncionarios), typeof(ValidadorDeCargoComFuncionarios));
            services.AddScoped(typeof(IValidadorDeCargoExistente), typeof(ValidadorDeCargoExistente));

            return services;
        }
    }
}
