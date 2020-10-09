using Moq;
using OnboardingSIGDB1.Domain.Entitys;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Interfaces.Services;
using OnboardingSIGDB1.Domain.Notifications;
using OnboardingSIGDB1.Domain.Services.CargoServices;
using OnboardingSIGDB1.Domain.Tests.EntityBuilders;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace OnboardingSIGDB1.Domain.Tests.Services.CargoServices
{
    public class CargoDeleteServiceTest
    {
        private readonly Mock<ICargoRepository> _cargoRepositoryMock;
        private readonly NotificationContext _notificationContext;

        private readonly ICargoDeleteService _cargoDeleteService;

        public CargoDeleteServiceTest()
        {
            _cargoRepositoryMock = new Mock<ICargoRepository>();
            _notificationContext = new NotificationContext();

            _cargoDeleteService = new CargoDeleteService(_cargoRepositoryMock.Object,
                _notificationContext);

        }

        [Fact(DisplayName = "Deletar cargo com sucesso")]
        public async Task Deletar_Cargo_Existente()
        {
            var builder = new CargoBuilder().WithId(1);
            var entity = builder.Build();

            _cargoRepositoryMock
                .Setup(c => c.GetWithIncludes(It.IsAny<Predicate<Cargo>>()))
                .ReturnsAsync(new List<Cargo> { entity });

            await _cargoDeleteService.Delete(1);

            _cargoRepositoryMock.Verify(r => r.Delete(1), Times.Once);
            Assert.False(_notificationContext.HasNotifications);
        }

        [Fact(DisplayName = "Deletar Id inexistente")]
        public async Task Deletar_Cargo_Inexistente()
        {
            _cargoRepositoryMock
                .Setup(c => c.Get(It.IsAny<Predicate<Cargo>>()))
                .ReturnsAsync(new List<Cargo> { });

            await _cargoDeleteService.Delete(1);

            _cargoRepositoryMock.Verify(r => r.Delete(1), Times.Never);
            Assert.True(_notificationContext.HasNotifications);
        }

        [Fact(DisplayName = "Deletar cargos com funcionarios vinculados")]
        public async Task Deletar_Cargo_Funcionarios_Vinculados()
        {
            var builder = new CargoBuilder()
                .WithId(1)
                .WithFuncionarios(3);

            var cargo = builder.Build();

            _cargoRepositoryMock
                .Setup(c => c.GetWithIncludes(It.IsAny<Predicate<Cargo>>()))
                .ReturnsAsync(new List<Cargo> { cargo });

            await _cargoDeleteService.Delete(1);

            Assert.True(_notificationContext.HasNotifications);
            Assert.Contains(_notificationContext.Notifications,
                n => n.Key.Equals("CargoComFuncionariosVinculados"));
            
            _cargoRepositoryMock.Verify(r => r.Delete(1), Times.Never);
        }

    }
}
