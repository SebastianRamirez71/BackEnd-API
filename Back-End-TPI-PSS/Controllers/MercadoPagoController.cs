using Back_End_TPI_PSS.Data.Entities;
using Back_End_TPI_PSS.Models;
using Back_End_TPI_PSS.Services.Implementations;
using Back_End_TPI_PSS.Services.Interfaces;
using MercadoPago.Client.Preference;
using MercadoPago.Resource.Preference;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Back_End_TPI_PSS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MercadoPagoController : ControllerBase
    {
        private readonly IMercadoPagoPayment _mercadoPagoPayment;

        public MercadoPagoController(IMercadoPagoPayment mercadoPagoPayment)
        {
            _mercadoPagoPayment = mercadoPagoPayment;
        }


        [HttpPost("payment")]
        public async Task<IActionResult> CreatePayment([FromBody] List<CartItem> items)
        {
            try
            {
                var preference = await _mercadoPagoPayment.CreatePreferenceRequest(items);
                return Ok(new { preferenceId = preference.Id });
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error creating payment preference: {ex.Message}");
                return BadRequest("Error creating payment preference");
            }
        }

    }
}
