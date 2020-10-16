using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace OnboardingSIGDB1.Domain._Base
{
    public class Specification<T>
    {
        public Expression<Func<T, bool>> Predicate { get; protected set; }
        public int? Page { get; set; }
        public int Size { get; set; }
        public Expression<Func<T, object>> Order { get; set; }

        public Specification(Expression<Func<T, bool>> predicate)
        {
            Predicate = predicate;
            Size = 10;
        }

        public Specification<T> Paging(int page)
        {
            var specification = Build();
            if (page > 0)
                specification.Page = page;
            return specification;
        }

        public Specification<T> PageSize(int size)
        {
            var specification = Build();
            if (size > 0)
                specification.Size = size;
            return specification;
        }

        public Specification<T> OrderBy(string fieldName)
        {
            var specification = Build();

            if (string.IsNullOrEmpty(fieldName))
                return specification;

            var param = Expression.Parameter(typeof(T), "x");
            Expression conversion = Expression.Convert(Expression.Property(param, fieldName), typeof(object));
            var order = Expression.Lambda<Func<T, object>>(conversion, param);
            specification.Order = order;
            return specification;
        }

        private Specification<T> Build()
        {
            return new Specification<T>(Predicate)
            {
                Page = Page,
                Size = Size,
                Order = Order
            };
        }
    }
}
