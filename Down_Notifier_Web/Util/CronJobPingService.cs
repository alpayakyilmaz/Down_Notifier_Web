using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;
using System;
using Down_Notifier_Web.Models;
using Microsoft.Win32;

namespace Down_Notifier_Web.Util
{
    public class CronJobPingService : IHostedService, IDisposable
    {
        private System.Timers.Timer _timer;
        private readonly DateTimeOffset _scheduleDate;
        private HealthCheckModel _healthCheckModel;

        public CronJobPingService(HealthCheckModel healthCheckModel)
        {
            _healthCheckModel = healthCheckModel;
        }

        public virtual async Task StartAsync(CancellationToken token)
        {
            await ScheduleJobOne(token);
        }

        private async Task ScheduleJobOne(CancellationToken token)
        {
            double delay = 0;
            delay = _healthCheckModel.PeriodType switch
            {
                1 => _healthCheckModel.Period * 1000,
                2 => _healthCheckModel.Period * 1000 * 60,
                3 => _healthCheckModel.Period * 1000 * 60 * 60,
                _ => 1000
            };
            


            _timer = new System.Timers.Timer(delay);
            async void OnTimerOnElapsed(object o, ElapsedEventArgs elapsedEventArgs)
            {
                if (!token.IsCancellationRequested) await DoWork(token);
            }
            _timer.Elapsed += OnTimerOnElapsed;
            _timer.Start();
            
            await Task.CompletedTask;
        }

        public virtual async Task DoWork(CancellationToken token)
        {
            await HealthCheckHelper.CheckURl(_healthCheckModel);
        }
        public virtual async Task StopAsync(CancellationToken token)
        {
            _timer?.Stop();
            var sc = CancellationTokenSource.CreateLinkedTokenSource(token);
            sc.Cancel();
            await Task.CompletedTask;
        }
        public virtual void Dispose() { _timer?.Dispose(); }
    }
}
