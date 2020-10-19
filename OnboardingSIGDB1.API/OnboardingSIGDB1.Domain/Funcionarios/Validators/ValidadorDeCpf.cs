using OnboardingSIGDB1.Domain.Notifications;
using OnboardingSIGDB1.Domain.Utils;

namespace OnboardingSIGDB1.Domain.Funcionarios.Validators
{
    public class ValidadorDeCpf: IValidadorDeCpf
    {
        private readonly NotificationContext _notificationContext;

        public ValidadorDeCpf(NotificationContext notificationContext)
        {
            _notificationContext = notificationContext;
        }

        public void Valid(Funcionario funcionario)
        {
            if (!funcionario.Cpf.IsCpfValid())
            {
                _notificationContext.AddNotification(new Notification("CpfInvalido",
                    "Insira um CPF válido"));
            }
        }
    }
}
