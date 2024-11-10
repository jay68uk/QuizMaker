using Microsoft.Extensions.DependencyInjection;

namespace QuizMaker.Common.Infrastructure.Authorisation;

public static class AuthorisationPolicies
{
  public static IServiceCollection AddAuthorisationPolicies(this IServiceCollection services)
  {
    const string permissionClaim = "Permission";

    services.AddAuthorizationBuilder()
      .AddPolicy(PolicyNames.MembersProfileGet, x =>
        x.RequireAssertion(a => a.User.HasClaim(permissionClaim, Permission.GetUser.Code)));

    services.AddAuthorizationBuilder()
      .AddPolicy(PolicyNames.MembersProfileUpdate, x =>
        x.RequireAssertion(a => a.User.HasClaim(permissionClaim, Permission.ModifyUser.Code)));

    services.AddAuthorizationBuilder()
      .AddPolicy(PolicyNames.QuizGet, x =>
        x.RequireAssertion(a => a.User.HasClaim(permissionClaim, Permission.GetQuizzes.Code)));

    return services;
  }
}