﻿using OnboardingSIGDB1.Domain.Cargos.Dtos;
using System;

namespace OnboardingSIGDB1.Domain.Dto
{
    public class FuncionarioCargoDto
    {
        public long FuncionarioId { get; set; }
        public CargoDto Cargo { get; set; }
        public DateTime DataVinculacao { get; set; }
    }
}
