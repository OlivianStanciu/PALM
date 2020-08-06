using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PALM.Services
{
    public class MailService : IDisposable
    {
        private SmtpClient _smtpClient;
        public MailService()
        {
            var credential = new NetworkCredential
            {
                UserName = "noreply.artpalmro@gmail.com",  // replace with valid value
                Password = "91@u(XL]0S5mxJGN!}"  // replace with valid value
            };
            _smtpClient = new SmtpClient();
            _smtpClient.Credentials = credential;
            _smtpClient.Host = "smtp.gmail.com";
            _smtpClient.Port = 587;
            _smtpClient.EnableSsl = true;

            _smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
        }

        public void SendEmail(string from, string to, string subject, bool isBodyHtml, string body)
        {
            var message = new MailMessage();

            message.From = new MailAddress(from);
            message.To.Add(new MailAddress(to));

            message.Body = body;
            message.IsBodyHtml = isBodyHtml;

            message.Subject = subject;

            try
            {
                _smtpClient.Send(message);
            }
            catch (Exception e)
            {

            }
            
        }


        public void Dispose()
        {
            _smtpClient.Dispose();
        }
    }
}
