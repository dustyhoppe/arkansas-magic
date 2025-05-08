using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ArkansasMagic.Domain.Http
{
    public static class HttpClientExtensions
    {
        internal const string _requestTypeKey = "__httpTracing:RequestType";
        internal const string _responseTypeKey = "__httpTracing:ResponseType";

        public static async Task<T> GetAsJsonAsync<T>(
            this HttpClient client,
            string uri,
            CancellationToken cancellationToken = default)
        {
            var (response, result) = await GetAsJsonWithResponseAsync<T>(
                client,
                uri,
                cancellationToken);

            response.Dispose();

            return result;
        }

        public static Task<(HttpResponseMessage, T)> GetAsJsonWithResponseAsync<T>(
            this HttpClient client,
            string uri,
            CancellationToken cancellationToken = default)
        {
            return SendAsJsonWithResponseAsync<T>(
                client,
                "GET",
                uri,
                null,
                cancellationToken);
        }

        public static async Task<T> DeleteAsJsonAsync<T>(
            this HttpClient client,
            string uri,
            CancellationToken cancellationToken = default)
        {
            var (response, result) = await DeleteAsJsonWithResponseAsync<T>(
                client,
                uri,
                cancellationToken);

            response.Dispose();

            return result;
        }

        public static Task<(HttpResponseMessage, T)> DeleteAsJsonWithResponseAsync<T>(
            this HttpClient client,
            string uri,
            CancellationToken cancellationToken = default)
        {
            return SendAsJsonWithResponseAsync<T>(
                client,
                "DELETE",
                uri,
                null,
                cancellationToken);
        }

        public static Task<T> PostAsJsonAsync<T>(
            this HttpClient client,
            string uri,
            object body,
            CancellationToken cancellationToken = default)
        {
            return SendAsJsonAsync<T>(client, "POST", uri, body, cancellationToken);
        }

        public static Task<HttpResponseMessage> PostAsJsonAsync(
            this HttpClient client,
            string uri,
            object body,
            CancellationToken cancellationToken = default)
        {
            return SendAsJsonAsync(client, "POST", uri, body, cancellationToken);
        }

        public static Task<(HttpResponseMessage, T)> PostAsJsonWithResponseAsync<T>(
            this HttpClient client,
            string uri,
            object body,
            CancellationToken cancellationToken = default)
        {
            return SendAsJsonWithResponseAsync<T>(
                client,
                "POST",
                uri,
                body,
                cancellationToken);
        }

        public static Task<HttpResponseMessage> PutAsJsonAsync(
            this HttpClient client,
            string uri,
            object body,
            CancellationToken cancellationToken = default)
        {
            return SendAsJsonAsync(client, "PUT", uri, body, cancellationToken);
        }

        public static Task<T> PutAsJsonAsync<T>(
            this HttpClient client,
            string uri,
            object body,
            CancellationToken cancellationToken = default)
        {
            return SendAsJsonAsync<T>(client, "PUT", uri, body, cancellationToken);
        }

        public static Task<(HttpResponseMessage, T)> PutAsJsonWithResponseAsync<T>(
            this HttpClient client,
            string uri,
            object body,
            CancellationToken cancellationToken = default)
        {
            return SendAsJsonWithResponseAsync<T>(
                client,
                "PUT",
                uri,
                body,
                cancellationToken);
        }

        public static Task<T> PatchAsJsonAsync<T>(
            this HttpClient client,
            string uri,
            object body,
            CancellationToken cancellationToken = default)
        {
            return SendAsJsonAsync<T>(client, "PATCH", uri, body, cancellationToken);
        }

        public static Task<(HttpResponseMessage, T)> PatchAsJsonWithResponseAsync<T>(
            this HttpClient client,
            string uri,
            object body,
            CancellationToken cancellationToken = default)
        {
            return SendAsJsonWithResponseAsync<T>(
                client,
                "PATCH",
                uri,
                body,
                cancellationToken);
        }

        public static async Task<T> SendAsJsonAsync<T>(
            this HttpClient client,
            string method,
            string uri,
            object body,
            CancellationToken cancellationToken = default)
        {
            var (response, result) = await SendAsJsonWithResponseAsync<T>(
                client,
                method,
                uri,
                body,
                cancellationToken);

            response.Dispose();

            return result;
        }

        public static async Task<(HttpResponseMessage, T)> SendAsJsonWithResponseAsync<T>(
            this HttpClient client,
            string method,
            string uri,
            object body,
            CancellationToken cancellationToken = default)
        {
            var message = new HttpRequestMessage(
                new HttpMethod(method),
                uri);

            if (body != null)
            {
                message.Content = new StringContent(
                    JsonSerializer.Serialize(body),
                    Encoding.UTF8,
                    "application/json");

                message.Properties[_requestTypeKey] = body.GetType();
            }

            message.Properties[_responseTypeKey] = typeof(T);

            var response = await client.SendAsync(message, HttpCompletionOption.ResponseContentRead, cancellationToken);
            var responseBody = await response.Content.ReadAsStringAsync();
            return (response, JsonSerializer.Deserialize<T>(responseBody));
        }

        public static Task<HttpResponseMessage> SendAsJsonAsync(
            this HttpClient client,
            string method,
            string uri,
            object body,
            CancellationToken cancellationToken = default)
        {
            var message = new HttpRequestMessage(
                new HttpMethod(method),
                uri);

            if (body != null)
            {
                message.Content = new StringContent(
                    JsonSerializer.Serialize(body),
                    Encoding.UTF8,
                    "application/json");

                message.Properties[_requestTypeKey] = body.GetType();
            };

            return client.SendAsync(message, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }
    }
}
