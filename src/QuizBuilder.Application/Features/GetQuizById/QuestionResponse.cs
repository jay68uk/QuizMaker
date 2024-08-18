namespace QuizBuilder.Application.Features.GetQuizById;

public record QuestionResponse(Guid Id, string Description, int Number);