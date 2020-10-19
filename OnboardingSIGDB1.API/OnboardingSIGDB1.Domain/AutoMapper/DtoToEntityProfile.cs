using AutoMapper;
using OnboardingSIGDB1.Domain.Cargos;
using OnboardingSIGDB1.Domain.Cargos.Dtos;
using OnboardingSIGDB1.Domain.Empresas;
using OnboardingSIGDB1.Domain.Empresas.Dtos;
using OnboardingSIGDB1.Domain.Funcionarios;
using OnboardingSIGDB1.Domain.Funcionarios.Dtos;
using OnboardingSIGDB1.Domain.Utils;

namespace OnboardingSIGDB1.Domain.AutoMapper
{
    public class DtoToEntityProfile : Profile
    {
        public DtoToEntityProfile()
        {
            CreateMap<EmpresaDto, Empresa>()
                .ConvertUsing(c => new Empresa(c.Nome, c.Cnpj.RemoveMaskCnpj(), c.DataFundacao));

            CreateMap<FuncionarioDto, Funcionario>()
              .ConvertUsing(c => new Funcionario(c.Nome, c.Cpf.RemoveMaskCpf(), c.DataContratacao));
           
            CreateMap<FuncionarioCompletoDto, Funcionario>();

            CreateMap<CargoDto, Cargo>();

        }
    }
}
