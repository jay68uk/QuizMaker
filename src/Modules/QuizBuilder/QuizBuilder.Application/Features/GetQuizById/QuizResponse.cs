using QuizBuilder.Domain.Quiz;

namespace QuizBuilder.Application.Features.GetQuizById;

public sealed record QuizResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string AccessCode { get; init; }
    public DateTimeOffset CreatedDate { get; init; }
    public DateTimeOffset? RanDate { get; init; }
    public Guid CreatedBy { get; init; }
    public QuizStatus Status { get; init; }
    public List<QuestionResponse> Questions { get; init; } = [];
}
