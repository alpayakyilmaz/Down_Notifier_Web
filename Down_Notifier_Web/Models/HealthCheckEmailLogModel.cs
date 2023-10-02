using System;

namespace Down_Notifier_Web.Models
{
    public class HealthCheckEmailLogModel
    {
        public int Id { get; set; }
        public int HealthCheckId { get; set; }
        public string Url { get; set; }
        public string Email { get; set; }
        public DateTime EmailSendDate { get; set; }
        public string EmailContent { get; set; }
        public string  ExceptionMessage { get; set; }
        public virtual HealthCheckModel HealthCheck { get; set; }
    }
}
