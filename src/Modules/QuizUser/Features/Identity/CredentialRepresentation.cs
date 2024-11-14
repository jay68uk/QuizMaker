namespace QuizUser.Features.Identity;

internal sealed record CredentialRepresentation(string Type, string Value, bool Temporary);