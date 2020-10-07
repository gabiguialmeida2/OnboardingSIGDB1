using Microsoft.EntityFrameworkCore;
using OnboardingSIGDB1.Domain.Entitys;
using OnboardingSIGDB1.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Data.Repositorios
{
    public class FuncionarioRepository : Repository<Funcionario>, IFuncionarioRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public FuncionarioRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Funcionario>> GetWithEmpresa(Predicate<Funcionario> predicate)
        {
            return await _unitOfWork
                .Context
                .Funcionarios
                .Include(f => f.Empresa)
                .Where(f => predicate(f)).ToListAsync();
        }
    }
}
