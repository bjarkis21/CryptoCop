using Cryptocop.Software.API.Models.Exceptions;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // TODO: Setup routes
        [HttpPost]
        [Route("")]
        [Authorize]
        public IActionResult AddPaymentCard([FromBody] PaymentCardInputModel cardInput)
        {
            if (!ModelState.IsValid) throw new ModelFormatException(string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));

            var email = User.Claims.FirstOrDefault(c => c.Type == "name")!.Value;

            _paymentService.AddPaymentCard(email, cardInput);
            return StatusCode(201);
        }

        [HttpGet]
        [Route("")]
        [Authorize]
        public IActionResult GetPaymentCards()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == "name")!.Value;
            return Ok(_paymentService.GetStoredPaymentCards(email));
        }
    }
}