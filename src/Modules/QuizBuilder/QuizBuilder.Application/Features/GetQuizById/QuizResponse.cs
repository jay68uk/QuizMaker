using QuizBuilder.Domain.Quiz;

namespace QuizBuilder.Application.Features.GetQuizById;

public sealed record QuizResponse(
  Guid Id,
  IEnumerable<QuestionResponse> Questions,
  string Name,
  string AccessCode,
  string QrCode,
  DateTimeOffset CreatedDate,
  DateTimeOffset? RanDate,
  Guid CreatedBy,
  QuizStatus Status);
