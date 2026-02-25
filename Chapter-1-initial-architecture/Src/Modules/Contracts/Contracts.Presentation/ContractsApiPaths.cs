namespace Contracts.Presentation;

internal static class ContractsApiPaths
{
    private const string ApiRoot = "api";
    private const string ContractsRootApi = $"{ApiRoot}/contracts";

    internal const string Prepare = ContractsRootApi;
    internal const string Sign = $"{ContractsRootApi}/{{id}}";
}
