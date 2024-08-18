using System.Net;
using FastEndpoints;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using QuizBuilder.Application.Features.GetQuizById;
using QuizBuilder.Application.UnitTests.TestData;
using QuizBuilder.Application.UnitTests.TestDoubles;
using QuizBuilder.Presentation.Features.QuizEndpoints;
using QuizBuilder.Presentation.UnitTests.TestData;
using QuizBuilder.Presentation.UnitTests.TestDoubles;

namespace QuizBuilder.Presentation.UnitTests;

public class GetByIdTests
{
  [Fact]
  public async Task Should_Return200WithQuiz_WhenQuizIdIsFound()
  {
    var quizId = Ulid.NewUlid().ToGuid();
    var expectedQuizResponse = QuizResponseDirectory.QuizResponseDefaultValid(quizId);
    var services = new ServiceCollection();
    var serviceProvider = services
      .AddMediatR(cfg => 
        cfg.RegisterServicesFromAssemblies(typeof(GetQuizByIdQueryHandlerFake).Assembly))
      .BuildServiceProvider();   
    
    var ep = Factory.Create<GetById>(context =>
      context.Request.RouteValues.Add("QuizId", expectedQuizResponse.Id),
      serviceProvider.GetRequiredService<ISender>());

    var quizResponse = await ep.ExecuteAsync(default);
    
    quizResponse.Result.Should().BeOfType<Ok<QuizResponse>>();
    quizResponse.Result.As<Ok<QuizResponse>>().Value.Should().BeEquivalentTo(expectedQuizResponse);
  }
}