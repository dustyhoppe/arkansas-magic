using ArkansasMagic.Core.Alerts.Types;
using System.Threading;
using System.Threading.Tasks;

namespace ArkansasMagic.Core.Alerts
{
    public interface IDeliveryProvider
    {
        Task AlertAsync(IAlert alert, CancellationToken cancellationToken = default);
    }
}
