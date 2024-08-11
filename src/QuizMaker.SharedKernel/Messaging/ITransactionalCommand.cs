using Ardalis.Result;
using MediatR;

namespace QuizMaker.SharedKernel.Messaging;

public interface ITransactionalCommand : ICommand;

public interface ITransactionalCommand<TResponse> : IRequest<Result<TResponse>>, ITransactionalCommand;
