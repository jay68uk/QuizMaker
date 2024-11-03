using QuizMaker.Common.Infrastructure;

namespace QuizMaker.Api.Extensions;

internal static class KeyCloakHealthChecksBuilderExtensions
{

    private const string KeyCloakHealthUrl = "KeyCloak:HealthUrl";



    internal static Uri GetKeyCloakHealthUrl(this IConfiguration configuration)
    {
        return new Uri(configuration.GetValueOrThrow<string>(KeyCloakHealthUrl));
    }
}
