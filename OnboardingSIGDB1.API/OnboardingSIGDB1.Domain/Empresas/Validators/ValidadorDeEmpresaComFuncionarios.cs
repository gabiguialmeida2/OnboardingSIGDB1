using OnboardingSIGDB1.Domain.Notifications;
using System.Linq;

namespace OnboardingSIGDB1.Domain.Empresas.Validators
{
    public class ValidadorDeEmpresaComFuncionarios: IValidadorDeEmpresaComFuncionarios
    {
        private readonly NotificationContext _notificationContext;

        public ValidadorDeEmpresaComFuncionarios(NotificationContext notificationContext)
        {
            _notificationContext = notificationContext;
        }

        public void Valid(Empresa empresa)
        {
            if (empresa != null &&
                 empresa.Funcionarios != null &&
                 empresa.Funcionarios.Any())
            {
                _notificationContext.AddNotification(new Notification("EmpresaComFuncionarios",
                   "Não é possível excluir empresa com funcionários vinculados"));
            }
        }
    }
}
