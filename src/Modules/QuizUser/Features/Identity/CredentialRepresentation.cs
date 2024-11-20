namespace QuizUser.Features.Identity;

public sealed record CredentialRepresentation(string Type, string Value, bool Temporary);