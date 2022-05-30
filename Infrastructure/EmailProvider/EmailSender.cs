using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace Infrastructure.EmailProvider
{
    public class EmailSender : IEmailSender
    {
        public void Send(string to, string subject, string html)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("epti.jwt.test@outlook.com"));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };
            
            using var smtp = new SmtpClient();
            smtp.Connect("smtp-mail.outlook.com", 587);
            smtp.Authenticate("epti.jwt.test@outlook.com", "Epti@12345");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}