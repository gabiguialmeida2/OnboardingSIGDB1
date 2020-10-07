using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Interfaces.Services
{
    public interface IFuncionarioDeleteService
    {
        Task Delete(long id);
    }
}
