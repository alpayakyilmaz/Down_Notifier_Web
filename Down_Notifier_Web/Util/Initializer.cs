using System.Threading;
using System.Threading.Tasks;

namespace Down_Notifier_Web.Util
{
    public class Initializer
    {
        public async Task InitializePingServices()
        {
            ServiceBuilder.InitServices();
            await ServiceBuilder.StartAllAsync(CancellationToken.None);
        }
    }
}
