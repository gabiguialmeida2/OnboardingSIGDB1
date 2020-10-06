using Microsoft.EntityFrameworkCore;
using OnboardingSIGDB1.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork;
        public Repository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Add(T entity)
        {
            _unitOfWork.Context.Set<T>().Add(entity);
            await _unitOfWork.Commit();
        }

        public async Task Delete(long id)
        {
            T existing = _unitOfWork.Context.Set<T>().Find(id);
            if (existing != null) _unitOfWork.Context.Set<T>().Remove(existing);
            await _unitOfWork.Commit();
        }

        public async Task<IEnumerable<T>> Get()
        {
            return await _unitOfWork.Context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> Get(Predicate<T> predicate)
        {
            return await _unitOfWork.Context.Set<T>().Where(f=> predicate(f)).ToListAsync();
        }

        public async Task Update(T entity)
        {
            _unitOfWork.Context.Entry(entity).State = EntityState.Modified;
            _unitOfWork.Context.Set<T>().Attach(entity);
            await _unitOfWork.Commit();
        }

    }
}
