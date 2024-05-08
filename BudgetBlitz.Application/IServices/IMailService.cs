namespace BudgetBlitz.Application.IServices;

public interface IMailService
{
    Task SendEmailAsync(string toEmail, string subject, string content);
}
