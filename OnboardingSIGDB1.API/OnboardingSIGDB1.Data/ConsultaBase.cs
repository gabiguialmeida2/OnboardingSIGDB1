using OnboardingSIGDB1.Domain._Base;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace OnboardingSIGDB1.Data
{
    public class ConsultaBase<TEntity, TDto>: IConsultaBase<TEntity, TDto>
        where TEntity : class
    {
        private readonly DataContext _context;
        private readonly ResultadoDaConsultaBase _resultado;
        private readonly IMapper _mapper;
        protected IQueryable<TEntity> Query { get; set; }

        public ConsultaBase(DataContext context,
            IMapper mapper)
        {
            _context = context;
            _resultado = new ResultadoDaConsultaBase();
            _mapper = mapper;
        }

        public virtual void PrepararQuery(Specification<TEntity> specification)
        {
            Query = _context.Set<TEntity>().Where(specification.Predicate);
        }


        public ResultadoDaConsultaBase Consultar(Specification<TEntity> specification)
        {
            PrepararQuery(specification);

            if (specification.Order != null)
            {
                Query = ConfigurarOrdenacao(specification);
            }

            if (!specification.Page.HasValue)
            {
                var entities = Query.ToList();
                _resultado.Lista = (IEnumerable<object>)_mapper.Map<IEnumerable<TDto>>(entities);
                _resultado.Total = Query.Count();
            }
            else
            {
                var page = (specification.Page.Value - 1) >= 0 ? (specification.Page.Value - 1) : 0;
                var total = page * specification.Size;

                List<TEntity> entities;

                _resultado.Total = Query.Count();
                entities = Query.Skip(total).Take(specification.Size).ToList();

                _resultado.Lista = (IEnumerable<object>)_mapper.Map<IEnumerable<TDto>>(entities);
            }

            return _resultado;
        }

        private IQueryable<TEntity> ConfigurarOrdenacao(Specification<TEntity> specification)
        {
            return Query.OrderByDescending(specification.Order);
        }

    }
}
