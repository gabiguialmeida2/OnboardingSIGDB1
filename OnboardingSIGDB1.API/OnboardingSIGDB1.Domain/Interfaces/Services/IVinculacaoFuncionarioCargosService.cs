using System;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Interfaces.Services
{
    public interface IVinculacaoFuncionarioCargosService
    {
        Task Vincular(long funcionarioId, long cargoId, DateTime dataVinculacao);
    }
}
