﻿using Ardalis.Result;
using MediatR;

namespace QuizMaker.SharedKernel.Messaging;

public interface ICommand : IRequest<Result>, IBaseCommand;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand;

public interface IBaseCommand;
