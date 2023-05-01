using ArkansasMagic.Core.Data;
using ArkansasMagic.Core.Wizards;
using ArkansasMagic.Domain.Clock;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ArkansasMagic.Api.Services
{
    public class OrganizationService : IHostedService
    {
        private readonly ILogger<OrganizationService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public OrganizationService(ILogger<OrganizationService> logger
            , IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Organizations Service running.");

            Task.Run(() => ProcessOrganizationsAsync(cancellationToken));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Organizations Service is stopping.");

            return Task.CompletedTask;
        }

        private async Task ProcessOrganizationsAsync(CancellationToken cancellationToken)
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

                    var organizationIds = await dbContext.Events
                        .Select(e => e.OrganizationId)
                        .Distinct()
                        .ToListAsync(cancellationToken);

                    foreach (var organizationId in organizationIds)
                    {
                        var dbOrganization = await dbContext.Organizations
                            .SingleOrDefaultAsync(o => o.Id == organizationId, cancellationToken);

                        var organization = await wizardsClient.GetOrganizationAsync(organizationId, cancellationToken);
                        if (organization != null)
                        {
                            if (dbOrganization == null)
                            {
                                dbOrganization = new Core.Entities.Organization
                                {
                                    Id = organizationId,
                                    CreatedDateUtc = timestamp,
                                };
                                await dbContext.Organizations.AddAsync(dbOrganization, cancellationToken);
                            }

                            dbOrganization.AcceptedTermsAt = organization.AcceptedTermsAt;
                            dbOrganization.Address = organization.Address;
                            dbOrganization.Brands = string.Join("|", organization.Brands ?? new System.Collections.Generic.List<string>());
                            dbOrganization.City = organization.City;
                            dbOrganization.Country = organization.Country;
                            dbOrganization.EmailAddress = organization.EmailAddress;
                            dbOrganization.IsPremium = organization.IsPremium;
                            dbOrganization.IsTestStore = organization.IsTestStore;
                            dbOrganization.Latitude = organization.Latitude;
                            dbOrganization.Longitude = organization.Longitude;
                            dbOrganization.Name = organization.Name;
                            dbOrganization.PhoneNumber = organization.PhoneNumber;
                            dbOrganization.PhoneNumbers = string.Join("|", organization.PhoneNumbers ?? new System.Collections.Generic.List<string>());
                            dbOrganization.PostalAddress = organization.PostalAddress;
                            dbOrganization.PostalCode = organization.PostalCode;
                            dbOrganization.ShowEmailInSel = organization.ShowEmailInSel;
                            dbOrganization.State = organization.State;
                            dbOrganization.UpdatedDateUtc = timestamp;
                            dbOrganization.Website = organization.Website;
                            dbOrganization.Websites = string.Join("|", organization.Websites ?? new System.Collections.Generic.List<string>());
                        }
                    }

                    await dbContext.SaveChangesAsync(cancellationToken);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Failure running Organizations execution loop.");
                }

                await Task.Delay(TimeSpan.FromMinutes(15), cancellationToken);
            }
        }
    }
}
