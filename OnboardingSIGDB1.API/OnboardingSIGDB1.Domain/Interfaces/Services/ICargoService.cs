using OnboardingSIGDB1.Domain.Dto.Filtros;
using OnboardingSIGDB1.Domain.Entitys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Interfaces.Services
{
    public interface ICargoService
    {
        Task InsertCargo(Cargo cargo);
        Task UpdateCargo(long id, Cargo cargo);
        Task Delete(long id);
        Task<IEnumerable<Cargo>> GetAll();
        Task<Cargo> GetById(long id);
        Task<IEnumerable<Cargo>> GetFiltro(CargoFiltroDto filtro);
    }
}
