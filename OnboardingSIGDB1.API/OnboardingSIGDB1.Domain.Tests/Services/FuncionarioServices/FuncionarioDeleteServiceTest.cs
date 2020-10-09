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
using System.Threading.Tasks;
using Xunit;

namespace OnboardingSIGDB1.Domain.Tests.Services.FuncionarioServices
{
    public class FuncionarioDeleteServiceTest
    {
        private readonly Mock<IFuncionarioRepository> _funcionarioRepository;
        private readonly NotificationContext _notificationContext;

        private readonly IFuncionarioDeleteService _funcionarioDeleteService;

        public FuncionarioDeleteServiceTest()
        {

            _funcionarioRepository = new Mock<IFuncionarioRepository>();
            _notificationContext = new NotificationContext();

            _funcionarioDeleteService = new FuncionarioDeleteService(_funcionarioRepository.Object,
                _notificationContext);
        }

        [Fact(DisplayName = "Deletar funcionario inexistente")]
        public async Task Deletar_Funcionario_Inexistente()
        {
            var builder = new FuncionarioBuilder()
                .WithId(1);

            var funcionario = builder.Build();

            _funcionarioRepository
             .Setup(c => c.Get(It.IsAny<Predicate<Funcionario>>()))
             .ReturnsAsync(new List<Funcionario>() { });

            await _funcionarioDeleteService.Delete(1);

            Assert.True(_notificationContext.HasNotifications);
            _notificationContext.Notifications.Should().HaveCount(1);

            Assert.Contains(_notificationContext.Notifications,
               n => n.Key.Equals("FuncionarioInexistente"));

            _funcionarioRepository.Verify(r => r.Delete(1), Times.Never);
        }

        
        [Fact(DisplayName = "Deletar funcionario com sucesso")]
        public async Task Deletar_Funcionario_Sucesso()
        {
            var builder = new FuncionarioBuilder()
                .WithId(1);

            var funcionario = builder.Build();

            _funcionarioRepository
                 .Setup(c => c.Get(It.IsAny<Predicate<Funcionario>>()))
                 .ReturnsAsync(new List<Funcionario>() { funcionario });

            await _funcionarioDeleteService.Delete(1);

            Assert.False(_notificationContext.HasNotifications);

            _funcionarioRepository.Verify(r => r.Delete(1), Times.Once);
        }
    }
}
