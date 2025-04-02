using System.Net;
using System.Net.Mail;
using FluentEmail.Core;
using MassTransit.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Modules.Users.Application.Services;
using Modules.Users.Infrastructure.Options;

namespace Modules.Users.Infrastructure.Services;

public class EmailService(IConfiguration configuration, IOptions<GmailSmtpOptions> options) : IEmailService
{
    private async Task sendMail(
        string to,
        string subject,
        string body)
    {
        GmailSmtpOptions gmailSmtp = options.Value;
        SmtpClient smtpClient = new SmtpClient(gmailSmtp.SmtpServer, gmailSmtp.Port)
        {
            EnableSsl = gmailSmtp.SSL,
            Credentials = new NetworkCredential(gmailSmtp.SenderEmail, gmailSmtp.SenderPassword)
        };
        await smtpClient.SendMailAsync(new MailMessage(
            from: gmailSmtp.SenderEmail, to: to, subject: subject, body: body));
    }

    public async Task SendVerificationToken(string FirstName, string Email, string Token)
    {
        string bodyTemplate = configuration.GetValue<string>("Users:EmailTemplates:VerificationMail:body")!;
        string subject = configuration.GetValue<string>("Users:EmailTemplates:VerificationMail:subject")!;
        await sendMail(
            Email,
            subject,
            FillTemplate(bodyTemplate, new Dictionary<string, string> {
                { "[FirstName]" , FirstName },
                { "[URL]" , $"http://localhost:5185/api/auth/email/{Token}"}
            }));
    }

    private static string FillTemplate(string template, Dictionary<string, string> placeholders)
    {
        foreach (var entry in placeholders)
        {
            template = template.Replace($"{entry.Key}", entry.Value);
        }
        return template;
    }

}
