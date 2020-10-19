using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Notifications;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Funcionarios.Validators
{
    public class ValidadorDeFuncionarioDuplicado: IValidadorDeFuncionarioDuplicado
    {
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly NotificationContext _notificationContext;

        public ValidadorDeFuncionarioDuplicado(IFuncionarioRepository funcionarioRepository, 
            NotificationContext notificationContext)
        {
            _funcionarioRepository = funcionarioRepository;
            _notificationContext = notificationContext;
        }

        public async Task Valid(Funcionario funcionario)
        {
            var funcionarios = await _funcionarioRepository.Get(emp => emp.Cpf.Equals(funcionario.Cpf));

            if (funcionarios.Any())
            {
                _notificationContext.AddNotification(new Notification("CpfDuplicado",
                    "Já existe um funcionario cadastrado com esse CPF"));
            }
        }
    }
}
