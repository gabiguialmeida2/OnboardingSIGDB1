using System;

namespace OnboardingSIGDB1.Domain.Funcionarios.Dtos
{
    public class FuncionarioDto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public DateTime? DataContratacao { get; set; }
    }
}
