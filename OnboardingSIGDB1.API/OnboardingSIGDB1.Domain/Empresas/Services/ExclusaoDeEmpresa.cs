using OnboardingSIGDB1.Domain.Empresas.Validators;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Empresas.Services
{
    public class ExclusaoDeEmpresa
    {
        private readonly IEmpresaRepository _empresaRepository;
        private readonly NotificationContext _notificationContext;
        private readonly IValidadorDeEmpresaExistente _validadorDeEmpresaExistente;
        private readonly IValidadorDeEmpresaComFuncionarios _validadorDeEmpresaComFuncionarios;

        public ExclusaoDeEmpresa(IEmpresaRepository empresaRepository, 
            NotificationContext notificationContext, 
            IValidadorDeEmpresaExistente validadorDeEmpresaExistente, 
            IValidadorDeEmpresaComFuncionarios validadorDeEmpresaComFuncionarios)
        {
            _empresaRepository = empresaRepository;
            _notificationContext = notificationContext;
            _validadorDeEmpresaExistente = validadorDeEmpresaExistente;
            _validadorDeEmpresaComFuncionarios = validadorDeEmpresaComFuncionarios;
        }

        public async Task Excluir(long id)
        {
            var empresaDatabase = (
                 await _empresaRepository
                     .GetWithFuncionarios(emp => emp.Id == id)
             ).FirstOrDefault();

            _validadorDeEmpresaExistente.Valid(empresaDatabase);
            _validadorDeEmpresaComFuncionarios.Valid(empresaDatabase);

            if (_notificationContext.HasNotifications)
                return;

            await _empresaRepository.Delete(id);
        }
    }
}
