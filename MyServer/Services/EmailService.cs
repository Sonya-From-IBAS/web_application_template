using Microsoft.Extensions.Options;
using MyServer.Models;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace MyServer.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _settings = new EmailSettings(
                "Leon Application",
                _configuration["Email:UserName"],
                _configuration["Email:UserName"],
                _configuration["Email:Password"],
                "smtp.yandex.ru",
                465);
        }

        public async Task<bool> SendEmailAsync(MailData mailData)
        {
            try
            {
                // Initialize a new instance of the MimeKit.MimeMessage class
                var mail = new MimeMessage();

                #region Sender / Receiver
                // Sender
                mail.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderAddress));
                mail.Sender = new MailboxAddress(_settings.SenderName, _settings.SenderAddress);

                // Receiver
                foreach (string mailAddress in mailData.To)
                    mail.To.Add(MailboxAddress.Parse(mailAddress));

                #endregion

                #region Content

                var body = new BodyBuilder();
                mail.Subject = mailData.Subject;
                body.HtmlBody = mailData.Body;
                mail.Body = body.ToMessageBody();

                #endregion

                #region Send Mail

                using (var client = new SmtpClient())
                {
                    try
                    {
                        await client.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.SslOnConnect);
                        client.AuthenticationMechanisms.Remove("XOAUTH2");
                        client.Authenticate(_settings.UserName, _settings.Password);
                        await client.SendAsync(mail);
                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                    finally
                    {
                        await client.DisconnectAsync(true);
                        client.Dispose();
                    }
                }

                #endregion

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
