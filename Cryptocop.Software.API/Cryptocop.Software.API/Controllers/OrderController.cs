using Cryptocop.Software.API.Models.Exceptions;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // TODO: Setup routes
        [HttpPost]
        [Route("")]
        [Authorize]
        public IActionResult CreateOrder([FromBody] OrderInputModel orderInput)
        {
            if (!ModelState.IsValid) throw new ModelFormatException(string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));

            var email = User.Claims.FirstOrDefault(c => c.Type == "name")!.Value;

            _orderService.CreateNewOrder(email, orderInput);

            return StatusCode(201);
        }

        [HttpGet]
        [Route("")]
        [Authorize]
        public IActionResult GetOrders()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == "name")!.Value;

            return Ok(_orderService.GetOrders(email));
        }
    }
}