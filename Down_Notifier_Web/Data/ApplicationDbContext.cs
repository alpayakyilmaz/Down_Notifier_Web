using Down_Notifier_Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Down_Notifier_Web.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<HealthCheckModel> HealthChecks { get; set; }
        public DbSet<HealthCheckLogModel> HealthCheckLogs { get; set; }
        public DbSet<HealthCheckEmailLogModel> HealthCheckMailLogs { get; set; }

    }
}
