using Back_End_TPI_PSS.Context;
using Back_End_TPI_PSS.Data.Entities;
using Back_End_TPI_PSS.Models;
using Back_End_TPI_PSS.Services.Implementations;
using Back_End_TPI_PSS.Services.Interfaces;
using MercadoPago.Resource.Payment;
using MercadoPago.Resource.Preference;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Back_End_TPI_PSS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MercadoPagoController : ControllerBase
    {
        private readonly PPSContext _context;

        private readonly IMercadoPagoPayment _mercadoPagoPayment;
        private readonly IOrderService _orderService;

        public MercadoPagoController(IMercadoPagoPayment mercadoPagoPayment, IOrderService orderService, PPSContext context)
        {
            _mercadoPagoPayment = mercadoPagoPayment;
            _orderService = orderService;
            _context = context;
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

        [HttpPut("paymentStatus")]
        public async Task<IActionResult> UpdatePaymentStatus([FromQuery] string preferenceId, string status)
        {

            var order = await _context.Orders.FirstOrDefaultAsync(o => o.PreferenceId == preferenceId);

            if(status == "approved")
            {
                await _orderService.UpdateOrderStatus(order);
                order.UpdatedAt = DateTime.Now;
                return Ok();    
            }
            return NotFound();  
        
        }
    }
}
