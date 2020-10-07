using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Interfaces.Services
{
    public interface IVinculacaoFuncionarioEmpresaService
    {
        Task Vincular(long funcionarioId, long empresaId);
    }
}
