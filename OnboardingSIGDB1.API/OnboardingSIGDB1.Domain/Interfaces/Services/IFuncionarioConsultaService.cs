using OnboardingSIGDB1.Domain.Funcionarios;
using OnboardingSIGDB1.Domain.Funcionarios.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Interfaces.Services
{
    public interface IFuncionarioConsultaService
    {
        Task<IEnumerable<Funcionario>> GetAll();
        Task<Funcionario> GetById(long id);
        Task<IEnumerable<Funcionario>> GetFiltro(FuncionarioFiltroDto filtro);
    }
}
