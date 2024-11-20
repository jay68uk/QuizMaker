namespace QuizMaker.Common.Domain;

public abstract class DomainEvent(Guid id, DateTimeOffset occurredOnUtc, DomainEventStatus status)
  : IDomainEvent
{
  protected DomainEvent() : this(Guid.NewGuid(), DateTime.UtcNow, DomainEventStatus.Active)
  {
  }

  public DomainEventStatus Status { get; private set; } = status;

  public Guid Id { get; init; } = id;

  public DateTimeOffset OccurredOnUtc { get; init; } = occurredOnUtc;

  public void UpdateDomainEventStatus(DomainEventStatus status)
  {
    Status = status;
  }
}

public enum DomainEventStatus
{
  Active,
  Pending,
  Cancelled
}