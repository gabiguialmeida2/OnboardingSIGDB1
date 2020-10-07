using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Interfaces.Services
{
    public interface IEmpresaDeleteService
    {
        Task Delete(long id);
    }
}
