using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Notifications;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Empresas.Validators
{
    public class ValidadorDeEmpresaDuplicada: IValidadorDeEmpresaDuplicada
    {
        private readonly IEmpresaRepository _empresaRepository;
        private readonly NotificationContext _notificationContext;

        public ValidadorDeEmpresaDuplicada(IEmpresaRepository empresaRepository, 
            NotificationContext notificationContext)
        {
            _empresaRepository = empresaRepository;
            _notificationContext = notificationContext;
        }

        public async Task Valid(Empresa empresa)
        {
            var empresas = await _empresaRepository.Get(emp => emp.Cnpj.Equals(empresa.Cnpj));

            if (empresas.Any())
            {
                _notificationContext.AddNotification(new Notification("CnpjDuplicado",
                    "Já existe uma empresa cadastrada com esse CNPJ"));
            }
        }
    }
}
