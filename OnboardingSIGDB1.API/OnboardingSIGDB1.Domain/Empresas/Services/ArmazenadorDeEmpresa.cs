using OnboardingSIGDB1.Domain.Empresas.Dtos;
using OnboardingSIGDB1.Domain.Empresas.Validators;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Notifications;
using OnboardingSIGDB1.Domain.Utils;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Empresas.Services
{
    public class ArmazenadorDeEmpresa
    {
        private readonly IEmpresaRepository _empresaRepository;
        private readonly NotificationContext _notificationContext;
        private readonly IValidadorDeCnpj _validadorDeCnpj;
        private readonly IValidadorDeEmpresaDuplicada _validadorDeEmpresaDuplicada;
        private readonly IValidadorDeEmpresaExistente _validadorDeEmpresaExistente;

        public ArmazenadorDeEmpresa(IEmpresaRepository empresaRepository,
            NotificationContext notificationContext,
            IValidadorDeCnpj validadorDeCnpj,
            IValidadorDeEmpresaDuplicada validadorDeEmpresaDuplicada,
            IValidadorDeEmpresaExistente validadorDeEmpresaExistente)
        {
            _empresaRepository = empresaRepository;
            _notificationContext = notificationContext;
            _validadorDeCnpj = validadorDeCnpj;
            _validadorDeEmpresaDuplicada = validadorDeEmpresaDuplicada;
            _validadorDeEmpresaExistente = validadorDeEmpresaExistente;
        }

        public async Task Armazenar(EmpresaDto empresaDto)
        {
            if (empresaDto.Id == 0)
                await NovaEmpresa(empresaDto);
            else
                await EditarEmpresa(empresaDto);
        }

        private async Task EditarEmpresa(EmpresaDto empresaDto)
        {
            var empresaDatabase = (await _empresaRepository.Get(emp => emp.Id == empresaDto.Id)).FirstOrDefault();

            _validadorDeEmpresaExistente.Valid(empresaDatabase);

            if (empresaDatabase != null)
            {
                empresaDatabase.AlterarNome(empresaDto.Nome);
                empresaDatabase.AlterarDataFundacao(empresaDto.DataFundacao);
             
                if (!empresaDatabase.Validate(empresaDatabase, new EmpresaValidator()))
                {
                    _notificationContext.AddNotifications(empresaDatabase.ValidationResult);
                    return;
                }

                await _empresaRepository.Update(empresaDatabase);
            }

        }

        private async Task NovaEmpresa(EmpresaDto empresaDto)
        {
            var empresa = new Empresa(empresaDto.Nome,
                empresaDto.Cnpj.RemoveMaskCnpj(),
                empresaDto.DataFundacao);

            if (!empresa.Valid)
            {
                _notificationContext.AddNotifications(empresa.ValidationResult);
                return;
            }

            _validadorDeCnpj.Valid(empresa);
            await _validadorDeEmpresaDuplicada.Valid(empresa);

            if (_notificationContext.HasNotifications)
                return;

            await _empresaRepository.Add(empresa);
        }

    }
}
