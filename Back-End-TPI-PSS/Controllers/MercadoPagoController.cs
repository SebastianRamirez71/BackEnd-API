using Back_End_TPI_PSS.Models;
using Back_End_TPI_PSS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
                Console.WriteLine($"Error creating payment preference: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                return BadRequest($"Error creating payment preference: {ex.Message}");
            }
        }
    }
}
