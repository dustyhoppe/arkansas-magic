using ArkansasMagic.Core.Alerts.Types;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ArkansasMagic.Core.Alerts
{
    public class DiscordDeliveryProvider : IDeliveryProvider
    {
        private readonly HttpClient _httpClient;

        public DiscordDeliveryProvider()
        {
            _httpClient = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(3)
            };
        }

        public async Task AlertAsync(IAlert alert, CancellationToken cancellationToken = default)
        {
            try
            {
                var message = new
                {
                    content = alert.Content
                };
                var json = JsonSerializer.Serialize(message);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                Console.WriteLine(content);
                await _httpClient.PostAsync(alert.Endpoint, content, cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
