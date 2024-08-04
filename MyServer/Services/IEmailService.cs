using MyServer.Models;

namespace MyServer.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(MailData mailData);
    }
}
