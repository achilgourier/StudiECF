using System.Net;
using System.Net.Mail;

namespace EFCProject
{
    public class EmailSender:IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = "frgamesacc@outlook.com";
            var password = "GsDj5tgFQ3FKWgn";

            var client = new SmtpClient("smtp-mail.outlook.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)

            };
            return client.SendMailAsync(
             new MailMessage(from: mail, to: email, subject: subject, message)
             );
        
        }

    }
}
