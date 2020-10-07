using OnboardingSIGDB1.Domain.Entitys;
using OnboardingSIGDB1.Domain.Entitys.Validators;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Interfaces.Services;
using OnboardingSIGDB1.Domain.Notifications;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Services.EmpresaServices
{
    public class EmpresaService : EmpresaValidationsService, IEmpresaService
    {
        public EmpresaService(IEmpresaRepository empresaRepository,
            NotificationContext notificationContext)
            : base(empresaRepository, notificationContext)
        {
        }
       
        public async Task InsertEmpresa(Empresa empresa)
        {
            if (!empresa.Valid)
            {
                _notificationContext.AddNotifications(empresa.ValidationResult);
                return;
            }

            ValidCnpj(empresa);
            await ValidDuplication(empresa);

            if (_notificationContext.HasNotifications)
                return;

            await _empresaRepository.Add(empresa);

        }

        public async Task UpdateEmpresa(long id, Empresa empresa)
        {
            var empresaDatabase = (await _empresaRepository.Get(emp => emp.Id == id)).FirstOrDefault();

            ValidExistEmpresa(empresaDatabase);

            if (_notificationContext.HasNotifications)
                return;

            empresaDatabase.Nome = empresa.Nome;
            empresaDatabase.DataFundacao = empresa.DataFundacao;

            if (!empresaDatabase.Validate(empresaDatabase, new EmpresaValidator()))
            {
                _notificationContext.AddNotifications(empresa.ValidationResult);
                return;
            }

            await _empresaRepository.Update(empresa);

        }
    }
}
