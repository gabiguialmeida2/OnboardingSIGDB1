using OnboardingSIGDB1.Domain.Funcionarios.Dtos;
using OnboardingSIGDB1.Domain.Funcionarios.Validators;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Notifications;
using OnboardingSIGDB1.Domain.Utils;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Funcionarios.Services
{
    public class ArmazenadorDeFuncionario
    {
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly NotificationContext _notificationContext;
        private readonly IValidadorDeCpf _validadorDeCpf;
        private readonly IValidadorDeFuncionarioExistente _validadorDeFuncionarioExistente;
        private readonly IValidadorDeFuncionarioDuplicado _validadorDeFuncionarioDuplicado;

        public ArmazenadorDeFuncionario(IFuncionarioRepository funcionarioRepository, 
            NotificationContext notificationContext, 
            IValidadorDeCpf validadorDeCpf, 
            IValidadorDeFuncionarioExistente validadorDeFuncionarioExistente, 
            IValidadorDeFuncionarioDuplicado validadorDeFuncionarioDuplicado)
        {
            _funcionarioRepository = funcionarioRepository;
            _notificationContext = notificationContext;
            _validadorDeCpf = validadorDeCpf;
            _validadorDeFuncionarioExistente = validadorDeFuncionarioExistente;
            _validadorDeFuncionarioDuplicado = validadorDeFuncionarioDuplicado;
        }

        public async Task Armazenar(FuncionarioDto funcionarioDto)
        {
            if (funcionarioDto.Id == 0)
                await NovoFuncionario(funcionarioDto);
            else
                await EditarFuncionario(funcionarioDto);
        }

        public async Task NovoFuncionario(FuncionarioDto funcionarioDto)
        {
            var funcionario = new Funcionario(funcionarioDto.Nome, 
                funcionarioDto.Cpf.RemoveMaskCpf(),
                funcionarioDto.DataContratacao);

            if (!funcionario.Valid)
            {
                _notificationContext.AddNotifications(funcionario.ValidationResult);
                return;
            }

            _validadorDeCpf.Valid(funcionario);
            await _validadorDeFuncionarioDuplicado.Valid(funcionario);

            if (_notificationContext.HasNotifications)
                return;

            await _funcionarioRepository.Add(funcionario);

        }

        public async Task EditarFuncionario(FuncionarioDto funcionarioDto)
        {
            var funcionarioDatabase = (await _funcionarioRepository
                .Get(emp => emp.Id == funcionarioDto.Id))
                .FirstOrDefault();

            _validadorDeFuncionarioExistente.Valid(funcionarioDatabase);

            if (_notificationContext.HasNotifications)
                return;

            funcionarioDatabase.AlterarNome(funcionarioDto.Nome);
            funcionarioDatabase.AlterarDataContratacao(funcionarioDto.DataContratacao);

            if (!funcionarioDatabase.Validate(funcionarioDatabase, new FuncionarioValidator()))
            {
                _notificationContext.AddNotifications(funcionarioDatabase.ValidationResult);
                return;
            }

            await _funcionarioRepository.Update(funcionarioDatabase);

        }
    }
}
