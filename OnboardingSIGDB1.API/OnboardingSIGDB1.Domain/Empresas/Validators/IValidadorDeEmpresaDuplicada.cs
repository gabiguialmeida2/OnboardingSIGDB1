using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Empresas.Validators
{
    public interface IValidadorDeEmpresaDuplicada
    {
        Task Valid(Empresa empresa);
    }
}