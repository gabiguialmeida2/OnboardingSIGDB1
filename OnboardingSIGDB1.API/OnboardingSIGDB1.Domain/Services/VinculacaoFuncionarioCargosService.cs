using OnboardingSIGDB1.Domain.Entitys;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Interfaces.Services;
using OnboardingSIGDB1.Domain.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Services
{
    public class VinculacaoFuncionarioCargosService: IVinculacaoFuncionarioCargosService
    {
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly NotificationContext _notificationContext;

        public VinculacaoFuncionarioCargosService(IFuncionarioRepository funcionarioRepository,
            NotificationContext notificationContext)
        {
            _funcionarioRepository = funcionarioRepository;
            _notificationContext = notificationContext;
        }

        public async Task Vincular(long funcionarioId, long cargoId, DateTime dataVinculacao)
        {
            var funcionario = (
                await _funcionarioRepository
                .GetWithIncludes(f => f.Id == funcionarioId)
                ).FirstOrDefault();

            ValidFuncionarioPossuiEmpresa(funcionario);
            ValidFuncionarioJaPossuiCargo(funcionario, cargoId);

            if (_notificationContext.HasNotifications)
            {
                return;
            }

            if (funcionario.FuncionarioCargos == null)
            {
                funcionario.FuncionarioCargos = new List<FuncionarioCargo>();
            }

            var funcionarioCargos = funcionario.FuncionarioCargos.ToList();
            funcionarioCargos.Add(new FuncionarioCargo(funcionarioId, cargoId, dataVinculacao));

            funcionario.FuncionarioCargos = funcionarioCargos;

            await _funcionarioRepository.Update(funcionario);
        }

        private void ValidFuncionarioJaPossuiCargo(Funcionario funcionario, long cargoId)
        {
            if (funcionario.FuncionarioCargos != null && 
                funcionario.FuncionarioCargos.Any(f => f.CargoId == cargoId))
            {
                _notificationContext.AddNotification(new Notification("FuncionarioJaPossuiCargo",
                   "Funcionário já está vinculado a esse cargo"));
            }
        }

        private void ValidFuncionarioPossuiEmpresa(Funcionario funcionario)
        {
            if (!funcionario.EmpresaId.HasValue)
            {
                _notificationContext.AddNotification(new Notification("FuncionarioSemEmpresa",
                    "Não é possível vincular funcionário a um cargo, sem que ele esteja vinculado a uma empresa"));
            }
        }
    }
}
