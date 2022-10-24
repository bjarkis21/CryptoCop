using System.Net.Http;
using System.Threading.Tasks;
using Cryptocop.Software.API.Models;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Services.Helpers;
using Cryptocop.Software.API.Services.Interfaces;
using Newtonsoft.Json.Linq;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class ExchangeService : IExchangeService
    {
        private readonly HttpClient _httpClient;

        public ExchangeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Envelope<ExchangeDto>> GetExchanges(int pageNumber = 1)
        {
            var response = await _httpClient.GetAsync($"v1/markets?page={pageNumber}?fields=id,exchange_name,exchange_slug,base_asset_symbol,price_usd,last_trade_at");

            if (!response.IsSuccessStatusCode) return new Envelope<ExchangeDto>();
            
            var content = await response.DeserializeJsonToList<ExchangeDto>(flatten: true);

            return new Envelope<ExchangeDto> {
                PageNumber = pageNumber,
                Items = content
            };
        } 
    }
}