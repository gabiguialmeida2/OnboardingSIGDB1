using System;
using System.Collections.Generic;
using System.Text;

namespace OnboardingSIGDB1.Domain.Funcionarios.Dtos
{
    public class FuncionarioFiltroDto
    {
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public DateTime? DataContratacaoInicio { get; set; }
        public DateTime? DataContratacaoFim { get; set; }
    }
}
