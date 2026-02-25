using Contracts.Application;
using Contracts.Application.PrepareContract;
using Contracts.Application.SignContract;
using Contracts.Infrastructure.Persistence;
using Contracts.Infrastructure.Repositories;
using EvolutionaryArchitecture.Fitnet.Common.Clock;
using EvolutionaryArchitecture.Fitnet.Common.Documentation;
using EvolutionaryArchitecture.Fitnet.Common.ErrorHandling;
using EvolutionaryArchitecture.Fitnet.Common.Events;
using EvolutionaryArchitecture.Fitnet.Common.Events.EventBus;
using EvolutionaryArchitecture.Fitnet.Common.Validation.Requests;
using EvolutionaryArchitecture.Fitnet.Offers;
using EvolutionaryArchitecture.Fitnet.Passes;
using EvolutionaryArchitecture.Fitnet.Reports;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddExceptionHandling();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddEventBus();
builder.Services.AddRequestsValidations();
builder.Services.AddClock();

builder.Services.AddPasses(builder.Configuration);

var contractsConnectionString = builder.Configuration.GetConnectionString("Contracts")
    ?? throw new InvalidOperationException("Contracts connection string not found");
builder.Services.AddDbContext<ContractsPersistence>(options =>
    options.UseNpgsql(contractsConnectionString));
builder.Services.AddScoped<IContractsRepository, ContractsRepository>();
builder.Services.AddScoped<IValidator<PrepareContractRequest>, PrepareContractRequestValidator>();
builder.Services.AddScoped<IValidator<SignContractRequest>, SignContractRequestValidator>();
builder.Services.AddScoped<Contracts.Application.Common.IEventPublisher, EventPublisherAdapter>();
builder.Services.AddScoped<IContractsService, ContractsService>();

builder.Services.AddOffers(builder.Configuration);
builder.Services.AddReports(builder.Configuration);

await using var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseApiDocumentation();
app.UsePasses();
app.UseReports();
app.UseOffers();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseErrorHandling();

app.MapControllers();

app.MapPasses();
Contracts.Presentation.ContractsEndpoints.MapContracts(app);
app.MapReports();

await app.RunAsync();

namespace EvolutionaryArchitecture.Fitnet
{
    [UsedImplicitly]
    public sealed class Program;
}
