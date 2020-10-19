using OnboardingSIGDB1.Domain.Funcionarios;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Interfaces
{
    public interface IFuncionarioRepository: IRepository<Funcionario>
    {
        Task<IEnumerable<Funcionario>> GetWithIncludes(Predicate<Funcionario> predicate);
    }
}
