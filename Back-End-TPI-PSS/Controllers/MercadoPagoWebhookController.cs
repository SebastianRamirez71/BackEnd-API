using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using MercadoPago.Client.Payment;
using MercadoPago.Webhook;
using Back_End_TPI_PSS.Data;
using Back_End_TPI_PSS.Models;
using Back_End_TPI_PSS.Data.Entities;
using Back_End_TPI_PSS.Services.Interfaces;

namespace Back_End_TPI_PSS.Controllers
{
    [ApiController]
    [Route("api/mercadopago/webhook")]
    public class MercadoPagoWebhookController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IMercadoPagoPayment _mercadoPagoPayment;

        public MercadoPagoWebhookController(AppDbContext dbContext, IMercadoPagoPayment mercadoPagoPayment)
        {
            _dbContext = dbContext;
            _mercadoPagoPayment = mercadoPagoPayment;
        }

        [HttpPost]
        public async Task<IActionResult> ReceiveWebhook([FromBody] MercadoPagoWebhookData webhookData)
        {
            if (webhookData == null)
            {
                return BadRequest();
            }

            // Validar la autenticidad del webhook si es necesario
            // Aquí puedes implementar lógica para validar la firma del webhook si MercadoPago la proporciona

            // Procesar el evento del webhook según su tipo
            switch (webhookData.Type)
            {
                case "payment":
                    var paymentClient = new PaymentClient();
                    var payment = await paymentClient.GetAsync(webhookData.Id);

                    // Actualizar estado de la orden según el payment.status recibido
                    if (payment.Status == "approved")
                    {
                        var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.PreferenceId == payment.PreferenceId);
                        if (order != null)
                        {
                            order.Status = OrderStatus.Approved;
                            order.UpdatedAt = DateTime.Now;

                            await _dbContext.SaveChangesAsync();

                            // Actualizar stock de productos (ejemplo)
                            foreach (var orderLine in order.OrderLines)
                            {
                                // Aquí podrías implementar lógica para actualizar el stock del producto
                                // E.g., restar la cantidad de orderLine.Quantity del stock del producto correspondiente
                                // Suponiendo que tienes un servicio o repositorio para gestionar productos y stock
                            }
                        }
                    }
                    break;
                default:
                    // Manejar otros tipos de eventos si es necesario
                    break;
            }

            return Ok();
        }
    }
}
