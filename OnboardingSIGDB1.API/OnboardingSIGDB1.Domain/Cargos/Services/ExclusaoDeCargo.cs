using OnboardingSIGDB1.Domain.Cargos.Validators;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Notifications;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Cargos.Services
{
    public class ExclusaoDeCargo
    {
        private readonly ICargoRepository _cargoRepository;
        private readonly NotificationContext _notificationContext;
        private readonly IValidadorDeCargoExistente _validadorDeCargoExistente;
        private readonly IValidadorDeCargoComFuncionarios _validadorDeCargoComFuncionarios;

        public ExclusaoDeCargo(ICargoRepository cargoRepository, 
            NotificationContext notificationContext, 
            IValidadorDeCargoExistente validadorDeCargoExistente, 
            IValidadorDeCargoComFuncionarios validadorDeCargoComFuncionarios)
        {
            _cargoRepository = cargoRepository;
            _notificationContext = notificationContext;
            _validadorDeCargoExistente = validadorDeCargoExistente;
            _validadorDeCargoComFuncionarios = validadorDeCargoComFuncionarios;
        }

        public async Task Excluir(long id)
        {
            var cargoDatabase = (await _cargoRepository.GetWithIncludes(cargo => cargo.Id == id)).FirstOrDefault();

            _validadorDeCargoExistente.Valid(cargoDatabase);
            _validadorDeCargoComFuncionarios.Valid(cargoDatabase);

            if (_notificationContext.HasNotifications)
                return;

            await _cargoRepository.Delete(id);
        }
        }
}
