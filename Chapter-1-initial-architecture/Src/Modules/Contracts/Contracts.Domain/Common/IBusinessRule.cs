namespace Contracts.Domain.Common;

internal interface IBusinessRule
{
    bool IsMet();
    string Error { get; }
}
