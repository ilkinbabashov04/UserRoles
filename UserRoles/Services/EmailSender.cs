using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserRoles.Data;

public class EmailSender
{
    private readonly AppDbContext _dbContext;

    public EmailSender(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        // 🔹 Retrieve user credentials from the database
        var user = await _dbContext.Users
            .Where(u => u.Email == toEmail)
            .Select(u => new { u.Email, u.AppPassword })
            .FirstOrDefaultAsync();

        if (user == null)
            throw new Exception("User email not found in the database.");

        if (string.IsNullOrEmpty(user.AppPassword))
            throw new Exception("No SMTP password found for this user.");

        using (var smtpClient = new SmtpClient("smtp.gmail.com"))
        {
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential(user.Email, user.AppPassword);
            smtpClient.EnableSsl = true;

            var mailMessage = new MailMessage
            {
                From = new MailAddress(user.Email),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
