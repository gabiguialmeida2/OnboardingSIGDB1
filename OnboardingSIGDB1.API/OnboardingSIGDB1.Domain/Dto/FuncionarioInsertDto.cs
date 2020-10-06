using System;

namespace OnboardingSIGDB1.Domain.Dto
{
    public class FuncionarioInsertDto
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public DateTime? DataContratacao { get; set; }
    }
}
