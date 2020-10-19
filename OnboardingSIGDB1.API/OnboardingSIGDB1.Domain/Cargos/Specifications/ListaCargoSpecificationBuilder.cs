using OnboardingSIGDB1.Domain._Base;

namespace OnboardingSIGDB1.Domain.Cargos.Specifications
{
    public class ListaCargoSpecificationBuilder : SpecificationBuilder<Cargo>
    {
        private string _descricao;
        private long _id;

        public static ListaCargoSpecificationBuilder Novo()
        {
            return new ListaCargoSpecificationBuilder();
        }

        public ListaCargoSpecificationBuilder ComId(long id)
        {
            _id = id;
            return this;
        }

        public ListaCargoSpecificationBuilder ComDescricao(string descricao)
        {
            _descricao = descricao;
            return this;
        }

        public override Specification<Cargo> Build()
        {
            return new Specification<Cargo>(cg =>
                   (_id == 0 || cg.Id == _id) &&
                   (string.IsNullOrEmpty(_descricao) || cg.Descricao.ToUpper().Contains(_descricao))
               )
               .OrderBy(OrdenarPor)
               .Paging(Pagina)
               .PageSize(TamanhoDaPagina);
        }
    }
}
