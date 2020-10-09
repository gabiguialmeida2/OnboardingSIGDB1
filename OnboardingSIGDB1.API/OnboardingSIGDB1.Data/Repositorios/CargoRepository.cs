using Microsoft.EntityFrameworkCore;
using OnboardingSIGDB1.Domain.Entitys;
using OnboardingSIGDB1.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Data.Repositorios
{
    public class CargoRepository : Repository<Cargo>, ICargoRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public CargoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Cargo>> GetWithIncludes(Predicate<Cargo> predicate)
        {
            return await _unitOfWork
                .Context
                .Cargos
                .Include(f => f.FuncionarioCargos)
                .Where(f => predicate(f)).ToListAsync();
        }
    }
}
