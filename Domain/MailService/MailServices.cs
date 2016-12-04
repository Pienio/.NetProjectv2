using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MailService
{
    public class MailServices
    {
        SmtpClient client= new SmtpClient();

        public MailServices()
        {
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("sysrejwiz@gmail.com", "qwerasdfzxcv");
        }
        public void SendSimpleMail()
        {
            //SmtpClient client = new SmtpClient();
           

            var appDomain = System.AppDomain.CurrentDomain;
            var basePath = appDomain.RelativeSearchPath ?? appDomain.BaseDirectory;
           
            string Body = System.IO.File.ReadAllText(Path.Combine(basePath,"index.html"));
            Body = Body.Replace("#Header#", "Test");
            Body = Body.Replace("#SubHeader#", "Próbna Wiadomość");
            Body = Body.Replace("#Opis#", "Czyżbyś właśnie otrzymał próbną wiadomość?");

            MailMessage mm = new MailMessage("sysrejwiz@gmail.com", "filip.maruszczak@o2.pl", "test", Body);
            mm.IsBodyHtml = true;
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            client.Send(mm);
        }
    }
}
