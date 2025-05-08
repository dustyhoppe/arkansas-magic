using System.Text.Json.Serialization;

namespace ArkansasMagic.Core.Wizards.Models
{
    public class Brand
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
