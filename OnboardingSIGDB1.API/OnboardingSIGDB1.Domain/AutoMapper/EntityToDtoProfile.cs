using AutoMapper;
using OnboardingSIGDB1.Domain.Dto;
using OnboardingSIGDB1.Domain.Entitys;

namespace OnboardingSIGDB1.Domain.AutoMapper
{
    public class EntityToDtoProfile : Profile
    {
        public EntityToDtoProfile()
        {
            CreateMap<Empresa, EmpresaDto>();
            CreateMap<Funcionario, FuncionarioDto>();
            CreateMap<Cargo, CargoDto>();
            CreateMap<FuncionarioCargo, FuncionarioCargoDto>();
        }
    }
}

