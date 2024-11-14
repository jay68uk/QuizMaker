using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace QuizUser.Features.Authentication;

internal static class AuthenticationExtensions
{
  internal static IServiceCollection AddAuthenticationInternal(this IServiceCollection services)
  {
    IEnumerable<string> validIssuers =
      new[]
      {
        "http://quizmaker.identity:8080/realms/quizmaker",
        "http://localhost:18080/realms/quizmaker"
      };

    services.AddAuthentication()
      .AddJwtBearer(options =>
      {
        options.Authority = "http://quizmaker.identity:8080/realms/quizmaker";
        options.MetadataAddress = "http://quizmaker.identity:8080/realms/quizmaker/.well-known/openid-configuration";
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuer = true,
          ValidIssuers = validIssuers,
          ValidateAudience = true,
          ValidAudience = "account"
        };
      });

    services.AddAuthorization();

    services.AddHttpContextAccessor();

    services.ConfigureOptions<JwtBearerConfigureOptions>();

    return services;
  }
}