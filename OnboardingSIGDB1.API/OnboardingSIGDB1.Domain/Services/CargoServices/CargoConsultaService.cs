using OnboardingSIGDB1.Domain.Dto.Filtros;
using OnboardingSIGDB1.Domain.Entitys;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Interfaces.Services;
using OnboardingSIGDB1.Domain.Notifications;
using OnboardingSIGDB1.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Services.CargoServices
{
    public class CargoConsultaService : CargoValidationService, ICargoConsultaService
    {
        public CargoConsultaService(IRepository<Cargo> cargoRepository,
              NotificationContext notificationContext)
                : base(cargoRepository, notificationContext)
        {
        }

        public async Task<IEnumerable<Cargo>> GetAll()
        {
            return await _cargoRepository.Get();
        }

        public async Task<Cargo> GetById(long id)
        {
            var cargos = await _cargoRepository.Get(cargo => cargo.Id == id);
            return cargos.FirstOrDefault();
        }

        public async Task<IEnumerable<Cargo>> GetFiltro(CargoFiltroDto filtro)
        {
            List<Predicate<Cargo>> filtros = new List<Predicate<Cargo>>();

            if (!string.IsNullOrEmpty(filtro.Descricao))
            {
                filtros.Add(cargo => cargo.Descricao.ToUpper().Contains(filtro.Descricao.ToUpper()));
            }

            var cargos = await _cargoRepository.Get(ExpressionConcatenation.And(filtros.ToArray()));
            return cargos;
        }
    }
}
