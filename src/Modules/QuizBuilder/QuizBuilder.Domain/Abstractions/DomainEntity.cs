using QuizMaker.Common.Domain;

namespace QuizBuilder.Domain.Abstractions;

public abstract class DomainEntity : Entity
{
  protected DomainEntity(Guid id) : base(id)
  {
  }

  protected DomainEntity()
  {
  }
}