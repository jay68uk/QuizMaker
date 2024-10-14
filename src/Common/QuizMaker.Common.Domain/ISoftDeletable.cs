namespace QuizMaker.Common.Domain;

public interface ISoftDeletable
{
    bool IsDeleted { get; }
    DateTimeOffset? DeletedDate { get; }
    void SoftDelete(DateTimeOffset dateDeleted);
}
