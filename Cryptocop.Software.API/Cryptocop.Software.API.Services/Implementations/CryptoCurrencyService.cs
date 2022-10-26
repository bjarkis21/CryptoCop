using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Services.Helpers;
using Cryptocop.Software.API.Services.Interfaces;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class CryptoCurrencyService : ICryptoCurrencyService
    {
        private readonly HttpClient _httpClient;
        private readonly IEnumerable<string> availableCurr;

        public CryptoCurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            availableCurr = new List<string> {"BTC", "ETH", "USDT", "XMR"};
        }

        public async Task<IEnumerable<CryptoCurrencyDto>> GetAvailableCryptocurrencies()
        {
            var response = await _httpClient.GetAsync($"v2/assets?fields=id,slug,symbol,metrics/market_data/price_usd,profile/general/overview/project_details,name&limit=500");
            if (!response.IsSuccessStatusCode) return new List<CryptoCurrencyDto>();

            var content = await response.DeserializeJsonToList<CryptoCurrencyDto>(flatten: true);

            return content.Where(c => availableCurr.Contains(c.Symbol));
        }
    }
}