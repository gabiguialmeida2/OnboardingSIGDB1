using OnboardingSIGDB1.Domain.Dto.Filtros;
using OnboardingSIGDB1.Domain.Entitys;
using OnboardingSIGDB1.Domain.Entitys.Validators;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Interfaces.Services;
using OnboardingSIGDB1.Domain.Notifications;
using OnboardingSIGDB1.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Services
{
    public class FuncionarioService: IFuncionarioService
    {
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly NotificationContext _notificationContext;

        public FuncionarioService(IFuncionarioRepository funcionarioRepository,
            NotificationContext notificationContext)
        {
            _funcionarioRepository = funcionarioRepository;
            _notificationContext = notificationContext;
        }
        public async Task<IEnumerable<Funcionario>> GetAll()
        {
            return await _funcionarioRepository.GetWithIncludes(f => true);
        }

        public async Task<Funcionario> GetById(long id)
        {
            var funcionarios = await _funcionarioRepository.GetWithIncludes(emp => emp.Id == id);
            return funcionarios.FirstOrDefault();
        }

        public async Task<IEnumerable<Funcionario>> GetFiltro(FuncionarioFiltroDto filtro)
        {
            List<Predicate<Funcionario>> filtros = new List<Predicate<Funcionario>>();

            if (!string.IsNullOrEmpty(filtro.Nome))
            {
                filtros.Add(emp => emp.Nome.ToUpper().Contains(filtro.Nome.ToUpper()));
            }

            if (!string.IsNullOrEmpty(filtro.Cpf))
            {
                filtros.Add(emp => emp.Cpf.Equals(filtro.Cpf));
            }

            if (filtro.DataContratacaoInicio.HasValue)
            {
                filtros.Add(emp => emp.DataContratacao.HasValue
                    && emp.DataContratacao.Value.Date >= filtro.DataContratacaoInicio.Value.Date);
            }

            if (filtro.DataContratacaoFim.HasValue)
            {
                filtros.Add(emp => emp.DataContratacao.HasValue
                    && emp.DataContratacao.Value.Date <= filtro.DataContratacaoFim.Value.Date);
            }

            var funcionarios = await _funcionarioRepository.GetWithIncludes(ExpressionConcatenation.And(filtros.ToArray()));
            return funcionarios;
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
        public async Task Delete(long id)
        {
            var funcionarioDatabase = await _funcionarioRepository.Get(emp => emp.Id == id);

            ValidExistFuncionario(funcionarioDatabase.FirstOrDefault());

            if (_notificationContext.HasNotifications)
                return;

            await _funcionarioRepository.Delete(id);

        }

        private async Task ValidDuplication(Funcionario funcionario)
        {
            var funcionarios = await _funcionarioRepository.Get(emp => emp.Cpf.Equals(funcionario.Cpf));

            if (funcionarios.Any())
            {
                _notificationContext.AddNotification(new Notification("CnpjDuplicado", "Já existe um funcionario cadastrado com esse CPF"));
            }
        }

        private void ValidCpf(Funcionario funcionario)
        {
            if (!funcionario.Cpf.IsCpfValid())
            {
                _notificationContext.AddNotification(new Notification("CpfInvalido", "Insira um CPF válido"));
            }
        }

        private void ValidExistFuncionario(Funcionario funcionarioDatabase)
        {
            if (funcionarioDatabase == null)
            {
                _notificationContext.AddNotification(new Notification("FuncionarioInexistente", "Não existe um funcionario para o Id informado"));
            }
        }

    }
}
