using OnboardingSIGDB1.Domain.Dto.Filtros;
using OnboardingSIGDB1.Domain.Entitys;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Interfaces.Services;
using OnboardingSIGDB1.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Services.FuncionarioServices
{
    public class FuncionarioConsultaService: IFuncionarioConsultaService
    {
        private readonly IFuncionarioRepository _funcionarioRepository;

        public FuncionarioConsultaService(IFuncionarioRepository funcionarioRepository)
        {
            _funcionarioRepository = funcionarioRepository;
        }

        public async Task<IEnumerable<Funcionario>> GetAll()
        {
            return await _funcionarioRepository.GetWithIncludes(f => true);
        }

        public async Task<Funcionario> GetById(long id)
        {
            var funcionarios = await _funcionarioRepository.GetWithIncludes(emp => emp.Id == id);
            return funcionarios.FirstOrDefault();
        }

        public async Task<IEnumerable<Funcionario>> GetFiltro(FuncionarioFiltroDto filtro)
        {
            List<Predicate<Funcionario>> filtros = new List<Predicate<Funcionario>>();

            if (!string.IsNullOrEmpty(filtro.Nome))
            {
                filtros.Add(emp => emp.Nome.ToUpper().Contains(filtro.Nome.ToUpper()));
            }

            if (!string.IsNullOrEmpty(filtro.Cpf))
            {
                filtros.Add(emp => emp.Cpf.Equals(filtro.Cpf));
            }

            if (filtro.DataContratacaoInicio.HasValue)
            {
                filtros.Add(emp => emp.DataContratacao.HasValue
                    && emp.DataContratacao.Value.Date >= filtro.DataContratacaoInicio.Value.Date);
            }

            if (filtro.DataContratacaoFim.HasValue)
            {
                filtros.Add(emp => emp.DataContratacao.HasValue
                    && emp.DataContratacao.Value.Date <= filtro.DataContratacaoFim.Value.Date);
            }

            var funcionarios = await _funcionarioRepository.GetWithIncludes(ExpressionConcatenation.And(filtros.ToArray()));
            return funcionarios;
        }
    }
}
