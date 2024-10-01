using Ardalis.Result;
using MediatR;

namespace QuizMaker.Common.Application.Messaging;

public interface ITransactionalCommand : ICommand;

public interface ITransactionalCommand<TResponse> : IRequest<Result<TResponse>>, ITransactionalCommand;
