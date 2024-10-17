using Polly;
using Polly.Retry;

namespace ProductApplication.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly AsyncRetryPolicy _retryPolicy;
        private readonly ILogger<ApiClient> _logger;

        public ApiClient(HttpClient httpClient, ILogger<ApiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            _retryPolicy = Policy
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(3, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (response, timespan, retryCount, context) =>
                    {
                        _logger.LogWarning($"Запрос не удался. Повтор попытки {retryCount}. Ожидание {timespan.TotalSeconds} секунд.");
                    });
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                var response = await _httpClient.GetAsync(requestUri);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Ошибка: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                else
                {
                    _logger.LogInformation($"Запрос GET к {requestUri} успешно выполнен.");
                }

                return response;
            });
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string requestUri, T content)
        {
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                var response = await _httpClient.PostAsJsonAsync(requestUri, content);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Ошибка POST-запроса: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                else
                {
                    _logger.LogInformation($"Запрос POST к {requestUri} успешно выполнен.");
                }

                return response;
            });
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string requestUri, T content)
        {
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                var response = await _httpClient.PutAsJsonAsync(requestUri, content);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Ошибка PUT-запроса: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                else
                {
                    _logger.LogInformation($"Запрос PUT к {requestUri} успешно выполнен.");
                }

                return response;
            });
        }

        public async Task<HttpResponseMessage> DeleteAsync(string requestUri)
        {
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                var response = await _httpClient.DeleteAsync(requestUri);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Ошибка DELETE-запроса: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
                else
                {
                    _logger.LogInformation($"Запрос DELETE к {requestUri} успешно выполнен.");
                }

                return response;
            });
        }
    }
}
