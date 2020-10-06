using OnboardingSIGDB1.Domain.Dto.Filtros;
using OnboardingSIGDB1.Domain.Entitys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Interfaces.Services
{
    public interface IFuncionarioService
    {
        Task InsertFuncionario(Funcionario funcionario);
        Task UpdateFuncionario(long id, Funcionario funcionario);
        Task Delete(long id);
        Task<IEnumerable<Funcionario>> GetAll();
        Task<Funcionario> GetById(long id);
        Task<IEnumerable<Funcionario>> GetFiltro(FuncionarioFiltroDto filtro);
    }
}
