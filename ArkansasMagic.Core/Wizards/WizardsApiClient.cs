using ArkansasMagic.Core.Wizards.Models;
using ArkansasMagic.Domain.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ArkansasMagic.Core.Wizards
{
    public class WizardsApiClient : IWizardsApiClient
    {
        private readonly string _baseAddress;
        private readonly HttpClient _client;

        public WizardsApiClient()
        {
            _baseAddress = "https://api.tabletop.wizards.com";
            _client = new HttpClient()
            {
                BaseAddress = new Uri(_baseAddress)
            };
        }

        public async Task<Organization> GetOrganizationAsync(int id, CancellationToken cancellationToken = default)
        {
            var url = $"/ogre-battledriver/Organizations/by-ids?ids={id}";
            var (response, organizations) = await _client.GetAsJsonWithResponseAsync<List<Organization>>(url, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            return organizations.FirstOrDefault();
        }

        public async Task<EventDetail> GetEventDetailsAsync(int id, CancellationToken cancellationToken = default)
        {
            var url = $"event-reservations-service/events/{id}";
            var (response, @event) = await _client.GetAsJsonWithResponseAsync<EventDetail>(url, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            return @event;
        }

        public async Task<EventSearchResponse> SearchEventsAsync(EventSearchQuery query, CancellationToken cancellationToken = default)
        {
            var url = $"/event-reservations-service/events/search?";
            var parameters = new Dictionary<string, object>
            {
                { "lat", query.Latitude },
                { "lng", query.Longitude },
                { "isPremium", query.IsPremium },
                { "tag", query.Tag },
                { "searchType", query.SearchType },
                { "maxMeters", query.MaxMeters },
                { "pageSize", query.PageSize },
                { "page", query.Page },
                { "sort", query.Sort },
                { "sortDirection", query.SortDirection }
            };

            url += string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"));

            var (response, results) = await _client.GetAsJsonWithResponseAsync<EventSearchResponse>(url, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            return results;
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
