using OnboardingSIGDB1.Domain.Entitys;
using OnboardingSIGDB1.Domain.Entitys.Validators;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Interfaces.Services;
using OnboardingSIGDB1.Domain.Notifications;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Services.CargoServices
{
    public class CargoService: CargoValidationService, ICargoService
    {
        public CargoService(IRepository<Cargo> cargoRepository,
            NotificationContext notificationContext)
            : base(cargoRepository, notificationContext)
        {
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

            await _cargoRepository.Update(cargoDatabase);
        }
        
    }
}
