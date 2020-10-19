using OnboardingSIGDB1.Domain._Base;
using System;

namespace OnboardingSIGDB1.Domain.Funcionarios.Specifications
{
    public class ListarFuncionarioSpecificationBuilder : SpecificationBuilder<Funcionario>
    {
        private long _id;
        private string _nome;
        private string _cpf;
        private DateTime? _dataContratacaoInicio;
        private DateTime? _dataContratacaoFim;

        public static ListarFuncionarioSpecificationBuilder Novo()
        {
            return new ListarFuncionarioSpecificationBuilder();
        }

        public ListarFuncionarioSpecificationBuilder ComId(long id)
        {
            _id = id;
            return this;
        }

        public ListarFuncionarioSpecificationBuilder ComNome(string nome)
        {
            _nome = nome;
            return this;
        }

        public ListarFuncionarioSpecificationBuilder ComCpf(string cpf)
        {
            _cpf = cpf;
            return this;
        }

        public ListarFuncionarioSpecificationBuilder ComDataContratacaoInicio(DateTime? dataFundacaoInicio)
        {
            _dataContratacaoInicio = dataFundacaoInicio;
            return this;
        }

        public ListarFuncionarioSpecificationBuilder ComDataContratacaoFim(DateTime? dataFundacaoFim)
        {
            _dataContratacaoFim = dataFundacaoFim;
            return this;
        }

        public override Specification<Funcionario> Build()
        {
            return new Specification<Funcionario>(func =>
                    (_id == 0 || func.Id == _id) &&
                    (string.IsNullOrEmpty(_nome) || func.Nome.ToUpper().Contains(_nome)) &&
                    (string.IsNullOrEmpty(_cpf) || func.Cpf.Equals(_cpf)) &&
                    (!_dataContratacaoInicio.HasValue || (func.DataContratacao.HasValue && func.DataContratacao.Value.Date >= _dataContratacaoInicio.Value.Date)) &&
                    (!_dataContratacaoFim.HasValue || (func.DataContratacao.HasValue && func.DataContratacao.Value.Date <= _dataContratacaoFim.Value.Date))
                )
                .OrderBy(OrdenarPor)
                .Paging(Pagina)
                .PageSize(TamanhoDaPagina);
        }
    }
}
