using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> Get();
       
        Task<IEnumerable<T>> Get(Predicate<T> predicate);
        Task Add(T entity);
        Task Delete(long id);
        Task Update(T entity);
    }
}
