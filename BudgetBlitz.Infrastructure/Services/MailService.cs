using SendGrid.Helpers.Mail;
using SendGrid;
using Microsoft.Extensions.Configuration;
using BudgetBlitz.Application.IServices;

namespace BudgetBlitz.Infrastructure.Services;

public class MailService(IConfiguration configuration) : IMailService
{
    private readonly IConfiguration _configuration = configuration;
    public async Task SendEmailAsync(string toEmail, string subject, string content)
    {
        var apiKey = _configuration["SendGridAPIKey"];

        var client = new SendGridClient(apiKey);

        var from = new EmailAddress("test@example.com", "BudgetBlitz");

        var to = new EmailAddress(toEmail);

        var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);

        var response = await client.SendEmailAsync(msg);
    }
}
