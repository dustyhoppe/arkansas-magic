using ArkansasMagic.Core.Wizards.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ArkansasMagic.Core.Wizards
{
    public interface IWizardsApiClient : IDisposable
    {
        Task<Organization> GetOrganizationAsync(int id, CancellationToken cancellationToken = default);
        Task<EventDetail> GetEventDetailsAsync(int id, CancellationToken cancellationToken = default);
        Task<EventSearchResponse> SearchEventsAsync(EventSearchQuery query, CancellationToken cancellationToken = default);
    }
}
