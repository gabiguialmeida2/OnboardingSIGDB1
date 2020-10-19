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
    public class FuncionarioDeleteServiceTest
    {
        private readonly Mock<IFuncionarioRepository> _funcionarioRepository;
        private readonly NotificationContext _notificationContext;
        private readonly ExclusaoDeFuncionario _exclusaoDeFuncionario;

        public FuncionarioDeleteServiceTest()
        {

            _funcionarioRepository = new Mock<IFuncionarioRepository>();
            _notificationContext = new NotificationContext();
            var validadorDeFuncionarioExistente = new ValidadorDeFuncionarioExistente(_notificationContext);

            _exclusaoDeFuncionario = new ExclusaoDeFuncionario(_funcionarioRepository.Object,
                    _notificationContext,
                    validadorDeFuncionarioExistente);
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

            await _exclusaoDeFuncionario.Excluir(1);

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

            await _exclusaoDeFuncionario.Excluir(1);

            Assert.False(_notificationContext.HasNotifications);

            _funcionarioRepository.Verify(r => r.Delete(1), Times.Once);
        }
    }
}
