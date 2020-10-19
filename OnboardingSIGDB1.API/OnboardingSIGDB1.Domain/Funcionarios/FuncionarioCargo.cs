using OnboardingSIGDB1.Domain.Cargos;
using System;

namespace OnboardingSIGDB1.Domain.Funcionarios
{
    public class FuncionarioCargo
    {
        public FuncionarioCargo(long funcionarioId, long cargoId, DateTime dataVinculacao)
        {
            FuncionarioId = funcionarioId;
            CargoId = cargoId;
            DataVinculacao = dataVinculacao;
        }

        public long FuncionarioId { get; set; }
        public Funcionario Funcionario { get; set; }
        public long CargoId { get; set; }
        public Cargo Cargo { get; set; }
        public DateTime DataVinculacao { get; set; }
    }
}
