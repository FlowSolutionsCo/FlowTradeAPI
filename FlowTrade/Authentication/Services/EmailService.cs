using Azure.Security.KeyVault.Secrets;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Net;

namespace FlowTrade.Authentication.Services
{
    public class EmailService : IEmailService
    {
        private readonly SecretClient secretClient;

        public EmailService(IConfiguration configuration, [FromServices] SecretClient secretClient)
        {
            this.secretClient = secretClient;           
        }

        public void SendEmail(string receiverEmail, string subject, string body, bool isBodyHtml = false)
        {
            var smtpHost = this.secretClient.GetSecret("FlowTrade-Smtp-Host").Value.Value;
            var smtpPort = int.Parse(this.secretClient.GetSecret("FlowTrade-Smtp-Port").Value.Value);
            var smtpUsername = this.secretClient.GetSecret("FlowTrade-Smtp-Username").Value.Value;
            var smtpPassword = this.secretClient.GetSecret("FlowTrade-Smtp-Password").Value.Value;
            var senderEmail = this.secretClient.GetSecret("FlowTrade-Smtp-Email").Value.Value;

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("FlowTrade", senderEmail));
            message.To.Add(new MailboxAddress(receiverEmail.ToString(), receiverEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = body;
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect(smtpHost, smtpPort, SecureSocketOptions.StartTls);
                client.Authenticate(new NetworkCredential(smtpUsername, smtpPassword));

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}