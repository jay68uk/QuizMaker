using QuizMaker.Common.Domain;

namespace QuizBuilder.Domain.Question;

public sealed class Question : Entity, IEquatable<Question>
{
    public Description Description { get; private set; }
    public QuestionNumber Number { get; private set; }

    private Question(Guid id, Description description, QuestionNumber number) : base(id)
    {
        Description = description;
        Number = number;
    }

    public static Question Create(Description description, QuestionNumber number)
    {
        var question = new Question(Ulid.NewUlid().ToGuid(), description, number);

        return question;
    }

    public void UpdateNumbering(QuestionNumber newNumber)
    {
        Number = newNumber;
    }

    public bool Equals(Question? other)
    {
        return Description.Value == other?.Description.Value && Number.Value == other?.Number.Value;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Question);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Description, Number);
    }

    public static bool operator ==(Question? left, Question? right) => Equals(left, right);

    public static bool operator !=(Question? left, Question? right) => !Equals(left, right);
}
