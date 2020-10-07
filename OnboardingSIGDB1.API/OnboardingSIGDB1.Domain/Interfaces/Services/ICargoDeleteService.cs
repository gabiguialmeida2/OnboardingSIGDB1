using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Interfaces.Services
{
    public interface ICargoDeleteService
    {
        Task Delete(long id);
    }
}
