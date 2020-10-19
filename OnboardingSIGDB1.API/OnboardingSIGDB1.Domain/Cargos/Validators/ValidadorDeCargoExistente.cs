using OnboardingSIGDB1.Domain.Notifications;

namespace OnboardingSIGDB1.Domain.Cargos.Validators
{
    public class ValidadorDeCargoExistente: IValidadorDeCargoExistente
    {
        private readonly NotificationContext _notificationContext;

        public ValidadorDeCargoExistente(NotificationContext notificationContext)
        {
            _notificationContext = notificationContext;
        }

        public void Valid(Cargo cargo)
        {
            if (cargo == null)
            {
                _notificationContext.AddNotification(new Notification("CargoInexistente",
                    "Não existe um cargo para o Id informado"));
            }
        }
    }
}
