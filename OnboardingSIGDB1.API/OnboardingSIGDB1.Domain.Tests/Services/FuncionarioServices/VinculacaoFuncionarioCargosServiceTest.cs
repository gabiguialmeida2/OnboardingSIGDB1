using FluentAssertions;
using Moq;
using OnboardingSIGDB1.Domain.Funcionarios;
using OnboardingSIGDB1.Domain.Funcionarios.Services;
using OnboardingSIGDB1.Domain.Funcionarios.Validators;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Notifications;
using OnboardingSIGDB1.Domain.Tests.EntityBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OnboardingSIGDB1.Domain.Tests.Services.FuncionarioServices
{
    public class VinculacaoFuncionarioCargosServiceTest
    {
        private readonly Mock<IFuncionarioRepository> _funcionarioRepository;
        private readonly NotificationContext _notificationContext;
        private readonly VinculadorDeFuncionarioCargo _vinculadorDeFuncionarioCargo;

        public VinculacaoFuncionarioCargosServiceTest()
        {

            _funcionarioRepository = new Mock<IFuncionarioRepository>();
            _notificationContext = new NotificationContext();
            var validadorDeFuncionarioComCargoExistente = new ValidadorDeFuncionarioComCargoExistente(_notificationContext);
            var validadorFuncionarioVinculadoAEmpresa = new ValidadorFuncionarioVinculadoAEmpresa(_notificationContext);

           _vinculadorDeFuncionarioCargo = new VinculadorDeFuncionarioCargo(_funcionarioRepository.Object,
                _notificationContext,
                validadorDeFuncionarioComCargoExistente,
                validadorFuncionarioVinculadoAEmpresa
                );
        }

        [Fact(DisplayName = "Vincular funcionario sem empresa")]
        public async Task Vicular_Funcionario_Sem_Empresa()
        {
            var funcionario = new FuncionarioBuilder().Build();

            _funcionarioRepository
            .Setup(c => c.GetWithIncludes(It.IsAny<Predicate<Funcionario>>()))
            .ReturnsAsync(new List<Funcionario>() { funcionario });

            await _vinculadorDeFuncionarioCargo.Vincular(funcionario.Id, 1, DateTime.Now.Date);

            Assert.True(_notificationContext.HasNotifications);
            _notificationContext.Notifications.Should().HaveCount(1);

            Assert.Contains(_notificationContext.Notifications,
               n => n.Key.Equals("FuncionarioSemEmpresa"));

            _funcionarioRepository.Verify(r => r.Update(funcionario), Times.Never);
        }

        [Fact(DisplayName = "Vincular funcionario a cargo duplicado")]
        public async Task Vicular_Funcionario_Cargo_Duplicado()
        {
            var builder = new FuncionarioBuilder()
                .WithId(1)
                .WithCargos(2)
                .WithEmpresa();

            var funcionario = builder.Build();

            _funcionarioRepository
           .Setup(c => c.GetWithIncludes(It.IsAny<Predicate<Funcionario>>()))
           .ReturnsAsync(new List<Funcionario>() { funcionario });

            await _vinculadorDeFuncionarioCargo.Vincular(funcionario.Id,
                funcionario.FuncionarioCargos.First().CargoId,
                DateTime.Now.Date);

            Assert.True(_notificationContext.HasNotifications);
            _notificationContext.Notifications.Should().HaveCount(1);

            Assert.Contains(_notificationContext.Notifications,
               n => n.Key.Equals("FuncionarioJaPossuiCargo"));

            _funcionarioRepository.Verify(r => r.Update(funcionario), Times.Never);
        }

        [Fact(DisplayName = "Vincular funcionario com sucesso")]
        public async Task Vicular_Funcionario_Com_Sucesso()
        {
            var builder = new FuncionarioBuilder()
               .WithId(1)
               .WithEmpresa();

            var funcionario = builder.Build();

            _funcionarioRepository
           .Setup(c => c.GetWithIncludes(It.IsAny<Predicate<Funcionario>>()))
           .ReturnsAsync(new List<Funcionario>() { funcionario });

            await _vinculadorDeFuncionarioCargo.Vincular(funcionario.Id,
                1,
                DateTime.Now.Date);

            Assert.False(_notificationContext.HasNotifications);

            _funcionarioRepository.Verify(r => r.Update(funcionario), Times.Once);
        }
    }
}
