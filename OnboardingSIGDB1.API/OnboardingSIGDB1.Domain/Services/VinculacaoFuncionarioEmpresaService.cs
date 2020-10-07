using OnboardingSIGDB1.Domain.Entitys;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Interfaces.Services;
using OnboardingSIGDB1.Domain.Notifications;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Services
{
    public class VinculacaoFuncionarioEmpresaService: IVinculacaoFuncionarioEmpresaService
    {
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly NotificationContext _notificationContext;

        public VinculacaoFuncionarioEmpresaService(IFuncionarioRepository funcionarioRepository, 
            NotificationContext notificationContext)
        {
            _funcionarioRepository = funcionarioRepository;
            _notificationContext = notificationContext;
        }

        public async Task Vincular(long funcionarioId, long empresaId)
        {
            var funcionario = (
                await _funcionarioRepository
                .GetWithIncludes(f => f.Id == funcionarioId)
                ).FirstOrDefault();

            ValidFuncionarioPossuiEmpresa(funcionario);

            if (_notificationContext.HasNotifications)
            {
                return;
            }

            funcionario.EmpresaId = empresaId;
           await _funcionarioRepository.Update(funcionario);
        }

        private void ValidFuncionarioPossuiEmpresa(Funcionario funcionario)
        {
            if (funcionario.EmpresaId.HasValue)
            {
                _notificationContext.AddNotification(new Notification("FuncionarioJaPossuiEmpresaVinculada", 
                    "Não é possível alterar a vinculação de um funcionário à uma empresa"));
            }
        }
    }
}
