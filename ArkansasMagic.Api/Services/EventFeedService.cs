using ArkansasMagic.Core.Data;
using ArkansasMagic.Core.Wizards;
using ArkansasMagic.Core.Wizards.Models;
using ArkansasMagic.Domain.Clock;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ArkansasMagic.Api.Services
{
    public class EventFeedService : IHostedService
    {
        private readonly ILogger<EventFeedService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly EventSearchQuery _searchQuery;

        public EventFeedService(ILogger<EventFeedService> logger
            , IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _searchQuery = new EventSearchQuery
            {
                Latitude = 35.20105m,
                Longitude = -91.8318334m,
                IsPremium = false,
                MaxMeters = 300000
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
                            if (dbEvent == null)
                            {
                                dbEvent = new Core.Entities.Event()
                                {
                                    Id = @event.Id,
                                    CreatedDateUtc = timestamp,
                                };
                                await dbContext.Events.AddAsync(dbEvent, cancellationToken);
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

                await Task.Delay(TimeSpan.FromMinutes(5), cancellationToken);
            }
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
    }
}
