using Ardalis.Result;
using MediatR;

namespace QuizMaker.Common.Application.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
