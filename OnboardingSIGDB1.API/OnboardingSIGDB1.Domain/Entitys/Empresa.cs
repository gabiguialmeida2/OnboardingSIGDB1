using OnboardingSIGDB1.Domain.Entitys.Validators;
using System;

namespace OnboardingSIGDB1.Domain.Entitys
{
    public class Empresa : Entity
    {
        public Empresa()
        {

        }

        public Empresa(string nome, string cnpj, DateTime? dataFundacao) :
            this()
        {
            Nome = nome;
            Cnpj = cnpj;
            DataFundacao = dataFundacao;
            Validate(this, new EmpresaValidator());
        }

        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public DateTime? DataFundacao { get; set; }
    }
}
