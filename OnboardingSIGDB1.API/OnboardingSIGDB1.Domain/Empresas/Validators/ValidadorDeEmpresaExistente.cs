using OnboardingSIGDB1.Domain.Notifications;

namespace OnboardingSIGDB1.Domain.Empresas.Validators
{
    public class ValidadorDeEmpresaExistente: IValidadorDeEmpresaExistente
    {
        private readonly NotificationContext _notificationContext;

        public ValidadorDeEmpresaExistente(NotificationContext notificationContext)
        {
            _notificationContext = notificationContext;
        }

        public void Valid(Empresa empresa)
        {
            if (empresa == null)
            {
                _notificationContext.AddNotification(new Notification("EmpresaInexistente",
                    "Não existe uma empresa para o Id informado"));
            }
        }
    }
}
