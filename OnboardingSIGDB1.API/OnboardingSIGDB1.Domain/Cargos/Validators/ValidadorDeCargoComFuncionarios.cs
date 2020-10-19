using OnboardingSIGDB1.Domain.Notifications;
using System.Linq;

namespace OnboardingSIGDB1.Domain.Cargos.Validators
{
    public class ValidadorDeCargoComFuncionarios: IValidadorDeCargoComFuncionarios
    {
        private readonly NotificationContext _notificationContext;

        public ValidadorDeCargoComFuncionarios(NotificationContext notificationContext)
        {
            _notificationContext = notificationContext;
        }

        public void Valid(Cargo cargo)
        {
            if (cargo != null &&
                cargo.FuncionarioCargos != null &&
                cargo.FuncionarioCargos.Count() > 0)
            {
                _notificationContext.AddNotification(new Notification("CargoComFuncionariosVinculados",
                    "Não é possível deletar cargo com funcionários vinculados"));
            }
        }
    }
}
