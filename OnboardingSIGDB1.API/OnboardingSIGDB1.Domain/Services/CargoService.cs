using OnboardingSIGDB1.Domain.Dto.Filtros;
using OnboardingSIGDB1.Domain.Entitys;
using OnboardingSIGDB1.Domain.Entitys.Validators;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Interfaces.Services;
using OnboardingSIGDB1.Domain.Notifications;
using OnboardingSIGDB1.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Services
{
    public class CargoService: ICargoService
    {
        private readonly IRepository<Cargo> _cargoRepository;
        private readonly NotificationContext _notificationContext;

        public CargoService(IRepository<Cargo> cargoRepository,
            NotificationContext notificationContext)
        {
            _cargoRepository = cargoRepository;
            _notificationContext = notificationContext;
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

        public async Task InsertCargo(Cargo cargo)
        {
            if (!cargo.Valid)
            {
                _notificationContext.AddNotifications(cargo.ValidationResult);
                return;
            }

            await _cargoRepository.Add(cargo);
        }

        public async Task UpdateCargo(long id, Cargo cargo)
        {
            var cargoDatabase = (await _cargoRepository.Get(c => c.Id == id)).FirstOrDefault();

            ValidExistCargo(cargoDatabase);

            if (_notificationContext.HasNotifications)
                return;

            cargoDatabase.Descricao = cargo.Descricao;

            if (!cargoDatabase.Validate(cargoDatabase, new CargoValidator()))
            {
                _notificationContext.AddNotifications(cargo.ValidationResult);
                return;
            }

            await _cargoRepository.Update(cargo);

        }
        public async Task Delete(long id)
        {
            var cargoDatabase = await _cargoRepository.Get(cargo => cargo.Id == id);

            ValidExistCargo(cargoDatabase.FirstOrDefault());

            if (_notificationContext.HasNotifications)
                return;

            await _cargoRepository.Delete(id);

        }

        private void ValidExistCargo(Cargo cargoDatabase)
        {
            if (cargoDatabase == null)
            {
                _notificationContext.AddNotification(new Notification("CargoInexistente", "Não existe um cargo para o Id informado"));
            }
        }
    }
}
