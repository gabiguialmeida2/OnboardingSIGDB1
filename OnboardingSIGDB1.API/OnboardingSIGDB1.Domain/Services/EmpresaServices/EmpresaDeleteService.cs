using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Interfaces.Services;
using OnboardingSIGDB1.Domain.Notifications;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Services.EmpresaServices
{
    public class EmpresaDeleteService: EmpresaValidationsService, IEmpresaDeleteService
    {

        public EmpresaDeleteService(IEmpresaRepository empresaRepository, 
            NotificationContext notificationContext):base(empresaRepository, notificationContext)
        {
        }

        public async Task Delete(long id)
        {
            var empresaDatabase = (
                    await _empresaRepository
                        .GetWithFuncionarios(emp => emp.Id == id)
                ).FirstOrDefault();

            ValidExistEmpresa(empresaDatabase);
            ValidEmpresaPossuiFuncionarios(empresaDatabase);

            if (_notificationContext.HasNotifications)
                return;

            await _empresaRepository.Delete(id);

        }
    }
}
