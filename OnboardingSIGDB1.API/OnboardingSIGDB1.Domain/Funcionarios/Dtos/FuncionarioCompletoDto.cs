using OnboardingSIGDB1.Domain.Empresas.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnboardingSIGDB1.Domain.Funcionarios.Dtos
{
    public class FuncionarioCompletoDto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public DateTime? DataContratacao { get; set; }
        public EmpresaDto Empresa { get; set; }
        public List<FuncionarioCargoDto> FuncionarioCargos { get; set; }
    }
}
