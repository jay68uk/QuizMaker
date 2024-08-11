using Ardalis.Result;
using MediatR;

namespace QuizMaker.SharedKernel.Messaging;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
