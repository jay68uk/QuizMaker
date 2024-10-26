using Ardalis.Result;
using FastEndpoints;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using QuizBuilder.Application.Features.GetQuizById;
using QuizBuilder.Presentation.Features.QuizEndpoints;
using QuizBuilder.Presentation.UnitTests.TestData;

namespace QuizBuilder.Presentation.UnitTests;

public class GetByIdTests
{
  [Fact]
  public async Task Should_Return200WithQuiz_WhenQuizIdIsFound()
  {
    var quizId = Ulid.NewUlid().ToGuid();
    var expectedQuizResponse = QuizResponseDirectory.QuizResponseDefaultValid(quizId);
    
    var isendMock = Substitute.For<ISender>();
    isendMock
        .Send(Arg.Any<RequestQuizByIdQuery>(), Arg.Any<CancellationToken>())
        .Returns(expectedQuizResponse);
   
    var endpoint = Factory.Create<GetById>(context =>
      context.Request.RouteValues.Add("QuizId", quizId),
        isendMock);

    var quizResponse = await endpoint.ExecuteAsync(default);
    
    quizResponse.Result.Should().BeOfType<Ok<QuizResponse>>();
    quizResponse.Result.As<Ok<QuizResponse>>().Value.Should().BeEquivalentTo(expectedQuizResponse);
  }
  
  [Fact]
  public async Task Should_Return404NotFound_WhenQuizIdDoesNotExist()
  {
      var quizId = Ulid.NewUlid().ToGuid();
      var isendMock = Substitute.For<ISender>();
      isendMock
          .Send(Arg.Any<RequestQuizByIdQuery>(), Arg.Any<CancellationToken>())
          .Returns(Result<QuizResponse>.NotFound());
    
      var endpoint = Factory.Create<GetById>(context =>
              context.Request.RouteValues.Add("QuizId", quizId),
          isendMock);

      var quizResponse = await endpoint.ExecuteAsync(default);
    
      quizResponse.Result.Should().BeOfType<NotFound>();
  }

  [Fact]
  public async Task Should_Return400BadRequest_WhenQuizIdIsInvalid()
  {
      var quizId = "01927839-bfaa-1389-13d3-c51feb2052d";
      var isendMock = Substitute.For<ISender>();
      isendMock
          .Send(Arg.Any<RequestQuizByIdQuery>(), Arg.Any<CancellationToken>())
          .Returns(Result<QuizResponse>.NotFound());

      var endpoint = Factory.Create<GetById>(context =>
              context.Request.RouteValues.Add("quizId", quizId),
          isendMock);

      var quizResponse = async () => await endpoint.ExecuteAsync(default);

      await quizResponse.Should().ThrowAsync<ValidationFailureException>();
  }
}
