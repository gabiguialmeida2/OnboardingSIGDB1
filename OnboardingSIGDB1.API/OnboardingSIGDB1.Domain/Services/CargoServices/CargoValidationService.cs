using OnboardingSIGDB1.Domain.Entitys;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Notifications;

namespace OnboardingSIGDB1.Domain.Services.CargoServices
{
    public class CargoValidationService
    {
        protected readonly IRepository<Cargo> _cargoRepository;
        protected readonly NotificationContext _notificationContext;

        public CargoValidationService(IRepository<Cargo> cargoRepository, 
            NotificationContext notificationContext)
        {
            _cargoRepository = cargoRepository;
            _notificationContext = notificationContext;
        }

        protected void ValidExistCargo(Cargo cargoDatabase)
        {
            if (cargoDatabase == null)
            {
                _notificationContext.AddNotification(new Notification("CargoInexistente", 
                    "Não existe um cargo para o Id informado"));
            }
        }
    }
}
