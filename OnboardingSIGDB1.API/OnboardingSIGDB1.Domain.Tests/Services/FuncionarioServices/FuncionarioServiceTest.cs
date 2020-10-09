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
    public class FuncionarioServiceTest
    {
        private readonly Mock<IFuncionarioRepository> _funcionarioRepository;
        private readonly NotificationContext _notificationContext;

        private readonly IFuncionarioService _funcionarioService;

        public FuncionarioServiceTest()
        {

            _funcionarioRepository = new Mock<IFuncionarioRepository>();
            _notificationContext = new NotificationContext();

            _funcionarioService = new FuncionarioService(_funcionarioRepository.Object,
                _notificationContext);
        }

        [Theory(DisplayName = "Inserir funcionario nome invalido")]
        [InlineData("")]
        [InlineData("Lorem ipsum molestie purus adipiscing vulputate dictum eros lacinia, litora aliquam viverra conubia dictumst hendrerit senectus sociosqu pulvinar, pela")]
        public async Task Inserir_Funcionario_Nome_Invalido(string nome)
        {
            var builder = new FuncionarioBuilder().WithNome(nome);
            var funcionario = builder.Build();

            await VerifyInsertFuncionario(funcionario);
        }

        [Theory(DisplayName = "Inserir funcionario cpf tamanho invalido")]
        [InlineData("")]
        [InlineData("123456789101")]
        public async Task Inserir_Funcionario_Cpf_Tamanho_Invalido(string cpf)
        {
            var builder = new FuncionarioBuilder().WithCpf(cpf);
            var funcionario = builder.Build();

            await VerifyInsertFuncionario(funcionario);
        }

        private async Task VerifyInsertFuncionario(Funcionario funcionario)
        {
            await _funcionarioService.InsertFuncionario(funcionario);

            _funcionarioRepository.Verify(r => r.Add(funcionario), Times.Never);
            Assert.False(funcionario.Valid);
            Assert.True(funcionario.ValidationResult.Errors.Count > 0);
        }

        [Fact(DisplayName = "Inserir funcionario cpf inválido")]
        public async Task Inserir_Funcionario_Cpf_Invalido()
        {
            var builder = new FuncionarioBuilder().WithCpf("12345678910");
            var funcionario = builder.Build();

            await _funcionarioService.InsertFuncionario(funcionario);

            _funcionarioRepository.Verify(r => r.Add(funcionario), Times.Never);

            Assert.True(_notificationContext.HasNotifications);

            _notificationContext.Notifications.Should().HaveCount(1);

            Assert.Contains(_notificationContext.Notifications,
                n => n.Key.Equals("CpfInvalido"));
        }

        [Fact(DisplayName = "Inserir funcionario cpf duplicado")]
        public async Task Inserir_Funcionario_Cpf_Duplicado()
        {
            var builder = new FuncionarioBuilder();
            var funcionario = builder.Build();

            _funcionarioRepository
               .Setup(c => c.Get(It.IsAny<Predicate<Funcionario>>()))
               .ReturnsAsync(new List<Funcionario>() { funcionario });

            await _funcionarioService.InsertFuncionario(funcionario);

            _funcionarioRepository.Verify(r => r.Add(funcionario), Times.Never);

            Assert.True(_notificationContext.HasNotifications);
            _notificationContext.Notifications.Should().HaveCount(1);

            Assert.Contains(_notificationContext.Notifications,
                n => n.Key.Equals("CpfDuplicado"));
        }

        [Fact(DisplayName = "Inserir funcionario com sucesso")]
        public async Task Inserir_Funcionario_Sucesso()
        {
            var builder = new FuncionarioBuilder();
            var funcionario = builder.Build();

            await _funcionarioService.InsertFuncionario(funcionario);

            _funcionarioRepository.Verify(r => r.Add(funcionario), Times.Once);

            Assert.False(_notificationContext.HasNotifications);
        }

        [Theory(DisplayName = "Alterar funcionario nome invalido")]
        [InlineData("")]
        [InlineData("Lorem ipsum molestie purus adipiscing vulputate dictum eros lacinia, litora aliquam viverra conubia dictumst hendrerit senectus sociosqu pulvinar, pela")]
        public async Task Alterar_Funcionario_Nome_Invalido(string nome)
        {
            var builder = new FuncionarioBuilder()
                .WithNome(nome)
                .WithId(1);

            var funcionario = builder.Build();

            _funcionarioRepository
             .Setup(c => c.Get(It.IsAny<Predicate<Funcionario>>()))
             .ReturnsAsync(new List<Funcionario>() { funcionario });

            await _funcionarioService.UpdateFuncionario(1, funcionario);

            Assert.True(_notificationContext.HasNotifications);
            _notificationContext.Notifications.Should().HaveCount(1);

            _funcionarioRepository.Verify(r => r.Update(funcionario), Times.Never);
        }

        [Fact(DisplayName = "Alterar funcionario inexistente")]
        public async Task Alterar_Funcionario_Inexistente()
        {
            var builder = new FuncionarioBuilder()
                .WithId(1);

            var funcionario = builder.Build();

            _funcionarioRepository
             .Setup(c => c.Get(It.IsAny<Predicate<Funcionario>>()))
             .ReturnsAsync(new List<Funcionario>() { });

            await _funcionarioService.UpdateFuncionario(1, funcionario);

            Assert.True(_notificationContext.HasNotifications);
            _notificationContext.Notifications.Should().HaveCount(1);

            Assert.Contains(_notificationContext.Notifications,
               n => n.Key.Equals("FuncionarioInexistente"));

            _funcionarioRepository.Verify(r => r.Update(funcionario), Times.Never);
        }

        [Fact(DisplayName = "Alterar funcionario com sucesso")]
        public async Task Alterar_Funcionario_Sucesso()
        {
            var builder = new FuncionarioBuilder()
                .WithId(1);

            var funcionario = builder.Build();

            _funcionarioRepository
             .Setup(c => c.Get(It.IsAny<Predicate<Funcionario>>()))
             .ReturnsAsync(new List<Funcionario>() { funcionario });

            await _funcionarioService.UpdateFuncionario(1, funcionario);

            Assert.False(_notificationContext.HasNotifications);

            _funcionarioRepository.Verify(r => r.Update(funcionario), Times.Once);
        }
    }
}
