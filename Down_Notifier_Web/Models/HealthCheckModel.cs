using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Down_Notifier_Web.Models
{
    public class HealthCheckModel
    {
        public int Id { get; set; }
        public string Name { get; set; }       
        [Url]
        public string Url { get; set; }
        public int PeriodType  { get; set; }
        public int Period { get; set; }
        public bool IsDeleted { get; set; }        
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
    }
}
