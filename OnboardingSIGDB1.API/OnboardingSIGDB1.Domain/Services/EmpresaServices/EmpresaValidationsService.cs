using OnboardingSIGDB1.Domain.Entitys;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Notifications;
using OnboardingSIGDB1.Domain.Utils;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Services.EmpresaServices
{
    public class EmpresaValidationsService
    {
        protected readonly IEmpresaRepository _empresaRepository;
        protected readonly NotificationContext _notificationContext;

        public EmpresaValidationsService(IEmpresaRepository empresaRepository, 
            NotificationContext notificationContext)
        {
            _empresaRepository = empresaRepository;
            _notificationContext = notificationContext;
        }

        protected void ValidEmpresaPossuiFuncionarios(Empresa empresaDatabase)
        {
            if (empresaDatabase != null &&
                empresaDatabase.Funcionarios != null &&
                empresaDatabase.Funcionarios.Any())
            {
                _notificationContext.AddNotification(new Notification("EmpresaComFuncionarios",
                   "Não é possível excluir empresa com funcionários vinculados"));
            }
        }

        protected async Task ValidDuplication(Empresa empresa)
        {
            var empresas = await _empresaRepository.Get(emp => emp.Cnpj.Equals(empresa.Cnpj));

            if (empresas.Any())
            {
                _notificationContext.AddNotification(new Notification("CnpjDuplicado",
                    "Já existe uma empresa cadastrada com esse CNPJ"));
            }
        }

        protected void ValidCnpj(Empresa empresa)
        {
            if (!empresa.Cnpj.IsCnpjValid())
            {
                _notificationContext.AddNotification(new Notification("CnpjInvalido",
                    "Insira um CNPJ válido"));
            }
        }

        protected void ValidExistEmpresa(Empresa empresaDatabase)
        {
            if (empresaDatabase == null)
            {
                _notificationContext.AddNotification(new Notification("EmpresaInexistente",
                    "Não existe uma empresa para o Id informado"));
            }
        }
    }
}
