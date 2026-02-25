namespace Contracts.Domain.BusinessRules;

using Contracts.Domain.Common;

internal sealed class ContractCanBePreparedOnlyForAdultRule : IBusinessRule
{
    private readonly int _age;

    internal ContractCanBePreparedOnlyForAdultRule(int age) => _age = age;

    public bool IsMet() => _age >= 18;

    public string Error => "Contract can not be prepared for a person who is not adult";
}
