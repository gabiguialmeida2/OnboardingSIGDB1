﻿using System;

namespace OnboardingSIGDB1.Domain.Entitys
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