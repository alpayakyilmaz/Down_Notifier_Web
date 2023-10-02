using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Down_Notifier_Web.Util
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(string to, string subject, string body)
        {
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var port = int.Parse(_configuration["EmailSettings:Port"]);
            var userName = _configuration["EmailSettings:UserName"];
            var password = _configuration["EmailSettings:Password"];
            var fromAddress = _configuration["EmailSettings:FromAddress"];
            var fromName = _configuration["EmailSettings:FromName"];

            using (var client = new SmtpClient(smtpServer))
            {
                client.Port = port;
                client.Credentials = new NetworkCredential(userName, password);
                client.EnableSsl = true;

                using (var message = new MailMessage(new MailAddress(fromAddress, fromName), new MailAddress(to)))
                {
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;

                    try
                    {
                        client.Send(message);
                    }
                    catch (Exception ex)
                    {
                        // Handle any email sending exceptions.

                        throw ex;
                    }
                }
            }
        }
    }
}
