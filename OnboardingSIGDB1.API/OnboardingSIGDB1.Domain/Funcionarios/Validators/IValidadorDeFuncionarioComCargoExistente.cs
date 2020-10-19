namespace OnboardingSIGDB1.Domain.Funcionarios.Validators
{
    public interface IValidadorDeFuncionarioComCargoExistente
    {
        void Valid(Funcionario funcionario, long cargoId);
    }
}