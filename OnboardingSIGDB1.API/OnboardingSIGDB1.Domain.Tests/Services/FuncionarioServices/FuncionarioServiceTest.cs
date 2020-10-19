using FluentAssertions;
using Moq;
using OnboardingSIGDB1.Domain.Funcionarios;
using OnboardingSIGDB1.Domain.Funcionarios.Dtos;
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
    public class FuncionarioServiceTest
    {
        private readonly Mock<IFuncionarioRepository> _funcionarioRepository;
        private readonly NotificationContext _notificationContext;
        private readonly ArmazenadorDeFuncionario _armazenadorDeFuncionario;

        public FuncionarioServiceTest()
        {
            _funcionarioRepository = new Mock<IFuncionarioRepository>();
            _notificationContext = new NotificationContext();
            var validadorDeCpf = new ValidadorDeCpf(_notificationContext);
            var validadorDeFuncionarioExistente = new ValidadorDeFuncionarioExistente(_notificationContext);
            var validadorDeFuncionarioDuplicado = new ValidadorDeFuncionarioDuplicado(_funcionarioRepository.Object,
                _notificationContext);

            _armazenadorDeFuncionario = new ArmazenadorDeFuncionario(_funcionarioRepository.Object,
                _notificationContext,
                validadorDeCpf,
                validadorDeFuncionarioExistente,
                validadorDeFuncionarioDuplicado);

        }

        [Theory(DisplayName = "Inserir funcionario nome invalido")]
        [InlineData("")]
        [InlineData("Lorem ipsum molestie purus adipiscing vulputate dictum eros lacinia, litora aliquam viverra conubia dictumst hendrerit senectus sociosqu pulvinar, pela")]
        public async Task Inserir_Funcionario_Nome_Invalido(string nome)
        {
            var builder = new FuncionarioBuilder().WithNome(nome);
            var funcionario = builder.Build();
            var dto = builder.BuildDto();

            await VerifyInsertFuncionario(funcionario, dto);
        }

        [Theory(DisplayName = "Inserir funcionario cpf tamanho invalido")]
        [InlineData("")]
        [InlineData("123456789101")]
        public async Task Inserir_Funcionario_Cpf_Tamanho_Invalido(string cpf)
        {
            var builder = new FuncionarioBuilder().WithCpf(cpf);
            var funcionario = builder.Build();
            var dto = builder.BuildDto();

            await VerifyInsertFuncionario(funcionario, dto);
        }

        private async Task VerifyInsertFuncionario(Funcionario funcionario, FuncionarioDto funcionarioDto)
        {
            await _armazenadorDeFuncionario.Armazenar(funcionarioDto);
            VerifyFuncionarioAdd(funcionarioDto, Times.Never());
            Assert.False(funcionario.Valid);
            Assert.True(funcionario.ValidationResult.Errors.Count > 0);
        }

        [Fact(DisplayName = "Inserir funcionario cpf inválido")]
        public async Task Inserir_Funcionario_Cpf_Invalido()
        {
            var builder = new FuncionarioBuilder().WithCpf("12345678910");
            var funcionario = builder.Build();
            var dto = builder.BuildDto();

            await _armazenadorDeFuncionario.Armazenar(dto);
            
            VerifyFuncionarioAdd(dto, Times.Never());

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
            var dto = builder.BuildDto();


            _funcionarioRepository
               .Setup(c => c.Get(It.IsAny<Predicate<Funcionario>>()))
               .ReturnsAsync(new List<Funcionario>() { funcionario });

            await _armazenadorDeFuncionario.Armazenar(dto);
            VerifyFuncionarioAdd(dto, Times.Never());

            Assert.True(_notificationContext.HasNotifications);
            _notificationContext.Notifications.Should().HaveCount(1);

            Assert.Contains(_notificationContext.Notifications,
                n => n.Key.Equals("CpfDuplicado"));
        }

        [Fact(DisplayName = "Inserir funcionario com sucesso")]
        public async Task Inserir_Funcionario_Sucesso()
        {
            var builder = new FuncionarioBuilder();
            var dto = builder.BuildDto();
            await _armazenadorDeFuncionario.Armazenar(dto);

            VerifyFuncionarioAdd(dto, Times.Once());

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
            var dto = builder.BuildDto();

            _funcionarioRepository
             .Setup(c => c.Get(It.IsAny<Predicate<Funcionario>>()))
             .ReturnsAsync(new List<Funcionario>() { funcionario });

            await _armazenadorDeFuncionario.Armazenar(dto);

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
            var dto = builder.BuildDto();
            
            _funcionarioRepository
             .Setup(c => c.Get(It.IsAny<Predicate<Funcionario>>()))
             .ReturnsAsync(new List<Funcionario>() { });

            await _armazenadorDeFuncionario.Armazenar(dto);

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
            var dto = builder.BuildDto();
            _funcionarioRepository
             .Setup(c => c.Get(It.IsAny<Predicate<Funcionario>>()))
             .ReturnsAsync(new List<Funcionario>() { funcionario });

            await _armazenadorDeFuncionario.Armazenar(dto);
            
            Assert.False(_notificationContext.HasNotifications);

            _funcionarioRepository.Verify(r => r.Update(funcionario), Times.Once);
        }

        private void VerifyFuncionarioAdd(FuncionarioDto funcionarioDto, Times times)
        {
            _funcionarioRepository.Verify(r => r.Add(It.Is<Funcionario>(func =>
                func.DataContratacao == funcionarioDto.DataContratacao
                && func.Cpf == funcionarioDto.Cpf
                && func.Nome == funcionarioDto.Nome)), times);
        }
    }
}
