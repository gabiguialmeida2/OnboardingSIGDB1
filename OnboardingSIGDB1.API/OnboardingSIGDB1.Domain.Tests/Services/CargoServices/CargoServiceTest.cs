using FluentAssertions;
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
    public class CargoServiceTest
    {
        private readonly Mock<IRepository<Cargo>> _cargoRepositoryMock;
        private readonly NotificationContext _notificationContext;

        private readonly ICargoService _cargoService;

        public CargoServiceTest()
        {
            _cargoRepositoryMock = new Mock<IRepository<Cargo>>();
            _notificationContext = new NotificationContext();

            _cargoService = new CargoService(_cargoRepositoryMock.Object,
                _notificationContext);
        }

        [Theory(DisplayName = "Inserir cargo inválido")]
        [InlineData("")]
        [InlineData("Lorem ipsum molestie purus adipiscing vulputate dictum eros lacinia, litora aliquam viverra conubia dictumst hendrerit senectus sociosqu pulvinar, pellentesque tellus ultrices praesent dictum venenatis donec. leo turpis mollis ligula fusce torquent es")]
        public async Task Inserir_Cargo_Invalido(string descricao)
        {
            var builder = new CargoBuilder().WithDescricao(descricao);
            var entity = builder.Build();

            await _cargoService.InsertCargo(entity);

            _cargoRepositoryMock.Verify(r => r.Add(entity), Times.Never);
            Assert.True(_notificationContext.HasNotifications);
            entity.ValidationResult.Errors.Should().HaveCount(1);
        }

        [Fact(DisplayName = "Inserir cargo válido")]
        public async Task Inserir_Cargo_Valido()
        {
            var builder = new CargoBuilder();
            var entity = builder.Build();

            await _cargoService.InsertCargo(entity);

            _cargoRepositoryMock.Verify(r => r.Add(entity), Times.Once);
            Assert.False(_notificationContext.HasNotifications);
        }

        [Theory(DisplayName = "Alterar cargo descrição invalida")]
        [InlineData("")]
        [InlineData("Lorem ipsum molestie purus adipiscing vulputate dictum eros lacinia, litora aliquam viverra conubia dictumst hendrerit senectus sociosqu pulvinar, pellentesque tellus ultrices praesent dictum venenatis donec. leo turpis mollis ligula fusce torquent es")]
        public async Task Alterar_Cargo_Descricao_Invalida(string descricao)
        {
            var builder = new CargoBuilder()
                .WithDescricao(descricao)
                .WithId(1);
            var entity = new Cargo(descricao);

            _cargoRepositoryMock
               .Setup(c => c.Get(It.IsAny<Predicate<Cargo>>()))
               .ReturnsAsync(new List<Cargo> { entity });

            await _cargoService.UpdateCargo(1, entity);

            _cargoRepositoryMock.Verify(r => r.Update(entity), Times.Never);
            Assert.True(_notificationContext.HasNotifications);
            entity.ValidationResult.Errors.Should().HaveCount(1);
        }

        [Fact(DisplayName = "Alterar cargo inexistente")]
        public async Task Alterar_Cargo_Inexistente()
        {
            var builder = new CargoBuilder().WithId(10);
            var entity = builder.Build();

            _cargoRepositoryMock
               .Setup(c => c.Get(It.IsAny<Predicate<Cargo>>()))
               .ReturnsAsync(new List<Cargo> { });

            await _cargoService.UpdateCargo(10, entity);

            _cargoRepositoryMock.Verify(r => r.Update(entity), Times.Never);
            Assert.True(_notificationContext.HasNotifications);
            _notificationContext.Notifications.Should().HaveCount(1);
        }

        [Fact(DisplayName = "Alterar cargo com sucesso")]
        public async Task Alterar_Cargo_Sucesso()
        {
            var builder = new CargoBuilder().WithId(1);
            var entityAlteracao = builder.Build();

            _cargoRepositoryMock
               .Setup(c => c.Get(It.IsAny<Predicate<Cargo>>()))
               .ReturnsAsync(new List<Cargo> { entityAlteracao });

            entityAlteracao.Descricao = "Teste Alteração";

            await _cargoService.UpdateCargo(1, entityAlteracao);

            _cargoRepositoryMock.Verify(r => r.Update(entityAlteracao), Times.Once);
            Assert.False(_notificationContext.HasNotifications);
        }
    }
}
