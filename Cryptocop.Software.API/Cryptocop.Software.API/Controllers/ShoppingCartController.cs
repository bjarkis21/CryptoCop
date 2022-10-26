using Cryptocop.Software.API.Models.Exceptions;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        // TODO: Setup routes
        [HttpPost]
        [Route("")]
        [Authorize]
        public async Task<IActionResult> AddCartItem([FromBody] ShoppingCartItemInputModel item)
        {
            if (!ModelState.IsValid) throw new ModelFormatException(string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));

            var email = User.Claims.FirstOrDefault(c => c.Type == "name")!.Value;

            await _shoppingCartService.AddCartItem(email, item);

            return StatusCode(200);
        }

        [HttpGet]
        [Route("")]
        [Authorize]
        public IActionResult GetCartItems()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == "name")!.Value;
            return Ok(_shoppingCartService.GetCartItems(email));
        }

        [HttpDelete]
        [Route("{shoppingCartId:int}")]
        [Authorize]
        public IActionResult DeleteCartItem(int shoppingCartId)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == "name")!.Value;
            _shoppingCartService.RemoveCartItem(email,shoppingCartId);
            return NoContent();
        }

        [HttpPatch]
        [Route("{shoppingCartId:int}")]
        [Authorize]
        public IActionResult UpdateCartItem(int shoppingCartId, [FromBody] UpdateShoppingCartItemInputModel input)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == "name")!.Value;
            _shoppingCartService.UpdateCartItemQuantity(email, shoppingCartId, (double)input.Quantity!);
            return StatusCode(204);
        }

        [HttpDelete]
        [Route("")]
        [Authorize]
        public IActionResult ClearCart()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == "name")!.Value;
            _shoppingCartService.ClearCart(email);
            return NoContent();
        }
    }
}