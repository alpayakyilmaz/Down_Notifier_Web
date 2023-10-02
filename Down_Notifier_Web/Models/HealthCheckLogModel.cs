using System;
using System.ComponentModel.DataAnnotations;

namespace Down_Notifier_Web.Models
{
    public class HealthCheckLogModel
    {
        public int Id { get; set; }
        public int HealthCheckId  { get; set; }
        public int StatusCode { get; set; }
        public DateTime CheckDate { get; set; }
        public string ExceptionMessage { get; set; }
        public virtual HealthCheckModel HealthCheck { get; set; }

    }
}
