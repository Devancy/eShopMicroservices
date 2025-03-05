namespace Ordering.Domain.Abstractions;

public abstract class Aggregate<TId> : Entity<TId>, IAggregate<TId> where TId : notnull
{
	private readonly List<IDomainEvent> _domainEvents = [];
	
	public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;

	public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

	public IDomainEvent[] ClearDomainEvent()
	{
		var dequeuedEvents = _domainEvents.ToArray();
		_domainEvents.Clear();
		return dequeuedEvents;
	}
}