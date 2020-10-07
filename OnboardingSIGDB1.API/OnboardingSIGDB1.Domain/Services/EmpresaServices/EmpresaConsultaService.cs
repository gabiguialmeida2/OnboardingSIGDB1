using OnboardingSIGDB1.Domain.Dto.Filtros;
using OnboardingSIGDB1.Domain.Entitys;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Interfaces.Services;
using OnboardingSIGDB1.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Services.EmpresaServices
{
    public class EmpresaConsultaService: IEmpresaConsultaService
    {
        private readonly IEmpresaRepository _empresaRepository;

        public EmpresaConsultaService(IEmpresaRepository empresaRepository)
        {
            _empresaRepository = empresaRepository;
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

    }
}
