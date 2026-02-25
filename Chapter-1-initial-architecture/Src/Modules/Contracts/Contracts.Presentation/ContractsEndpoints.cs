namespace Contracts.Presentation;

using Contracts.Presentation.SignContract;
using Contracts.Presentation.PrepareContract;
using Microsoft.AspNetCore.Routing;

public static class ContractsEndpoints
{
    public static void MapContracts(this IEndpointRouteBuilder app)
    {
        app.MapPrepareContract();
        app.MapSignContract();
    }
}
