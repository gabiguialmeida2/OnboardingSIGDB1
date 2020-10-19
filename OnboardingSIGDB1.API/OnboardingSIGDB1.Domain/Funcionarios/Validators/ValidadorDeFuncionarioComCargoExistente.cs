using OnboardingSIGDB1.Domain.Notifications;
using System.Linq;

namespace OnboardingSIGDB1.Domain.Funcionarios.Validators
{
    public class ValidadorDeFuncionarioComCargoExistente: IValidadorDeFuncionarioComCargoExistente
    {
        private readonly NotificationContext _notificationContext;

        public ValidadorDeFuncionarioComCargoExistente(NotificationContext notificationContext)
        {
            _notificationContext = notificationContext;
        }

        public void Valid(Funcionario funcionario, long cargoId)
        {
            if (funcionario.FuncionarioCargos != null &&
                funcionario.FuncionarioCargos.Any(f => f.CargoId == cargoId))
            {
                _notificationContext.AddNotification(new Notification("FuncionarioJaPossuiCargo",
                   "Funcionário já está vinculado a esse cargo"));
            }
        }
    }
}
