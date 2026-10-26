using System.Net.Http;

namespace PAW.Web.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly HttpClient _httpClient;

        public ExchangeRateService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetExchangeRateAsync()
        {
            var response = await _httpClient.GetAsync(
                "https://api.hacienda.go.cr/indicadores/tc");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}