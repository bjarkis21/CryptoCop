using Cryptocop.Software.API.Models.Exceptions;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/addresses")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        [Route("")]
        [Authorize]
        public IActionResult GetAllAddresses()
        {
            return Ok(_addressService.GetAllAddresses(User.Claims.FirstOrDefault(c => c.Type == "name")!.Value));
        }

        [HttpPost]
        [Route("")]
        [Authorize]
        public IActionResult AddAddress([FromBody] AddressInputModel addressInput)
        {
            if (!ModelState.IsValid) throw new ModelFormatException(string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));

            _addressService.AddAddress(User.Claims.FirstOrDefault(c => c.Type == "name")!.Value, addressInput);

            return StatusCode(201);
        }

        [HttpDelete]
        [Route("{addressId:int}")]
        [Authorize]
        public IActionResult DeleteAddress(int addressId)
        {
            _addressService.DeleteAddress(User.Claims.FirstOrDefault(c => c.Type == "name")!.Value, addressId);
            return NoContent();
        }

    }
}