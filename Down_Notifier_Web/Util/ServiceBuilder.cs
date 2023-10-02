using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Down_Notifier_Web.Data;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Down_Notifier_Web.Models;

namespace Down_Notifier_Web.Util
{
    public static class ServiceBuilder
    {
        private static readonly Dictionary<int, CronJobPingService> Services = new();

        public static void InitServices()
        {
            var context = Startup.GetRequiredService<ApplicationDbContext>();
            var healthChecks = context.HealthChecks.Where(x=>x.IsDeleted==false).ToList();
            foreach (var healthCheck in healthChecks)
            {
                Services.Add(healthCheck.Id, new CronJobPingService(healthCheck));
            }
        }
        public static async Task StartAllAsync(CancellationToken token)
        {

            await Task.WhenAll(Services.Select(a => a.Value.StartAsync(token)));
        }
        public static async Task StopAllAsync(CancellationToken token)
        {
            await Task.WhenAll(Services.Select(a => a.Value.StopAsync(token)));
        }

        public static async Task CreateNewCheck(HealthCheckModel model)
        {
            var cj = new CronJobPingService(model);
            Services.Add(model.Id,cj);
            await cj.StartAsync(CancellationToken.None);
        }

        public static async Task DeleteCheck(HealthCheckModel model)
        {
            var hc = Services[model.Id];
            hc.StopAsync(CancellationToken.None);
            Services.Remove(model.Id);
            hc.Dispose();
        }

        public static async Task UpdateCheck(HealthCheckModel model)
        {
            var hc = Services[model.Id];
            hc.StopAsync(CancellationToken.None);
            Services.Remove(model.Id);
            hc.Dispose();

            var cj = new CronJobPingService(model);
            Services.Add(model.Id, cj);
            await cj.StartAsync(CancellationToken.None);
        }
    }
}
