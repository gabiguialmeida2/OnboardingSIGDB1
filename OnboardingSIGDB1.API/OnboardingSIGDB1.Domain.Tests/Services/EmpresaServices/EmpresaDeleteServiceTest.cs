using FluentAssertions;
using Moq;
using OnboardingSIGDB1.Domain.Entitys;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Interfaces.Services;
using OnboardingSIGDB1.Domain.Notifications;
using OnboardingSIGDB1.Domain.Services.EmpresaServices;
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
        private readonly NotificationContext _notificationContext;

        private readonly IEmpresaDeleteService _empresaDeleteService;

        public EmpresaDeleteServiceTest()
        {

            _empresaRepository = new Mock<IEmpresaRepository>();
            _notificationContext = new NotificationContext();

            _empresaDeleteService = new EmpresaDeleteService(_empresaRepository.Object,
                _notificationContext);
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

            await _empresaDeleteService.Delete(1);

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

            await _empresaDeleteService.Delete(1);

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

            await _empresaDeleteService.Delete(1);

            Assert.False(_notificationContext.HasNotifications);
            
            _empresaRepository.Verify(r => r.Delete(1), Times.Once);
        }
    }
}
