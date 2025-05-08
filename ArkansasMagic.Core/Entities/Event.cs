using System;

namespace ArkansasMagic.Core.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string GroupId { get; set; }
        public string ShortCode { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public string Currency { get; set; }
        public int StartingTableNumber { get; set; }
        public bool HasTop8 { get; set; }
        public bool IsAdHoc { get; set; }
        public bool IsOnline { get; set; }
        public string OfficialEventTemplate { get; set; }
        public int Reservations { get; set; }
        public int Registrations { get; set; }
        public bool IsReserved { get; set; }
        public DateTime StartTime { get; set; }
        public string Format { get; set; }
        public string FormatId { get; set; }
        public int RequiredTeamSize { get; set; }
        public string CreatedBy { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string TimeZone { get; set; }
        public string RuleEnforcementLevel { get; set; }
        public string PairingType { get; set; }

        public DateTime CreatedDateUtc { get; set; }
        public DateTime UpdatedDateUtc { get; set; }

        //public Organization Host { get; set; }
    }
}
