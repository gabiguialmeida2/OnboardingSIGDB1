using OnboardingSIGDB1.Domain.Entitys;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Notifications;
using System.Linq;

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

        protected void ValidCargoComFuncionario(Cargo cargoDatabase)
        {
            if (cargoDatabase != null &&
                cargoDatabase.FuncionarioCargos != null &&
                cargoDatabase.FuncionarioCargos.Count() > 0)
            {
                _notificationContext.AddNotification(new Notification("CargoComFuncionariosVinculados",
                    "Não é possível deletar cargo com funcionários vinculados"));
            }
        }
    }
}
