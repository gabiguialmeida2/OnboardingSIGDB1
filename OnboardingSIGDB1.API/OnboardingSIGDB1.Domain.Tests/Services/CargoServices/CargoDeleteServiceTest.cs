using Moq;
using OnboardingSIGDB1.Domain.Cargos;
using OnboardingSIGDB1.Domain.Cargos.Services;
using OnboardingSIGDB1.Domain.Cargos.Validators;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Notifications;
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
        private readonly IValidadorDeCargoExistente _validadorDeCargoExistente;
        private readonly IValidadorDeCargoComFuncionarios _validadorDeCargoComFuncionarios;
        private readonly NotificationContext _notificationContext;

        private readonly ExclusaoDeCargo _exclusaoDeCargo;

        public CargoDeleteServiceTest()
        {
            _cargoRepositoryMock = new Mock<ICargoRepository>();
            _notificationContext = new NotificationContext();
            _validadorDeCargoExistente = new ValidadorDeCargoExistente(_notificationContext);
            _validadorDeCargoComFuncionarios = new ValidadorDeCargoComFuncionarios(_notificationContext);

            _exclusaoDeCargo = new ExclusaoDeCargo(_cargoRepositoryMock.Object,
                _notificationContext,
                _validadorDeCargoExistente,
                _validadorDeCargoComFuncionarios
                );

        }

        [Fact(DisplayName = "Deletar cargo com sucesso")]
        public async Task Deletar_Cargo_Existente()
        {
            var builder = new CargoBuilder().WithId(1);
            var entity = builder.Build();

            _cargoRepositoryMock
                .Setup(c => c.GetWithIncludes(It.IsAny<Predicate<Cargo>>()))
                .ReturnsAsync(new List<Cargo> { entity });

            await _exclusaoDeCargo.Excluir(1);

            _cargoRepositoryMock.Verify(r => r.Delete(1), Times.Once);
            Assert.False(_notificationContext.HasNotifications);
        }

        [Fact(DisplayName = "Deletar Id inexistente")]
        public async Task Deletar_Cargo_Inexistente()
        {
            _cargoRepositoryMock
                .Setup(c => c.GetWithIncludes(It.IsAny<Predicate<Cargo>>()))
                .ReturnsAsync(new List<Cargo> { });

            await _exclusaoDeCargo.Excluir(1);

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

            await _exclusaoDeCargo.Excluir(1);

            Assert.True(_notificationContext.HasNotifications);
            Assert.Contains(_notificationContext.Notifications,
                n => n.Key.Equals("CargoComFuncionariosVinculados"));
            
            _cargoRepositoryMock.Verify(r => r.Delete(1), Times.Never);
        }

    }
}
