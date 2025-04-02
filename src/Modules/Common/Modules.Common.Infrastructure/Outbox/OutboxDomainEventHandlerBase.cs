namespace Modules.Common.Infrastructure.Outbox;

public abstract class OutboxDomainEventHandlerBase<TDomainEvent>(
    INotificationHandler<TDomainEvent> innerHandler,
    ILogger<OutboxDomainEventHandlerBase<TDomainEvent>> logger)
    : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    public async Task Handle(TDomainEvent notification, CancellationToken cancellationToken)
    {
        if (!AssemblyHelper<TDomainEvent, OutboxDomainEventHandlerBase<TDomainEvent>>.SameModule())
        {
            logger.LogInformation("Skipping handling for {EventType} as it belongs to a different module.", typeof(TDomainEvent).Name);
            await innerHandler.Handle(notification, cancellationToken);
            return;
        }

        logger.LogInformation("Handling event {EventType} in Outbox.", typeof(TDomainEvent).Name);
        await HandleEvent(notification, cancellationToken);
    }

    protected abstract Task HandleEvent(TDomainEvent notification, CancellationToken cancellationToken);
}