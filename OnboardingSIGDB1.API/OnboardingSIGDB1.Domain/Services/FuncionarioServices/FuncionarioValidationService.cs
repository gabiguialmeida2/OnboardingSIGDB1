using OnboardingSIGDB1.Domain.Entitys;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Notifications;
using OnboardingSIGDB1.Domain.Utils;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Services.FuncionarioServices
{
    public class FuncionarioValidationService
    {
        protected readonly IFuncionarioRepository _funcionarioRepository;
        protected readonly NotificationContext _notificationContext;

        public FuncionarioValidationService(IFuncionarioRepository funcionarioRepository,
            NotificationContext notificationContext)
        {
            _funcionarioRepository = funcionarioRepository;
            _notificationContext = notificationContext;
        }
        protected async Task ValidDuplication(Funcionario funcionario)
        {
            var funcionarios = await _funcionarioRepository.Get(emp => emp.Cpf.Equals(funcionario.Cpf));

            if (funcionarios.Any())
            {
                _notificationContext.AddNotification(new Notification("CpfDuplicado", 
                    "Já existe um funcionario cadastrado com esse CPF"));
            }
        }

        protected void ValidCpf(Funcionario funcionario)
        {
            if (!funcionario.Cpf.IsCpfValid())
            {
                _notificationContext.AddNotification(new Notification("CpfInvalido", 
                    "Insira um CPF válido"));
            }
        }

        protected void ValidExistFuncionario(Funcionario funcionarioDatabase)
        {
            if (funcionarioDatabase == null)
            {
                _notificationContext.AddNotification(new Notification("FuncionarioInexistente", 
                    "Não existe um funcionario para o Id informado"));
            }
        }
    }
}
