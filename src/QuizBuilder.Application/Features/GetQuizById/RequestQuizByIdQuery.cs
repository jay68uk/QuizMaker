using QuizMaker.SharedKernel.Messaging;

namespace QuizMaker.Application.Features.GetQuizById;

public record RequestQuizByIdQuery(Guid Id) : IQuery<QuizResponse>;