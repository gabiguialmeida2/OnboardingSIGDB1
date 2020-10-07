using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Interfaces.Services;
using OnboardingSIGDB1.Domain.Notifications;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Services.FuncionarioServices
{
    public class FuncionarioDeleteService : FuncionarioValidationService, IFuncionarioDeleteService
    {
        public FuncionarioDeleteService(IFuncionarioRepository funcionarioRepository,
              NotificationContext notificationContext)
            : base(funcionarioRepository, notificationContext)
        {
        }

        public async Task Delete(long id)
        {
            var funcionarioDatabase = await _funcionarioRepository.Get(emp => emp.Id == id);

            ValidExistFuncionario(funcionarioDatabase.FirstOrDefault());

            if (_notificationContext.HasNotifications)
                return;

            await _funcionarioRepository.Delete(id);

        }
    }
}
