using OnboardingSIGDB1.Domain.Dto.Filtros;
using OnboardingSIGDB1.Domain.Entitys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Interfaces.Services
{
    public interface ICargoConsultaService
    {
        Task<IEnumerable<Cargo>> GetAll();
        Task<Cargo> GetById(long id);
        Task<IEnumerable<Cargo>> GetFiltro(CargoFiltroDto filtro);
    }
}
