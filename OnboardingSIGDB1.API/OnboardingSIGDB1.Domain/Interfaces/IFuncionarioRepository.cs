using OnboardingSIGDB1.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Interfaces
{
    public interface IFuncionarioRepository: IRepository<Funcionario>
    {
        Task<IEnumerable<Funcionario>> GetWithEmpresa(Predicate<Funcionario> predicate);
    }
}
