namespace FlowTrade.Authentication.Services
{
    public interface IEmailService
    {
        void SendEmail(string receiverEmail, string subject, string body, bool isBodyHtml = false);
    }
}
