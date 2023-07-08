using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Net;

namespace FlowTrade.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _senderEmail;

        public EmailService(IConfiguration configuration)
        {
            _smtpHost = configuration["Smtp:Host"];
            _smtpPort = int.Parse(configuration["Smtp:Port"]);
            _smtpUsername = configuration["Smtp:Username"];
            _smtpPassword = configuration["Smtp:Password"];
            _senderEmail = configuration["Smtp:SenderEmail"];
        }

        public void SendEmail(string receiverEmail, string subject, string body, bool isBodyHtml = false)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("FlowTrade", _senderEmail));
            message.To.Add(new MailboxAddress(receiverEmail.ToString(), receiverEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = body;
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect(_smtpHost, _smtpPort, SecureSocketOptions.StartTls);
                client.Authenticate(new NetworkCredential(_smtpUsername, _smtpPassword));

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}