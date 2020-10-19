using AutoMapper;
using OnboardingSIGDB1.Domain.Cargos;
using OnboardingSIGDB1.Domain.Cargos.Dtos;
using OnboardingSIGDB1.Domain.Empresas;
using OnboardingSIGDB1.Domain.Empresas.Dtos;
using OnboardingSIGDB1.Domain.Funcionarios;
using OnboardingSIGDB1.Domain.Funcionarios.Dtos;

namespace OnboardingSIGDB1.Domain.AutoMapper
{
    public class EntityToDtoProfile : Profile
    {
        public EntityToDtoProfile()
        {
            CreateMap<Empresa, EmpresaDto>();
            CreateMap<Funcionario, FuncionarioDto>();
            CreateMap<Funcionario, FuncionarioCompletoDto>();
            CreateMap<Cargo, CargoDto>();
            CreateMap<FuncionarioCargo, FuncionarioCargoDto>();
        }
    }
}

