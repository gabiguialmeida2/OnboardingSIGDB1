using FluentAssertions;
using Moq;
using OnboardingSIGDB1.Domain.Entitys;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Interfaces.Services;
using OnboardingSIGDB1.Domain.Notifications;
using OnboardingSIGDB1.Domain.Services.FuncionarioServices;
using OnboardingSIGDB1.Domain.Tests.EntityBuilders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OnboardingSIGDB1.Domain.Tests.Services.FuncionarioServices
{
    public class VinculacaoFuncionarioEmpresaServiceTest
    {
        private readonly Mock<IFuncionarioRepository> _funcionarioRepository;
        private readonly NotificationContext _notificationContext;

        private readonly IVinculacaoFuncionarioEmpresaService _vinculacaoFuncionarioEmpresaService;

        public VinculacaoFuncionarioEmpresaServiceTest()
        {

            _funcionarioRepository = new Mock<IFuncionarioRepository>();
            _notificationContext = new NotificationContext();

            _vinculacaoFuncionarioEmpresaService = new VinculacaoFuncionarioEmpresaService(_funcionarioRepository.Object,
                _notificationContext);
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

            await _vinculacaoFuncionarioEmpresaService.Vincular(funcionario.Id, 1);

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

            await _vinculacaoFuncionarioEmpresaService.Vincular(funcionario.Id, 1);

            Assert.False(_notificationContext.HasNotifications);
            _funcionarioRepository.Verify(r => r.Update(funcionario), Times.Once);
        }
    }
}
