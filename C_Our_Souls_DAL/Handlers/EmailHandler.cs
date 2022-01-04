using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace C_Our_Souls_DAL.Handlers
{
    public class EmailHandler
    {
        public string SmtpServer
        {
            get
            {
                return "smtp.gmail.com";
            }
        }
        public string SmtpEmail
        {
            get
            {
                return "bibProjectProgrammerenAgile@gmail.com";
            }
        }
        public string SmtpPassword
        {
            get
            {
                return "hCx%FdJmVNU3LVHEt2w6v838g$NAMn*wA%1";
            }
        }
        public int SmtpSslPort
        {
            get
            {
                return 587;
            }
        }

        public void SendMail(string naamTo, string emailTo, string emailSubject, string emailBody)
        {
            SmtpClient client = new SmtpClient(SmtpServer, SmtpSslPort)
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = true,
                Credentials = new NetworkCredential(SmtpEmail, SmtpPassword),
            };

            MailAddress emailSender = new MailAddress(SmtpEmail, "Bibliotheek");
            MailAddress emailReceiver = new MailAddress(emailTo, naamTo);

            MailMessage message = new MailMessage(emailSender, emailReceiver)
            {
                Subject = emailSubject,
                Body = emailBody,
                IsBodyHtml = true
            };
            client.Send(message);
        }
    }
}
