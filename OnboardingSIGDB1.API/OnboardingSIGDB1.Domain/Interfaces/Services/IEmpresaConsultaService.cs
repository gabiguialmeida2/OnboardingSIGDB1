﻿using OnboardingSIGDB1.Domain.Dto.Filtros;
using OnboardingSIGDB1.Domain.Entitys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Interfaces.Services
{
    public interface IEmpresaConsultaService
    {
        Task<IEnumerable<Empresa>> GetAll();
        Task<Empresa> GetById(long id);
        Task<IEnumerable<Empresa>> GetFiltro(EmpresaFiltroDto filtro);
    }
}