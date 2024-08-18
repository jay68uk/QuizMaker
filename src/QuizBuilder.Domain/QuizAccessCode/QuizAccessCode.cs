﻿namespace QuizBuilder.Domain.QuizAccessCode;

public sealed class QuizAccessCode
{
  public string AccessCode { get; private set; }
  public string QrCode { get; private set; }

  private QuizAccessCode(string accessCode, string qrCode)
  {
    AccessCode = accessCode;
    QrCode = qrCode;
  }
  
  public static async Task<QuizAccessCode> Create()
  {
    var code = Ulid.NewUlid().ToGuid().ToString();
    await Task.Delay(1000); //TODO Generate QR code
    return new QuizAccessCode(code, code);
  }
}