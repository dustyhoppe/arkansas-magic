using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ArkansasMagic.Core.Wizards.Models
{
    public class Event
    {
        [JsonPropertyName("eventId")]
        public int Id { get; set; }

        [JsonPropertyName("organizationId")]
        public int OrganizationId { get; set; }

        [JsonPropertyName("groupId")]
        public string GroupId { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("cost")]
        public decimal Cost { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("startingTableNumber")]
        public int StartingTableNumber { get; set; }

        [JsonPropertyName("hasTop8")]
        public bool HasTop8 { get; set; }

        [JsonPropertyName("isAdHoc")]
        public bool IsAdHoc { get; set; }

        [JsonPropertyName("isOnline")]
        public bool IsOnline { get; set; }

        [JsonPropertyName("officialEventTemplate")]
        public string OfficialEventTemplate { get; set; }

        [JsonPropertyName("reservations")]
        public int Reservations { get; set; }

        [JsonPropertyName("registrations")]
        public int Registrations { get; set; }

        [JsonPropertyName("isReserved")]
        public bool IsReserved { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("latitude")]
        public decimal Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public decimal Longitude { get; set; }

        [JsonPropertyName("startDatetime")]
        public DateTime StartTime { get; set; }

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; }

        [JsonPropertyName("formatId")]
        public string FormatId { get; set; }

        [JsonPropertyName("distance")]
        public decimal Distance { get; set; }

        [JsonPropertyName("format")]
        public string Format { get; set; }

        [JsonPropertyName("requiredTeamSize")]
        public int RequiredTeamSize { get; set; }
    }
}