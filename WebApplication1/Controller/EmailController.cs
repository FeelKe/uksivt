using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    private readonly string _smtpServer = "smtp.mail.ru";
    private readonly int _smtpPort = 465;
    private readonly string _fromAddress = "feelkkq@mail.ru";
    private readonly string _fromPassword = "Us7RN2snA5QdHNADddkV";

    private static readonly Dictionary<string, string> EmailConfirmationCodes = new();

    [HttpPost("send")]
    public async Task<IActionResult> SendEmail([FromBody] EmailRequest emailRequest)
    {
        if (emailRequest == null)
        {
            return BadRequest("Invalid email request");
        }

        try
        {
            var confirmationCode = GenerateConfirmationCode();
            EmailConfirmationCodes[emailRequest.ToAddress] = confirmationCode;

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("MovieApp", _fromAddress));
            message.To.Add(new MailboxAddress("", emailRequest.ToAddress));
            message.Subject = "Код подтверждения регистрации";

            var body = $"<h1>Ваш код подтверждения:</h1><p>{confirmationCode}</p>";
            message.Body = new TextPart("html") { Text = body };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpServer, _smtpPort, true);  
                await client.AuthenticateAsync(_fromAddress, _fromPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

            return Ok("Email sent successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("verify")]
    public IActionResult VerifyCode([FromBody] VerificationRequest verificationRequest)
    {
        if (verificationRequest == null)
        {
            return BadRequest("Invalid verification request");
        }

        if (EmailConfirmationCodes.TryGetValue(verificationRequest.Email, out var storedCode))
        {
            if (storedCode == verificationRequest.Code)
            {
                EmailConfirmationCodes.Remove(verificationRequest.Email);
                return Ok("Code verified successfully");
            }
            return BadRequest("Invalid code");
        }

        return BadRequest("Code not found for this email");
    }

    private string GenerateConfirmationCode()
    {
        var random = new Random();
        return random.Next(10000, 99999).ToString();
    }
}
public class EmailRequest
{
    public string ToAddress { get; set; }
}
public class VerificationRequest
{
    public string Email { get; set; }
    public string Code { get; set; }
}
