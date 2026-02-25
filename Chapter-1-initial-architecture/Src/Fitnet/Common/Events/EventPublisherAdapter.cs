namespace EvolutionaryArchitecture.Fitnet.Common.Events;

using global::Contracts.Application.Common;
using MediatR;

internal sealed class EventPublisherAdapter(IMediator mediator) : IEventPublisher
{
    async Task IEventPublisher.PublishAsync<TEvent>(TEvent integrationEvent, CancellationToken cancellationToken) =>
        await mediator.Publish(integrationEvent, cancellationToken);
}
