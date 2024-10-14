namespace QuizBuilder.Application.Features.GetQuizById;

public record QuestionResponse(Guid QuestionId, int QuestionNumber, string QuestionDescription);
