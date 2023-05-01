using System.Text.Json.Serialization;

namespace ArkansasMagic.Contracts.Health
{
    public class CheckResponse
    {
        [JsonPropertyName("thumbsUp")]
        public bool ThumbsUp { get; set; }
    }
}
