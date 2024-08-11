using Ardalis.Result;
using MediatR;

namespace QuizMaker.SharedKernel.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
