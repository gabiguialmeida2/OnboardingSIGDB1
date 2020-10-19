using OnboardingSIGDB1.Domain.Funcionarios.Validators;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Notifications;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Funcionarios.Services
{
    public class ExclusaoDeFuncionario
    {
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly NotificationContext _notificationContext;
        private readonly IValidadorDeFuncionarioExistente _validadorDeFuncionarioExistente;

        public ExclusaoDeFuncionario(IFuncionarioRepository funcionarioRepository, 
            NotificationContext notificationContext, 
            IValidadorDeFuncionarioExistente validadorDeFuncionarioExistente)
        {
            _funcionarioRepository = funcionarioRepository;
            _notificationContext = notificationContext;
            _validadorDeFuncionarioExistente = validadorDeFuncionarioExistente;
        }

        public async Task Excluir(long id)
        {
            var funcionarioDatabase = (await _funcionarioRepository
                .Get(emp => emp.Id == id))
                .FirstOrDefault();

            _validadorDeFuncionarioExistente.Valid(funcionarioDatabase);
            
            if (_notificationContext.HasNotifications)
                return;

            await _funcionarioRepository.Delete(id);

        }
    }
}
