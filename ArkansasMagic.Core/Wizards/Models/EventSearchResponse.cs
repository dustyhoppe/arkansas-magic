using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ArkansasMagic.Core.Wizards.Models
{
    public class EventSearchResponse
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        [JsonPropertyName("totalResults")]
        public int TotalResults { get; set; }

        [JsonPropertyName("results")]
        public List<Event> Results { get; set; }
    }
}
