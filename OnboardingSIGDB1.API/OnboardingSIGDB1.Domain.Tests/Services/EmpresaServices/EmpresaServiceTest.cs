using FluentAssertions;
using Moq;
using OnboardingSIGDB1.Domain.Empresas;
using OnboardingSIGDB1.Domain.Empresas.Dtos;
using OnboardingSIGDB1.Domain.Empresas.Services;
using OnboardingSIGDB1.Domain.Empresas.Validators;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Notifications;
using OnboardingSIGDB1.Domain.Tests.EntityBuilders;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace OnboardingSIGDB1.Domain.Tests.Services.EmpresaServices
{
    public class EmpresaServiceTest
    {
        private readonly Mock<IEmpresaRepository> _empresaRepository;
        private readonly NotificationContext _notificationContext;
        private readonly Mock<IValidadorDeCnpj> _validadorDeCnpj;
        private readonly Mock<IValidadorDeEmpresaDuplicada> _validadorDeEmpresaDuplicada;
        private readonly Mock<IValidadorDeEmpresaExistente> _validadorDeEmpresaExistente;

        private readonly ArmazenadorDeEmpresa _armazenadorDeEmpresa;

        public EmpresaServiceTest()
        {

            _empresaRepository = new Mock<IEmpresaRepository>();
            _notificationContext = new NotificationContext();

            _armazenadorDeEmpresa = new ArmazenadorDeEmpresa(_empresaRepository.Object,
                _notificationContext,
                _validadorDeCnpj.Object,
                _validadorDeEmpresaDuplicada.Object,
                _validadorDeEmpresaExistente.Object);
        }

        [Theory(DisplayName = "Inserir empresa nome invalido")]
        [InlineData("")]
        [InlineData("Lorem ipsum molestie purus adipiscing vulputate dictum eros lacinia, litora aliquam viverra conubia dictumst hendrerit senectus sociosqu pulvinar, pela")]
        public async Task Inserir_Empresa_Nome_Invalido(string nome)
        {
            var builder = new EmpresaBuilder().WithNome(nome);
            var empresa = builder.Build();
            var empresaDto = builder.BuildDto(empresa);

            await VerifyInsertEmpresa(empresaDto, empresa);
        }

        [Theory(DisplayName = "Inserir empresa cnpj tamanho invalido")]
        [InlineData("")]
        [InlineData("12345678910111412")]
        public async Task Inserir_Empresa_Cnpj_Tamanho_Invalido(string cnpj)
        {
            var builder = new EmpresaBuilder().WithCnpj(cnpj);
            var empresa = builder.Build();
            var empresaDto = builder.BuildDto(empresa);

            await VerifyInsertEmpresa(empresaDto, empresa);
        }

        private async Task VerifyInsertEmpresa(EmpresaDto empresaDto, Empresa empresa)
        {
            await _armazenadorDeEmpresa.Armazenar(empresaDto);

            _empresaRepository.Verify(r => r.Add(empresa), Times.Never);
            Assert.False(empresa.Valid);
            Assert.True(empresa.ValidationResult.Errors.Count > 0);
        }

        [Fact(DisplayName = "Inserir empresa cnpj inválido")]
        public async Task Inserir_Empresa_Cnpj_Invalido()
        {
            var builder = new EmpresaBuilder().WithCnpj("12345678910111");
            var empresa = builder.Build();
            var empresaDto = builder.BuildDto(empresa);

            await _armazenadorDeEmpresa.Armazenar(empresaDto);

            _empresaRepository.Verify(r => r.Add(empresa), Times.Never);

            Assert.True(_notificationContext.HasNotifications);

            _notificationContext.Notifications.Should().HaveCount(1);

            Assert.Contains(_notificationContext.Notifications,
                n => n.Key.Equals("CnpjInvalido"));
        }

        [Fact(DisplayName = "Inserir empresa cnpj duplicado")]
        public async Task Inserir_Empresa_Cnpj_Duplicado()
        {
            var builder = new EmpresaBuilder();
            var empresa = builder.Build();
            var empresaDto = builder.BuildDto(empresa);

            _empresaRepository
               .Setup(c => c.Get(It.IsAny<Predicate<Empresa>>()))
               .ReturnsAsync(new List<Empresa>() { empresa });

            await _armazenadorDeEmpresa.Armazenar(empresaDto);

            _empresaRepository.Verify(r => r.Add(empresa), Times.Never);

            Assert.True(_notificationContext.HasNotifications);
            _notificationContext.Notifications.Should().HaveCount(1);

            Assert.Contains(_notificationContext.Notifications,
                n => n.Key.Equals("CnpjDuplicado"));
        }

        [Fact(DisplayName = "Inserir empresa com sucesso")]
        public async Task Inserir_Empresa_Sucesso()
        {
            var builder = new EmpresaBuilder();
            var empresa = builder.Build();
            var empresaDto = builder.BuildDto(empresa);

            await _armazenadorDeEmpresa.Armazenar(empresaDto);

            _empresaRepository.Verify(r => r.Add(empresa), Times.Once);

            Assert.False(_notificationContext.HasNotifications);
        }

        [Theory(DisplayName = "Alterar empresa nome invalido")]
        [InlineData("")]
        [InlineData("Lorem ipsum molestie purus adipiscing vulputate dictum eros lacinia, litora aliquam viverra conubia dictumst hendrerit senectus sociosqu pulvinar, pela")]
        public async Task Alterar_Empresa_Nome_Invalido(string nome)
        {
            var builder = new EmpresaBuilder()
                .WithNome(nome)
                .WithId(1);

            var empresa = builder.Build();
            var empresaDto = builder.BuildDto(empresa);

            _empresaRepository
             .Setup(c => c.Get(It.IsAny<Predicate<Empresa>>()))
             .ReturnsAsync(new List<Empresa>() { empresa });

            await _armazenadorDeEmpresa.Armazenar(empresaDto);

            Assert.True(_notificationContext.HasNotifications);
            _notificationContext.Notifications.Should().HaveCount(1);

            _empresaRepository.Verify(r => r.Update(empresa), Times.Never);
        }

        [Fact(DisplayName = "Alterar empresa inexistente")]
        public async Task Alterar_Empresa_Inexistente()
        {
            var builder = new EmpresaBuilder()
                .WithId(1);

            var empresa = builder.Build();
            var empresaDto = builder.BuildDto(empresa);

            _empresaRepository
             .Setup(c => c.Get(It.IsAny<Predicate<Empresa>>()))
             .ReturnsAsync(new List<Empresa>() { });

            await _armazenadorDeEmpresa.Armazenar(empresaDto);

            Assert.True(_notificationContext.HasNotifications);
            _notificationContext.Notifications.Should().HaveCount(1);

            Assert.Contains(_notificationContext.Notifications,
               n => n.Key.Equals("EmpresaInexistente"));

            _empresaRepository.Verify(r => r.Update(empresa), Times.Never);
        }

        [Fact(DisplayName = "Alterar empresa com sucesso")]
        public async Task Alterar_Empresa_Sucesso()
        {
            var builder = new EmpresaBuilder()
                .WithId(1);

            var empresa = builder.Build();
            var empresaDto = builder.BuildDto(empresa);

            _empresaRepository
             .Setup(c => c.Get(It.IsAny<Predicate<Empresa>>()))
             .ReturnsAsync(new List<Empresa>() { empresa });

            await _armazenadorDeEmpresa.Armazenar(empresaDto);

            Assert.False(_notificationContext.HasNotifications);

            _empresaRepository.Verify(r => r.Update(empresa), Times.Once);
        }

    }
}
