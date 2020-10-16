using FluentAssertions;
using Moq;
using OnboardingSIGDB1.Domain.Empresas;
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
    public class EmpresaDeleteServiceTest
    {
        private readonly Mock<IEmpresaRepository> _empresaRepository;
        private readonly Mock<IValidadorDeEmpresaExistente> _validadorDeEmpresaExistente;
        private readonly Mock<IValidadorDeEmpresaComFuncionarios> _validadorDeEmpresaComFuncionarios;
        private readonly NotificationContext _notificationContext;

        private readonly ExclusaoDeEmpresa _exclusaoDeEmpresa;

        public EmpresaDeleteServiceTest()
        {

            _empresaRepository = new Mock<IEmpresaRepository>();
            _notificationContext = new NotificationContext();

            _exclusaoDeEmpresa = new ExclusaoDeEmpresa(_empresaRepository.Object,
                _notificationContext,
                _validadorDeEmpresaExistente.Object,
                _validadorDeEmpresaComFuncionarios.Object);
        }

        [Fact(DisplayName = "Deletar empresa inexistente")]
        public async Task Deletar_Empresa_Inexistente()
        {
            var builder = new EmpresaBuilder()
                .WithId(1);

            var empresa = builder.Build();

            _empresaRepository
             .Setup(c => c.GetWithFuncionarios(It.IsAny<Predicate<Empresa>>()))
             .ReturnsAsync(new List<Empresa>() { });

            await _exclusaoDeEmpresa.Excluir(1);

            Assert.True(_notificationContext.HasNotifications);
            _notificationContext.Notifications.Should().HaveCount(1);

            Assert.Contains(_notificationContext.Notifications,
               n => n.Key.Equals("EmpresaInexistente"));

            _empresaRepository.Verify(r => r.Delete(1), Times.Never);
        }

        [Fact(DisplayName = "Deletar empresa com funcionarios")]
        public async Task Deletar_Empresa_Com_Funcionarios()
        {
            var builder = new EmpresaBuilder()
                .WithId(1)
                .WithFuncionarios(3);

            var empresa = builder.Build();

            _empresaRepository
                 .Setup(c => c.GetWithFuncionarios(It.IsAny<Predicate<Empresa>>()))
                 .ReturnsAsync(new List<Empresa>() { empresa });

            await _exclusaoDeEmpresa.Excluir(1);

            Assert.True(_notificationContext.HasNotifications);
            _notificationContext.Notifications.Should().HaveCount(1);

            Assert.Contains(_notificationContext.Notifications,
               n => n.Key.Equals("EmpresaComFuncionarios"));

            _empresaRepository.Verify(r => r.Delete(1), Times.Never);
        }

        [Fact(DisplayName = "Deletar empresa com sucesso")]
        public async Task Deletar_Empresa_Sucesso()
        {
            var builder = new EmpresaBuilder()
                .WithId(1);

            var empresa = builder.Build();

            _empresaRepository
                 .Setup(c => c.GetWithFuncionarios(It.IsAny<Predicate<Empresa>>()))
                 .ReturnsAsync(new List<Empresa>() { empresa });

            await _exclusaoDeEmpresa.Excluir(1);

            Assert.False(_notificationContext.HasNotifications);
            
            _empresaRepository.Verify(r => r.Delete(1), Times.Once);
        }
    }
}
