using OnboardingSIGDB1.Domain.Notifications;
using OnboardingSIGDB1.Domain.Utils;

namespace OnboardingSIGDB1.Domain.Empresas.Validators
{
    public class ValidadorDeCnpj : IValidadorDeCnpj
    {
        protected readonly NotificationContext _notificationContext;

        public ValidadorDeCnpj(NotificationContext notificationContext)
        {
            _notificationContext = notificationContext;
        }

        public void Valid(Empresa empresa)
        {
            if (!empresa.Cnpj.IsCnpjValid())
            {
                _notificationContext.AddNotification(new Notification("CnpjInvalido",
                    "Insira um CNPJ válido"));
            }
        }
    }
}
