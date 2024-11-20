using Ardalis.Result;
using FakeItEasy;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using QuizMaker.Common.Infrastructure.Logging;
using QuizUser.Abstractions.Identity;
using QuizUser.Abstractions.Users;
using QuizUser.Features.UpdateUserProfile;
using QuizUser.Features.Users;
using QuizUser.Features.Users.DomainEvents;
using QuizUser.UnitTests.TestData;

namespace QuizUser.UnitTests;

public class UpdateUserTests
{
  private readonly IIdentityProviderService _identityProvider;
  private readonly ILoggerAdaptor<UpdateUserCommandHandler> _loggerAdaptor;
  private readonly IUnitOfWork _unitOfWork;
  private readonly IUserRepository _userRepository;
  private readonly UpdateUserCommand _command;
  private readonly UserIdentityProvider _identityUser;
  private readonly User _userEntity;

  public UpdateUserTests()
  {
    _userRepository = Substitute.For<IUserRepository>();
    _identityProvider = Substitute.For<IIdentityProviderService>();
    _unitOfWork = Substitute.For<IUnitOfWork>();
    _loggerAdaptor = Substitute.For<ILoggerAdaptor<UpdateUserCommandHandler>>();
    _command = A.Fake<UpdateUserCommand>();
    _userEntity = UserTestData.DefaultUser();
    _userEntity.ClearDomainEvents();
    _identityUser = new UserIdentityProvider(_userEntity.IdentityId, _command.Email, string.Empty, _command.FirstName,
      _command.LastName);
  }

  [Fact]
  public async Task Should_BeSuccess_WhenUserExists_And_InputIsValid()
  {
    _userRepository.GetAsync(_command.UserId).Returns(_userEntity);
    _unitOfWork.SaveChangesAsync().Returns(1);
    _identityProvider.UpdateUserAsync(_identityUser).Returns(Result.Success());

    var handler = new UpdateUserCommandHandler(_identityProvider, _userRepository, _unitOfWork, _loggerAdaptor);

    var result = await handler.Handle(_command, default);

    var domainEvent = _userEntity.DomainEvents.FirstOrDefault() as UserProfileUpdatedDomainEvent;

    result.IsSuccess.Should().BeTrue();
    _userEntity.FirstName.Should().Be(_command.FirstName);
    _userEntity.LastName.Should().Be(_command.LastName);
    _userEntity.Email.Should().Be(_command.Email);
    _userEntity.DomainEvents.Should().HaveCount(1);
    domainEvent.Should().BeOfType<UserProfileUpdatedDomainEvent>();
    domainEvent!.UserId.Should().Be(_userEntity.Id);
  }

  [Fact]
  public async Task Should_BeConflict_WhenEmailAlreadyExists_InIdentityProvider()
  {
    _userRepository.GetAsync(_command.UserId).Returns(_userEntity);
    _unitOfWork.SaveChangesAsync().Returns(1);
    _identityProvider.UpdateUserAsync(_identityUser).Returns(Result.Conflict());

    var handler = new UpdateUserCommandHandler(_identityProvider, _userRepository, _unitOfWork, _loggerAdaptor);

    var result = await handler.Handle(_command, default);

    result.IsConflict().Should().BeTrue();
    _userEntity.FirstName.Should().NotBe(_command.FirstName);
    _userEntity.LastName.Should().NotBe(_command.LastName);
    _userEntity.Email.Should().NotBe(_command.Email);
    _userEntity.DomainEvents.Should().BeEmpty();
  }

  [Fact]
  public async Task Should_BeError_WhenIdentityProvider_ThrowsException()
  {
    _userRepository.GetAsync(_command.UserId).Returns(_userEntity);
    _unitOfWork.SaveChangesAsync().Returns(1);
    _identityProvider.UpdateUserAsync(_identityUser).Returns(Result.Error());

    var handler = new UpdateUserCommandHandler(_identityProvider, _userRepository, _unitOfWork, _loggerAdaptor);

    var result = await handler.Handle(_command, default);

    result.IsError().Should().BeTrue();
    _userEntity.FirstName.Should().NotBe(_command.FirstName);
    _userEntity.LastName.Should().NotBe(_command.LastName);
    _userEntity.Email.Should().NotBe(_command.Email);
    _userEntity.DomainEvents.Should().BeEmpty();
  }

  [Fact]
  public async Task Should_BeError_WhenRepository_ThrowsException()
  {
    _userRepository.GetAsync(_command.UserId).Returns(_userEntity);
    _unitOfWork.SaveChangesAsync().Throws(new Exception());
    _identityProvider.UpdateUserAsync(_identityUser).Returns(Result.Success());

    var handler = () => new UpdateUserCommandHandler(_identityProvider, _userRepository, _unitOfWork, _loggerAdaptor);

    var result = await handler.Invoke().Handle(_command, default);

    _loggerAdaptor.Received(1).LogError(Arg.Any<Exception>(), Arg.Any<string>());
    result.IsError().Should().BeTrue();
    _userEntity.FirstName.Should().NotBe(_command.FirstName);
    _userEntity.LastName.Should().NotBe(_command.LastName);
    _userEntity.Email.Should().NotBe(_command.Email);
    _userEntity.DomainEvents.Should().BeEmpty();
  }
}