using OnboardingSIGDB1.Domain.Entitys;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Interfaces.Services
{
    public interface IEmpresaService
    {
        Task InsertEmpresa(Empresa empresa);
        Task UpdateEmpresa(long id, Empresa empresa);
    }
}
