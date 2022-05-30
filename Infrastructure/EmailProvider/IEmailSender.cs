namespace Infrastructure.EmailProvider
{
    public interface IEmailSender
    {
        void Send(string to, string subject, string html);
    }
}