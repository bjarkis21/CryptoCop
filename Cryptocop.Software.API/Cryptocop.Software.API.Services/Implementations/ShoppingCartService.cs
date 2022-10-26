using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.Exceptions;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Services.Helpers;
using Cryptocop.Software.API.Services.Interfaces;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly HttpClient _httpClient;
        private readonly List<string> _slugs;
        private readonly IShoppingCartRepository _shoppingCartRepository;


        public ShoppingCartService(HttpClient httpClient, IShoppingCartRepository shoppingCartRepository)
        {
            _httpClient = httpClient;
            _slugs = new List<string> { "bitcoin", "ethereum", "tether", "monero" };
            _shoppingCartRepository = shoppingCartRepository;
        }
        public IEnumerable<ShoppingCartItemDto> GetCartItems(string email)
        {
            return _shoppingCartRepository.GetCartItems(email);
        }

        public async Task AddCartItem(string email, ShoppingCartItemInputModel shoppingCartItem)
        {
            var response = await _httpClient.GetAsync($"v1/assets/{shoppingCartItem.ProductIdentifier}/metrics?fields=slug,market_data/price_usd");

            if (response.StatusCode == HttpStatusCode.NotFound) throw new ProductException($"CryptoCurrency with identifier {shoppingCartItem.ProductIdentifier} does not exist. Please use valid CryptoCurrency ID or slug.");

            CryptoCurrencyDto cc = await response.DeserializeJsonToObject<CryptoCurrencyDto>(flatten: true);

            shoppingCartItem.ProductIdentifier = cc.Slug;
            
            if (!_slugs.Contains(cc.Slug)) throw new ProductException("The only Cryptocurrencies available are " + string.Join(", ", _slugs));

            _shoppingCartRepository.AddCartItem(email, shoppingCartItem, cc.PriceInUsd);
        }

        public void RemoveCartItem(string email, int id)
        {
            _shoppingCartRepository.RemoveCartItem(email, id);
        }

        public void UpdateCartItemQuantity(string email, int id, double quantity)
        {
            _shoppingCartRepository.UpdateCartItemQuantity(email, id, quantity);
        }

        public void ClearCart(string email)
        {
            _shoppingCartRepository.ClearCart(email);
        }
    }
}
