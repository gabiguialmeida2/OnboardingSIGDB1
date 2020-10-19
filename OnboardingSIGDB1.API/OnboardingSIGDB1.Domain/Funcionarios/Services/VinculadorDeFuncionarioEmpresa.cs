using OnboardingSIGDB1.Domain.Funcionarios.Validators;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Notifications;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Funcionarios.Services
{
    public class VinculadorDeFuncionarioEmpresa
    {
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly NotificationContext _notificationContext;
        private readonly IValidadorFuncionarioPossuiAlgumaEmpresaVinculada _validadorFuncionarioPossuiAlgumaEmpresaVinculada;

        public VinculadorDeFuncionarioEmpresa(IFuncionarioRepository funcionarioRepository, 
            NotificationContext notificationContext,
            IValidadorFuncionarioPossuiAlgumaEmpresaVinculada validadorFuncionarioPossuiAlgumaEmpresaVinculada)
        {
            _funcionarioRepository = funcionarioRepository;
            _notificationContext = notificationContext;
            _validadorFuncionarioPossuiAlgumaEmpresaVinculada = validadorFuncionarioPossuiAlgumaEmpresaVinculada;
        }

        public async Task Vincular(long funcionarioId, long empresaId)
        {
            var funcionario = (
                await _funcionarioRepository
                .GetWithIncludes(f => f.Id == funcionarioId)
                ).FirstOrDefault();

            _validadorFuncionarioPossuiAlgumaEmpresaVinculada.Valid(funcionario);

            if (_notificationContext.HasNotifications)
            {
                return;
            }

            funcionario.AlterarEmpresaId(empresaId);
            await _funcionarioRepository.Update(funcionario);
        }
    }
}
