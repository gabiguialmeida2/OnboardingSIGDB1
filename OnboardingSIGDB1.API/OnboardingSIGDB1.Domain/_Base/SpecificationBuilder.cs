namespace OnboardingSIGDB1.Domain._Base
{
    public abstract class SpecificationBuilder<TEntity>
    {
        protected int Pagina;
        protected int TamanhoDaPagina;
        protected string OrdenarPor;

        public SpecificationBuilder<TEntity> ComPagina(int pagina)
        {
            Pagina = pagina;
            return this;
        }

        public SpecificationBuilder<TEntity> ComTamanhoDaPagina(int tamanhoDaPagina)
        {
            TamanhoDaPagina = tamanhoDaPagina;
            return this;
        }
        public SpecificationBuilder<TEntity> ComOrdemPor(string ordenarPor)
        {
            OrdenarPor = ordenarPor;
            return this;
        }

        public abstract Specification<TEntity> Build();
    }
}
