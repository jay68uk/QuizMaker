using System.ComponentModel.DataAnnotations.Schema;

namespace QuizMaker.Common.Domain;

public abstract class Entity
{
  private readonly List<IDomainEvent> _domainEvents = [];

  protected Entity(Guid id)
  {
    Id = id;
  }

  protected Entity()
  {
  }

  public Guid Id { get; init; }

  [NotMapped] public List<IDomainEvent> DomainEvents => _domainEvents.ToList();

  public void ClearDomainEvents()
  {
    _domainEvents.Clear();
  }

  protected void Raise(IDomainEvent domainEvent)
  {
    _domainEvents.Add(domainEvent);
  }
}