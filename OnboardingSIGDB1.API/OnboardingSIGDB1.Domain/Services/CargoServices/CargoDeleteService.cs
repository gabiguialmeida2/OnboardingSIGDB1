﻿using OnboardingSIGDB1.Domain.Entitys;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Interfaces.Services;
using OnboardingSIGDB1.Domain.Notifications;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Services.CargoServices
{
    public class CargoDeleteService : CargoValidationService, ICargoDeleteService
    {
        public CargoDeleteService(IRepository<Cargo> cargoRepository,
            NotificationContext notificationContext) 
            : base(cargoRepository, notificationContext)
        {

        }

        public async Task Delete(long id)
        {
            var cargoDatabase = await _cargoRepository.Get(cargo => cargo.Id == id);

            ValidExistCargo(cargoDatabase.FirstOrDefault());

            if (_notificationContext.HasNotifications)
                return;

            await _cargoRepository.Delete(id);

        }
    }
}
