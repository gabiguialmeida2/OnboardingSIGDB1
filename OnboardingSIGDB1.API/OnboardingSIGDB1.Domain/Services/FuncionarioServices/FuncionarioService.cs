using OnboardingSIGDB1.Domain.Entitys;
using OnboardingSIGDB1.Domain.Entitys.Validators;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Interfaces.Services;
using OnboardingSIGDB1.Domain.Notifications;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Services.FuncionarioServices
{
    public class FuncionarioService : FuncionarioValidationService, IFuncionarioService
    {
        public FuncionarioService(IFuncionarioRepository funcionarioRepository,
            NotificationContext notificationContext)
            : base(funcionarioRepository, notificationContext)

        {
        }


        public async Task InsertFuncionario(Funcionario funcionario)
        {
            if (!funcionario.Valid)
            {
                _notificationContext.AddNotifications(funcionario.ValidationResult);
                return;
            }

            ValidCpf(funcionario);
            await ValidDuplication(funcionario);

            if (_notificationContext.HasNotifications)
                return;

            await _funcionarioRepository.Add(funcionario);

        }

        public async Task UpdateFuncionario(long id, Funcionario funcionario)
        {
            var funcionarioDatabase = (await _funcionarioRepository.Get(emp => emp.Id == id)).FirstOrDefault();

            ValidExistFuncionario(funcionarioDatabase);

            if (_notificationContext.HasNotifications)
                return;

            funcionarioDatabase.Nome = funcionario.Nome;
            funcionarioDatabase.DataContratacao = funcionario.DataContratacao;

            if (!funcionarioDatabase.Validate(funcionarioDatabase, new FuncionarioValidator()))
            {
                _notificationContext.AddNotifications(funcionario.ValidationResult);
                return;
            }

            await _funcionarioRepository.Update(funcionario);

        }
    }
}
