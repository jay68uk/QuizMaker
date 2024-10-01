using QuizMaker.Common.Application.Messaging;

namespace QuizBuilder.Application.Features.GetQuizById;

public record RequestQuizByIdQuery(Guid Id) : IQuery<QuizResponse>;