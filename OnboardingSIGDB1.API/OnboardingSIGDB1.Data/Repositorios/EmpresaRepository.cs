using Microsoft.EntityFrameworkCore;
using OnboardingSIGDB1.Domain.Entitys;
using OnboardingSIGDB1.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Data.Repositorios
{
    public class EmpresaRepository : Repository<Empresa>, IEmpresaRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmpresaRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Empresa>> GetWithFuncionarios(Predicate<Empresa> predicate)
        {
            return await _unitOfWork
                .Context
                .Empresas
                .Include(f => f.Funcionarios)
                .Where(f => predicate(f)).ToListAsync();
        }
    }
}

