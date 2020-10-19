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
using System.Threading.Tasks;
using Xunit;

namespace OnboardingSIGDB1.Domain.Tests.Services.FuncionarioServices
{
    public class VinculacaoFuncionarioEmpresaServiceTest
    {
        private readonly Mock<IFuncionarioRepository> _funcionarioRepository;
        private readonly NotificationContext _notificationContext;

        private readonly VinculadorDeFuncionarioEmpresa _vinculadorDeFuncionarioEmpresa;

        public VinculacaoFuncionarioEmpresaServiceTest()
        {

            _funcionarioRepository = new Mock<IFuncionarioRepository>();
            _notificationContext = new NotificationContext();
            var validadorFuncionarioPossuiAlgumaEmpresaVinculada = new ValidadorFuncionarioPossuiAlgumaEmpresaVinculada(_notificationContext);

            _vinculadorDeFuncionarioEmpresa = new VinculadorDeFuncionarioEmpresa(_funcionarioRepository.Object,
                _notificationContext,
                validadorFuncionarioPossuiAlgumaEmpresaVinculada
                );
        }

        [Fact(DisplayName = "Vincular empresa a funcionário que já possui empresa vinculada")]
        public async Task Vincular_Funcionario_Com_Empresa()
        {
            var builder = new FuncionarioBuilder()
               .WithId(1)
               .WithEmpresa();

            var funcionario = builder.Build();

            _funcionarioRepository
           .Setup(c => c.GetWithIncludes(It.IsAny<Predicate<Funcionario>>()))
           .ReturnsAsync(new List<Funcionario>() { funcionario });

            await _vinculadorDeFuncionarioEmpresa.Vincular(funcionario.Id, 1);

            Assert.True(_notificationContext.HasNotifications);
            _notificationContext.Notifications.Should().HaveCount(1);

            Assert.Contains(_notificationContext.Notifications,
               n => n.Key.Equals("FuncionarioJaPossuiEmpresaVinculada"));

            _funcionarioRepository.Verify(r => r.Update(funcionario), Times.Never);
        }

        [Fact(DisplayName = "Vincular empresa a funcionário com sucesso")]
        public async Task Vincular_Funcionario_Com_Sucesso()
        {
            var builder = new FuncionarioBuilder()
               .WithId(1);

            var funcionario = builder.Build();

            _funcionarioRepository
           .Setup(c => c.GetWithIncludes(It.IsAny<Predicate<Funcionario>>()))
           .ReturnsAsync(new List<Funcionario>() { funcionario });

            await _vinculadorDeFuncionarioEmpresa.Vincular(funcionario.Id, 1);

            Assert.False(_notificationContext.HasNotifications);
            _funcionarioRepository.Verify(r => r.Update(funcionario), Times.Once);
        }
    }
}
