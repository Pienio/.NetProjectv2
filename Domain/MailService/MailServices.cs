﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace MailService
{
    public class MailServices
    {
        public MailServices()
        {
        }

        public void SendSimpleMail()
        {
            SendMail("filip.maruszczak@o2.pl", "test", "Test", "subtest", "testest");
        }

        public void SendRegistrationConfirmation(string mailAddress, string token)
        {
            string content = "Witamy w Systemie rezerwacji wizyt.\n" +
                "Aby potwierdzić rejestrację, wpisz w wymaganym polu dany token: " + token;
            SendMail(mailAddress, "Witamy w Systemie rezerwacji wizyt", "System rezerwacji wizyt", "Potwierdzenie rejestracji", content);
        }

        public void SendMailChangeConfirmation(string mailAddress, string token)
        {
            string content = "Aby potwierdzić zmianę adresu e-mail, wpisz w wymaganym polu dany token: " + token;
            SendMail(mailAddress, "Zmiana adresu e-mail", "System rezerwacji wizyt", "Potwierdzenie zmiany adresu e-mail", content);
        }

        private void SendMail(string address, string title, string header, string subheader, string description)
        {
            using (var client = CreateClient())
            {
                ResourceManager mng = new ResourceManager(typeof(Properties.Resources));
                string Body = mng.GetString("index");
                Body = Body.Replace("#Header#", header);
                Body = Body.Replace("#SubHeader#", subheader);
                Body = Body.Replace("#Opis#", description);

                using (MailMessage mm = new MailMessage("sysrejwiz@gmail.com", address, title, Body))
                {
                    mm.IsBodyHtml = true;
                    mm.BodyEncoding = UTF8Encoding.UTF8;
                    mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                    client.Send(mm);
                }
            }
        }
            
        private SmtpClient CreateClient()
        {
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("sysrejwiz@gmail.com", "qwerasdfzxcv");
            return client;
        }
    }
}