using OnboardingSIGDB1.Domain.Notifications;

namespace OnboardingSIGDB1.Domain.Funcionarios.Validators
{
    public class ValidadorFuncionarioVinculadoAEmpresa: IValidadorFuncionarioVinculadoAEmpresa
    {
        private readonly NotificationContext _notificationContext;

        public ValidadorFuncionarioVinculadoAEmpresa(NotificationContext notificationContext)
        {
            _notificationContext = notificationContext;
        }

        public void Valid(Funcionario funcionario)
        {
            if (!funcionario.EmpresaId.HasValue)
            {
                _notificationContext.AddNotification(new Notification("FuncionarioSemEmpresa",
                    "Não é possível vincular funcionário a um cargo, sem que ele esteja vinculado a uma empresa"));
            }
        }
    }
}
