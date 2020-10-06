using OnboardingSIGDB1.Domain.Dto.Filtros;
using OnboardingSIGDB1.Domain.Entitys;
using OnboardingSIGDB1.Domain.Entitys.Validators;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Interfaces.Services;
using OnboardingSIGDB1.Domain.Notifications;
using OnboardingSIGDB1.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Services
{
    public class EmpresaService : IEmpresaService
    {
        private readonly IRepository<Empresa> _empresaRepository;
        private readonly NotificationContext _notificationContext;

        public EmpresaService(IRepository<Empresa> empresaRepository,
            NotificationContext notificationContext)
        {
            _empresaRepository = empresaRepository;
            _notificationContext = notificationContext;
        }
        public async Task<IEnumerable<Empresa>> GetAll()
        {
            return await _empresaRepository.Get();
        }

        public async Task<Empresa> GetById(long id)
        {
            var empresas = await _empresaRepository.Get(emp => emp.Id == id);
            return empresas.FirstOrDefault();
        }

        public async Task<IEnumerable<Empresa>> GetFiltro(EmpresaFiltroDto filtro)
        {
            List<Predicate<Empresa>> filtros = new List<Predicate<Empresa>>();

            if (!string.IsNullOrEmpty(filtro.Nome))
            {
                filtros.Add(emp => emp.Nome.ToUpper().Contains(filtro.Nome.ToUpper()));
            }

            if (!string.IsNullOrEmpty(filtro.Cnpj))
            {
                filtros.Add(emp => emp.Cnpj.Equals(filtro.Cnpj));
            }

            if (filtro.DataFundacaoInicio.HasValue)
            {
                filtros.Add(emp => emp.DataFundacao.HasValue
                    && emp.DataFundacao.Value.Date >= filtro.DataFundacaoInicio.Value.Date);
            }

            if (filtro.DataFundacaoFim.HasValue)
            {
                filtros.Add(emp => emp.DataFundacao.HasValue
                    && emp.DataFundacao.Value.Date <= filtro.DataFundacaoFim.Value.Date);
            }

            var empresas = await _empresaRepository.Get(ExpressionConcatenation.And(filtros.ToArray()));
            return empresas;
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
        public async Task Delete(long id)
        {
            var empresaDatabase = await _empresaRepository.Get(emp => emp.Id == id);

            ValidExistEmpresa(empresaDatabase.FirstOrDefault());

            if (_notificationContext.HasNotifications)
                return;

            await _empresaRepository.Delete(id);

        }

        private async Task ValidDuplication(Empresa empresa)
        {
            var empresas = await _empresaRepository.Get(emp => emp.Cnpj.Equals(empresa.Cnpj));

            if (empresas.Any())
            {
                _notificationContext.AddNotification(new Notification("CnpjDuplicado", "Já existe uma empresa cadastrada com esse CNPJ"));
            }
        }

        private void ValidCnpj(Empresa empresa)
        {
            if (!empresa.Cnpj.IsCnpjValid())
            {
                _notificationContext.AddNotification(new Notification("CnpjInvalido", "Insira um CNPJ válido"));
            }
        }

        private void ValidExistEmpresa(Empresa empresaDatabase)
        {
            if (empresaDatabase == null)
            {
                _notificationContext.AddNotification(new Notification("EmpresaInexistente", "Não existe uma empresa para o Id informado"));
            }
        }
    }
}
