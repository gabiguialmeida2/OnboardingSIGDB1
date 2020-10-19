using OnboardingSIGDB1.Domain.Cargos;
using OnboardingSIGDB1.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Interfaces
{
    public interface ICargoRepository : IRepository<Cargo>
    {
        Task<IEnumerable<Cargo>> GetWithIncludes(Predicate<Cargo> predicate);
    }
}

