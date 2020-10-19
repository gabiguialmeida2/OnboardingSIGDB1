using OnboardingSIGDB1.Domain.Cargos.Dtos;
using OnboardingSIGDB1.Domain.Cargos.Validators;
using OnboardingSIGDB1.Domain.Entitys.Validators;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Notifications;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Cargos.Services
{
    public class ArmazenadorDeCargo
    {
        private readonly IRepository<Cargo> _cargoRepository;
        private readonly NotificationContext _notificationContext;
        private readonly IValidadorDeCargoExistente _validadorDeCargoExistente;

        public ArmazenadorDeCargo(IRepository<Cargo> cargoRepository, 
            NotificationContext notificationContext, 
            IValidadorDeCargoExistente validadorDeCargoExistente)
        {
            _cargoRepository = cargoRepository;
            _notificationContext = notificationContext;
            _validadorDeCargoExistente = validadorDeCargoExistente;
        }

        public async Task Armazenar(CargoDto cargoDto)
        {
            if (cargoDto.Id == 0)
                await NovoCargo(cargoDto);
            else
                await EditarCargo(cargoDto);
        }


        public async Task NovoCargo(CargoDto cargoDto)
        {
            var cargo = new Cargo(cargoDto.Descricao);
            
            if (!cargo.Valid)
            {
                _notificationContext.AddNotifications(cargo.ValidationResult);
                return;
            }

            await _cargoRepository.Add(cargo);
        }

        public async Task EditarCargo(CargoDto cargoDto)
        {
            var cargoDatabase = (await _cargoRepository.Get(c => c.Id == cargoDto.Id)).FirstOrDefault();

            _validadorDeCargoExistente.Valid(cargoDatabase);

            if (_notificationContext.HasNotifications)
                return;

            cargoDatabase.AlterarDescricao(cargoDto.Descricao);

            if (!cargoDatabase.Validate(cargoDatabase, new CargoValidator()))
            {
                _notificationContext.AddNotifications(cargoDatabase.ValidationResult);
                return;
            }

            await _cargoRepository.Update(cargoDatabase);
        }
    }
}
