using ArkansasMagic.Api.Configuration;
using ArkansasMagic.Core.Alerts;
using ArkansasMagic.Core.Alerts.Types;
using ArkansasMagic.Core.Data;
using ArkansasMagic.Core.Wizards;
using ArkansasMagic.Core.Wizards.Models;
using ArkansasMagic.Domain.Clock;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ArkansasMagic.Api.Services
{
    public class EventFeedService : IHostedService
    {
        private readonly ILogger<EventFeedService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly EventSearchQuery _searchQuery;
        private readonly IOptionsMonitor<EventFiltersOptions> _options;

        private const string DiscordDeliveryEndpoint = "https://discord.com/api/webhooks/1102620473600266301/dQVFwu6shViPAnNxa4mxu58xpXpexXA1USugEXabnWhwb3FnlmfNZfQDPJZ856RFwr0h";

        public EventFeedService(ILogger<EventFeedService> logger
            , IServiceProvider serviceProvider
            , IOptionsMonitor<EventFiltersOptions> options)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _options = options;
            _searchQuery = new EventSearchQuery
            {
                Latitude = _options.CurrentValue.Coordinates.Latitude,
                Longitude = _options.CurrentValue.Coordinates.Longitude,
                IsPremium = false,
                MaxMeters = ConvertMilesToMeters(_options.CurrentValue.MaximumMiles)
            };
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Event Feed Service running.");

            Task.Run(() => ProcessEventFeedAsync(cancellationToken));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Event Feed Service is stopping.");

            return Task.CompletedTask;
        }

        private async Task ProcessEventFeedAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();

                try
                {
                    using var dbContext = scope.ServiceProvider.GetService<IApplicationDbContext>();
                    using var wizardsClient = scope.ServiceProvider.GetService<IWizardsApiClient>();
                    var discord = scope.ServiceProvider.GetRequiredService<IDeliveryProvider>();

                    var clock = scope.ServiceProvider.GetService<ISystemClock>();

                    var timestamp = clock.UtcNow();

                    EventSearchResponse response = null;
                    var page = 1;
                    do
                    {
                        response = await wizardsClient.SearchEventsAsync(GetEventSearchQuery(page), cancellationToken);

                        foreach (var @event in response?.Results ?? new List<Event>())
                        {
                            Console.WriteLine($"{@event.Name}: {@event.Description} @{@event.StartTime}");

                            var dbEvent = await dbContext.Events
                                .SingleOrDefaultAsync(e => e.Id == @event.Id, cancellationToken: cancellationToken);

                            var createEvent = false;
                            if (dbEvent == null)
                            {
                                dbEvent = new Core.Entities.Event()
                                {
                                    Id = @event.Id,
                                    CreatedDateUtc = timestamp,
                                };
                                await dbContext.Events.AddAsync(dbEvent, cancellationToken);
                                createEvent = true;
                            }

                            dbEvent.Cost = @event.Cost;
                            dbEvent.Currency = @event.Currency;
                            dbEvent.Description = @event.Description;
                            dbEvent.Format = @event.Format;
                            dbEvent.FormatId = @event.FormatId;
                            dbEvent.GroupId = @event.GroupId;
                            dbEvent.HasTop8 = @event.HasTop8;
                            dbEvent.IsAdHoc = @event.IsAdHoc;
                            dbEvent.IsOnline = @event.IsOnline;
                            dbEvent.IsReserved = @event.IsReserved;
                            dbEvent.Name = @event.Name;
                            dbEvent.OfficialEventTemplate = @event.OfficialEventTemplate;
                            dbEvent.OrganizationId = @event.OrganizationId;
                            dbEvent.Registrations = @event.Registrations;
                            dbEvent.RequiredTeamSize = @event.RequiredTeamSize;
                            dbEvent.Reservations = @event.Reservations;
                            dbEvent.StartingTableNumber = @event.StartingTableNumber;
                            dbEvent.StartTime = @event.StartTime;
                            dbEvent.Status = @event.Status;
                            dbEvent.UpdatedDateUtc = timestamp;

                            if (createEvent && dbEvent.Name.Contains("RCQ", StringComparison.OrdinalIgnoreCase))
                            {
                                var organization = await GetOrganizationAsync(dbContext, wizardsClient, dbEvent.OrganizationId, cancellationToken);

                                var alert = new RcqAddedAlert(DiscordDeliveryEndpoint, dbEvent, organization);
                                await discord.AlertAsync(alert, cancellationToken);
                            }
                        }

                        await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                        page++;
                    } while (response != null && response.Results.Count > 0);

                    await dbContext.SaveChangesAsync(cancellationToken);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Failure running Event Feed execution loop.");
                }

                await Task.Delay(TimeSpan.FromMinutes(15), cancellationToken);
            }
        }

        private static async Task<Core.Entities.Organization> GetOrganizationAsync(IApplicationDbContext dbContext, IWizardsApiClient wizardsClient
            , int organizationId, CancellationToken cancellationToken)
        {
            var organization = await dbContext.Organizations
                                    .SingleOrDefaultAsync(o => o.Id == organizationId, cancellationToken: cancellationToken);
            if (organization != null)
                return organization;

            var apiOrganization = await wizardsClient.GetOrganizationAsync(organizationId, cancellationToken);
            if (apiOrganization != null)
            {
                var timestamp = DateTime.UtcNow;

                var dbOrganization = new Core.Entities.Organization
                {
                    Id = organizationId,
                    CreatedDateUtc = timestamp,
                };
                await dbContext.Organizations.AddAsync(dbOrganization, cancellationToken);

                dbOrganization.AcceptedTermsAt = apiOrganization.AcceptedTermsAt;
                dbOrganization.Address = apiOrganization.Address;
                dbOrganization.Brands = string.Join("|", apiOrganization.Brands?.Select(b => b.Name) ?? new System.Collections.Generic.List<string>());
                dbOrganization.City = apiOrganization.City;
                dbOrganization.Country = apiOrganization.Country;
                dbOrganization.EmailAddress = apiOrganization.EmailAddress ?? "UNKNOWN";
                dbOrganization.IsPremium = apiOrganization.IsPremium;
                dbOrganization.IsTestStore = apiOrganization.IsTestStore;
                dbOrganization.Latitude = apiOrganization.Latitude;
                dbOrganization.Longitude = apiOrganization.Longitude;
                dbOrganization.Name = apiOrganization.Name;
                dbOrganization.PhoneNumber = apiOrganization.PhoneNumber;
                dbOrganization.PhoneNumbers = string.Join("|", apiOrganization.PhoneNumbers ?? new System.Collections.Generic.List<string>());
                dbOrganization.PostalAddress = apiOrganization.PostalAddress;
                dbOrganization.PostalCode = apiOrganization.PostalCode;
                dbOrganization.ShowEmailInSel = apiOrganization.ShowEmailInSel;
                dbOrganization.State = apiOrganization.State;
                dbOrganization.UpdatedDateUtc = timestamp;
                dbOrganization.Website = apiOrganization.Website;
                dbOrganization.Websites = string.Join("|", apiOrganization.Websites ?? new System.Collections.Generic.List<string>());

                return dbOrganization;
            }

            return null;
        }

        private EventSearchQuery GetEventSearchQuery(int page)
        {
            return new EventSearchQuery
            {
                Page = page,
                PageSize = _searchQuery.PageSize,
                IsPremium = _searchQuery.IsPremium,
                Latitude = _searchQuery.Latitude,
                Longitude = _searchQuery.Longitude,
                MaxMeters = _searchQuery.MaxMeters,
                SearchType = _searchQuery.SearchType,
                Sort = _searchQuery.Sort,
                SortDirection = _searchQuery.SortDirection,
                Tag = _searchQuery.Tag,
            };
        }

        public static int ConvertMilesToMeters(int miles)
        {
            const double metersPerMile = 1609.344;
            return (int)(miles * metersPerMile);
        }

    }
}
