using OnboardingSIGDB1.Domain.Funcionarios.Validators;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Funcionarios.Services
{
    public class VinculadorDeFuncionarioCargo
    {
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly NotificationContext _notificationContext;
        private readonly IValidadorDeFuncionarioComCargoExistente _validadorDeFuncionarioComCargoExistente;
        private readonly IValidadorFuncionarioVinculadoAEmpresa _validadorFuncionarioVinculadoAEmpresa;

        public VinculadorDeFuncionarioCargo(IFuncionarioRepository funcionarioRepository,
            NotificationContext notificationContext, 
            IValidadorDeFuncionarioComCargoExistente validadorDeFuncionarioComCargoExistente, 
            IValidadorFuncionarioVinculadoAEmpresa validadorFuncionarioVinculadoAEmpresa)
        {
            _funcionarioRepository = funcionarioRepository;
            _notificationContext = notificationContext;
            _validadorDeFuncionarioComCargoExistente = validadorDeFuncionarioComCargoExistente;
            _validadorFuncionarioVinculadoAEmpresa = validadorFuncionarioVinculadoAEmpresa;
        }

        public async Task Vincular(long funcionarioId, long cargoId, DateTime dataVinculacao)
        {
            var funcionario = (
                await _funcionarioRepository
                .GetWithIncludes(f => f.Id == funcionarioId)
                ).FirstOrDefault();

            _validadorFuncionarioVinculadoAEmpresa.Valid(funcionario);
            _validadorDeFuncionarioComCargoExistente.Valid(funcionario, cargoId);

            if (_notificationContext.HasNotifications)
            {
                return;
            }

            if (funcionario.FuncionarioCargos == null)
            {
                funcionario.AlterarFuncionarioCargos(new List<FuncionarioCargo>());
            }

            var funcionarioCargos = funcionario.FuncionarioCargos.ToList();
            funcionarioCargos.Add(new FuncionarioCargo(funcionarioId, cargoId, dataVinculacao));

            funcionario.AlterarFuncionarioCargos(funcionarioCargos);

            await _funcionarioRepository.Update(funcionario);
        }
    }
}
