using OnboardingSIGDB1.Domain.Notifications;

namespace OnboardingSIGDB1.Domain.Funcionarios.Validators
{
    public class ValidadorDeFuncionarioExistente: IValidadorDeFuncionarioExistente
    {
        private readonly NotificationContext _notificationContext;

        public ValidadorDeFuncionarioExistente(NotificationContext notificationContext)
        {
            _notificationContext = notificationContext;
        }

        public void Valid(Funcionario funcionario)
        {
            if (funcionario == null)
            {
                _notificationContext.AddNotification(new Notification("FuncionarioInexistente",
                    "Não existe um funcionario para o Id informado"));
            }
        }
    }
}
