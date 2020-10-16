namespace OnboardingSIGDB1.Domain._Base
{
    public interface IConsultaBase<TEntity, TDto>
    {
        ResultadoDaConsultaBase Consultar(Specification<TEntity> specification);
    }
}
