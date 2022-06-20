using Informator.Models;
using System.Net;
using System.Net.Mail;

namespace Informator.SendServices.Email
{
    public class EmailHandler : IHandler
    {
        MailAddress sender;
        MailAddress contact;
        SmtpClient smtpClient;
        MailMessage mailMessage;
        public EmailHandler()
        {
            sender = new MailAddress("", "Informator");
            smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new NetworkCredential("", "");
            smtpClient.EnableSsl = true;
        }
        public void Listener()
        {
            
        }
        public void Send(string message, List<string> mailingList)
        {
            foreach (var item in mailingList)
            {
                contact = new MailAddress(item);
                mailMessage = new MailMessage(sender, contact);
                mailMessage.Body = message;
                mailMessage.IsBodyHtml = true;
                smtpClient.Send(mailMessage);
            }
            
        }
    }
}
