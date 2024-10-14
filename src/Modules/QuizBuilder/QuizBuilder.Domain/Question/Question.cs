using QuizMaker.Common.Domain;

namespace QuizBuilder.Domain.Question;

public sealed class Question : Entity, IEquatable<Question>, ISoftDeletable
{
    public Description Description { get; private set; }
    public QuestionNumber Number { get; private set; }
    public Guid QuizId { get; }
    public bool IsDeleted { get; private set;}
    public DateTimeOffset? DeletedDate { get; private set;}

    private Question(Guid id, Description description, QuestionNumber number, Guid quizId) : base(id)
    {
        Description = description;
        Number = number;
        QuizId = quizId;
    }

    public static Question Create(Description description, QuestionNumber number, Guid quizId)
    {
        var question = new Question(Ulid.NewUlid().ToGuid(), description, number, quizId);

        return question;
    }

    public void UpdateNumbering(QuestionNumber newNumber)
    {
        Number = newNumber;
    }

    public bool Equals(Question? other)
    {
        return Description.Value == other?.Description.Value 
               && Number.Value == other?.Number.Value
               && QuizId == other.QuizId;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Question);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Description, Number, QuizId);
    }

    public static bool operator ==(Question? left, Question? right) => Equals(left, right);

    public static bool operator !=(Question? left, Question? right) => !Equals(left, right);

    public void UpdateDescription(string updatesQuestionDescription)
    {
        Description = new Description(updatesQuestionDescription);
    }
    
    public void SoftDelete(DateTimeOffset dateDeleted)
    {
        IsDeleted = true;
        DeletedDate = dateDeleted.ToUniversalTime();
    }
   
    public void UndoDelete()
    {
        IsDeleted = false;
        DeletedDate = null;
    }
}
