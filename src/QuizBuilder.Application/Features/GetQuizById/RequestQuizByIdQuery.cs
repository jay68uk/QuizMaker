using QuizMaker.SharedKernel.Messaging;

namespace QuizBuilder.Application.Features.GetQuizById;

public record RequestQuizByIdQuery(Guid Id) : IQuery<QuizResponse>;