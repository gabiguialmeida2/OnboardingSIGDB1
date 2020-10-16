using OnboardingSIGDB1.Domain.Empresas;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Interfaces
{
    public interface IEmpresaRepository : IRepository<Empresa>
    {
        Task<IEnumerable<Empresa>> GetWithFuncionarios(Predicate<Empresa> predicate);
    }
}
