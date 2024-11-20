namespace QuizUser.Features.Identity;

public sealed class KeyCloakOptions
{
  public string AdminUrl { get; set; }

  public string TokenUrl { get; set; }

  public string ConfidentialClientId { get; init; }

  public string ConfidentialClientSecret { get; init; }

  public string PublicClientId { get; init; }
}