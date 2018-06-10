using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace FinalDiploma.Utils
{
    public class Mail
    {
        public async static Task MailSender(string NameFrom, string Message, string Subject, List<string> Mails)
        {
            MailAddress from = new MailAddress("restorauntpromo@gmail.com", NameFrom);
            //MailAddress to = new MailAddress(String.Join(",", Mails.ToArray()));
            MailMessage m = new MailMessage();
            m.From = from;
            foreach( string CurrentMail in Mails)
            {
                m.To.Add(CurrentMail);
            }
            m.Subject = Subject;
            m.Body = Message;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("restorauntpromo@gmail.com", "restarauntPromo1");
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(m);
        }
        public  static void SendClientsNewDish(string NameDish)
        {

        }
    }
}