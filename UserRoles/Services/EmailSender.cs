using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

public class EmailSender
{
    private readonly IConfiguration _configuration;

    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var emailSettings = _configuration.GetSection("EmailSettings");

        var smtpClient = new SmtpClient(emailSettings["SmtpServer"])
        {
            Port = int.Parse(emailSettings["Port"]),
            Credentials = new NetworkCredential(emailSettings["Username"], emailSettings["Password"]),
            EnableSsl = bool.Parse(emailSettings["EnableSSL"]),
            DeliveryMethod = SmtpDeliveryMethod.Network
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(emailSettings["Username"]),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mailMessage.To.Add(toEmail);

        await smtpClient.SendMailAsync(mailMessage);
    }
}
