using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using System.Web.UI.WebControls;
using System.Collections.Specialized;

namespace OrgCommunication.Helpers
{
    public class MailSender
    {
        public static string Host { get; set; }
        public static int Port { get; set; }
        public static bool EnableSsl { get; set; }
        public static string Username { get; set; }
        public static string Password { get; set; }

        public static void Send(string fromEmail, string[] toEmails, string subject, string body)
        {
            if (toEmails == null || toEmails.Length == 0)
                throw new Exception("Invalid recipient");

            var message = new MailMessage();

            message.From = new MailAddress(fromEmail);
            foreach (string recipient in toEmails)
            {
                message.To.Add(new MailAddress(recipient));
            }


            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient(MailSender.Host, MailSender.Port))
            {
                if (!String.IsNullOrWhiteSpace(Username) && !String.IsNullOrWhiteSpace(Password))
                {
                    smtp.UseDefaultCredentials = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                    var credential = new NetworkCredential
                    {
                        UserName = MailSender.Username, 
                        Password = MailSender.Password
                    };

                    smtp.Credentials = credential;
                }

                smtp.EnableSsl = MailSender.EnableSsl;

                smtp.Send(message);
            }
        }

        public static void Send(string fromEmail, string toEmail, string subject, ListDictionary listReplacement, string templateFilePath)
        {
            if (String.IsNullOrWhiteSpace(toEmail))
                throw new Exception("Invalid recipient");

            MailDefinition md = new MailDefinition();

            md.BodyFileName = templateFilePath;
            md.From = fromEmail;
            md.IsBodyHtml = true;
            md.Subject = subject;

            var message = md.CreateMailMessage(toEmail, listReplacement, new System.Web.UI.Control());

            using (var smtp = new SmtpClient(MailSender.Host, MailSender.Port))
            {
                if (!String.IsNullOrWhiteSpace(Username) && !String.IsNullOrWhiteSpace(Password))
                {
                    smtp.UseDefaultCredentials = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                    var credential = new NetworkCredential
                    {
                        UserName = MailSender.Username,
                        Password = MailSender.Password
                    };

                    smtp.Credentials = credential;
                }

                smtp.EnableSsl = MailSender.EnableSsl;

                smtp.Send(message);
            }
        }
    }
}