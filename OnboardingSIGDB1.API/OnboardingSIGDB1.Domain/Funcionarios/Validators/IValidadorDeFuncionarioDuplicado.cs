using System.Threading.Tasks;

namespace OnboardingSIGDB1.Domain.Funcionarios.Validators
{
    public interface IValidadorDeFuncionarioDuplicado
    {
        Task Valid(Funcionario funcionario);
    }
}