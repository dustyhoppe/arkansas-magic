using ArkansasMagic.Core.Entities;
using System;

namespace ArkansasMagic.Core.Alerts.Types
{
    public class RcqAddedAlert : IAlert
    {
        private readonly string _endpoint;
        private readonly Event _event;
        private readonly Organization _organization;

        private const string UNKNOWN = "[UNKNOWN]";

        public RcqAddedAlert(string endpoint
            , Event @event
            , Organization organization)
        {
            _endpoint = endpoint;
            _event = @event;
            _organization = organization;
        }

        public string Endpoint => _endpoint;

        public string Content
        {
            get
            {
                var currentTimeZone = TimeZoneInfo.Local;
                var startTime = TimeZoneInfo.ConvertTimeFromUtc(_event.StartTime, currentTimeZone);

                return $@"New Regional Championship Qualifier 

{_event.Name}

{_event.Description}

Date: {startTime}
Format: {_event.Format}
Entry Fee: ${_event.Cost / 100.0m:0.00} {_event.Currency}
Location: {_organization?.Name ?? UNKNOWN}
{_organization?.City ?? UNKNOWN}, {_organization?.State ?? UNKNOWN}

----------------------------------------
";
            }
        }
    }
}
