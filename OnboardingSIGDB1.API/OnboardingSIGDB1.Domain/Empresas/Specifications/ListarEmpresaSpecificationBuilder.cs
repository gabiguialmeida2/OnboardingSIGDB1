using OnboardingSIGDB1.Domain._Base;
using System;

namespace OnboardingSIGDB1.Domain.Empresas.Specifications
{
    public class ListarEmpresaSpecificationBuilder : SpecificationBuilder<Empresa>
    {
        private long _id;
        private string _nome;
        private string _cnpj;
        private DateTime? _dataFundacaoInicio;
        private DateTime? _dataFundacaoFim;

        public static ListarEmpresaSpecificationBuilder Novo()
        {
            return new ListarEmpresaSpecificationBuilder();
        }

        public ListarEmpresaSpecificationBuilder ComId(long id)
        {
            _id = id;
            return this;
        }

        public ListarEmpresaSpecificationBuilder ComNome(string nome)
        {
            _nome = nome;
            return this;
        }

        public ListarEmpresaSpecificationBuilder ComCnpj(string cnpj)
        {
            _cnpj = cnpj;
            return this;
        }

        public ListarEmpresaSpecificationBuilder ComDataFundacaoInicio(DateTime? dataFundacaoInicio)
        {
            _dataFundacaoInicio = dataFundacaoInicio;
            return this;
        }

        public ListarEmpresaSpecificationBuilder ComDataFundacaoFim(DateTime? dataFundacaoFim)
        {
            _dataFundacaoFim = dataFundacaoFim;
            return this;
        }

        public override Specification<Empresa> Build()
        {
            return new Specification<Empresa>(emp =>
                    (_id == 0 || emp.Id == _id) &&
                    (string.IsNullOrEmpty(_nome) || emp.Nome.ToUpper().Contains(_nome)) &&
                    (string.IsNullOrEmpty(_cnpj) || emp.Cnpj.Equals(_cnpj)) &&
                    (!_dataFundacaoInicio.HasValue || (emp.DataFundacao.HasValue && emp.DataFundacao.Value.Date >= _dataFundacaoInicio.Value.Date)) &&
                    (!_dataFundacaoFim.HasValue || (emp.DataFundacao.HasValue && emp.DataFundacao.Value.Date <= _dataFundacaoFim.Value.Date))
                )
                .OrderBy(OrdenarPor)
                .Paging(Pagina)
                .PageSize(TamanhoDaPagina);
        }
    }
}
