using System.Text.Json.Serialization;

namespace ArkansasMagic.Core.Wizards.Models
{
    public class EventDetail : Event
    {
        [JsonPropertyName("shortCode")]
        public string ShortCode { get; set; }

        [JsonPropertyName("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("timeZone")]
        public string TimeZone { get; set; }

        [JsonPropertyName("rulesEnforcementLevel")]
        public string RuleEnforcementLevel { get; set; }

        [JsonPropertyName("pairingType")]
        public string PairingType { get; set; }
    }
}
