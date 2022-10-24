using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [ApiController]
    [Route("api/cryptocurrencies")]
    public class CryptoCurrencyController : ControllerBase
    {
        private readonly ICryptoCurrencyService _cryptoCurrencyService;

        public CryptoCurrencyController(ICryptoCurrencyService cryptoCurrencyService)
        {
            _cryptoCurrencyService = cryptoCurrencyService;
        }
        // TODO: Setup routes
        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<IActionResult> GetAvailableCryptocurrencies()
        {
            var data = await _cryptoCurrencyService.GetAvailableCryptocurrencies();
            return Ok(data);
        }
    }
}
