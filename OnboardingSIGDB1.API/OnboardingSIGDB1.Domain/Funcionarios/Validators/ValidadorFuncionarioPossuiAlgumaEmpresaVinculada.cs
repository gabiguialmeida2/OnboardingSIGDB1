using OnboardingSIGDB1.Domain.Notifications;

namespace OnboardingSIGDB1.Domain.Funcionarios.Validators
{
    public class ValidadorFuncionarioPossuiAlgumaEmpresaVinculada: IValidadorFuncionarioPossuiAlgumaEmpresaVinculada
    {
        private readonly NotificationContext _notificationContext;

        public ValidadorFuncionarioPossuiAlgumaEmpresaVinculada(NotificationContext notificationContext)
        {
            _notificationContext = notificationContext;
        }

        public void Valid(Funcionario funcionario)
        {
            if (funcionario.EmpresaId.HasValue)
            {
                _notificationContext.AddNotification(new Notification("FuncionarioJaPossuiEmpresaVinculada",
                    "Não é possível alterar a vinculação de um funcionário à uma empresa"));
            }
        }
    }
}
