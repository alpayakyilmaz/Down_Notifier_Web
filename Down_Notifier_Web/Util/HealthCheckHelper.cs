using Down_Notifier_Web.Data;
using Down_Notifier_Web.Models;
using Elmah;
using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using System.Threading.Tasks;


namespace Down_Notifier_Web.Util
{
    public static class HealthCheckHelper
    {
        public static async Task CheckURl(HealthCheckModel model )
        {
            if (model != null)
            {
                var context = Startup.GetRequiredService<ApplicationDbContext>();
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("User-Agent", "HealthCheck");
                client.Timeout = TimeSpan.FromSeconds(2);
                try
                {
                    var result = await client.GetAsync(model.Url);
                    Console.WriteLine(result.StatusCode);
                   
                    context.HealthCheckLogs.Add(new HealthCheckLogModel { HealthCheckId = model.Id, StatusCode = result.StatusCode.GetHashCode(), CheckDate = DateTime.Now });
                    context.SaveChanges();
                    if (result.StatusCode.GetHashCode() < 200 || result.StatusCode.GetHashCode() > 299)
                    {
                        PingMessageSend(model, result.StatusCode.GetHashCode());
                    }
                }
                catch (Exception ex)
                {
                    ex.Data.Add("SendEmailError", model);
                    context.HealthCheckLogs.Add(new HealthCheckLogModel { HealthCheckId = model.Id, StatusCode = 0, CheckDate = DateTime.Now, ExceptionMessage=ex.Message });
                    context.SaveChanges();
                    PingMessageSend(model, 0);
                }
                

               
            }
        }

        public static async Task PingMessageSend(HealthCheckModel model, int statusCode)
        {
            var context = Startup.GetRequiredService<ApplicationDbContext>();
            string subject= "";
            string body = "";
            try
            {
                var emailservice = Startup.GetRequiredService<EmailService>();
                subject = model.Url + "Status Code Problem";
                body = model.Name + " " + model.Url + " Status Code :" + statusCode.ToString();
                emailservice.SendEmail(model.Email, subject, body);
               
                context.HealthCheckMailLogs.Add(new HealthCheckEmailLogModel { HealthCheckId = model.Id, Email = model.Email, EmailContent = body, EmailSendDate = DateTime.Now, Url = model.Url });
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                context.HealthCheckMailLogs.Add(new HealthCheckEmailLogModel { HealthCheckId = model.Id, Email = model.Email, EmailContent = body, EmailSendDate = DateTime.Now, Url = model.Url, ExceptionMessage = ex.Message });
                context.SaveChanges();
              
            }
          

        }
    }
}
