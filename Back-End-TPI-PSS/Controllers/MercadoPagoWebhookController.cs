using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using MercadoPago.Client.Payment;
using Back_End_TPI_PSS.Data;
using Back_End_TPI_PSS.Models;
using Back_End_TPI_PSS.Data.Entities;
using Back_End_TPI_PSS.Services.Interfaces;
using Back_End_TPI_PSS.Context;
using Back_End_TPI_PSS.Services.Implementations;
using MercadoPago.Resource.Preference;

namespace Back_End_TPI_PSS.Controllers
{
    //[ApiController]
    //[Route("api/mercadopago/webhook")]
    //public class MercadoPagoWebhookController : ControllerBase
    //{
    //    private readonly PPSContext _dbContext;
    //    private readonly IMercadoPagoPayment _mercadoPagoPayment;
    //    private readonly IOrderService _orderService;

    //    public MercadoPagoWebhookController(PPSContext dbContext, IMercadoPagoPayment mercadoPagoPayment, IOrderService orderService)
    //    {
    //        _dbContext = dbContext;
    //        _mercadoPagoPayment = mercadoPagoPayment;
    //        _orderService = orderService;
    //    }

    //    [HttpPost]
    //    public async Task<IActionResult> ReceiveWebhook([FromBody] MercadoPagoWebHook webhookData)
    //    {
    //        if (webhookData == null)
    //        {
    //            return BadRequest("Webhook data is null.");
    //        }

    //        switch (webhookData.Type)
    //        {
    //            case "payment":
    //                var paymentClient = new PaymentClient();
    //                var payment = await paymentClient.GetAsync(webhookData.Id);

    //                if (payment != null)
    //                {
    //                    var preferenceId = payment.Metadata.ContainsKey("preference_id") ? payment.Metadata["preference_id"].ToString() : null;
                        
    //                    if (preferenceId != null)
    //                    {
    //                        var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.PreferenceId == preferenceId);

    //                        //if (order == null)
    //                        //{
    //                        //    order = new Order
    //                        //    {
    //                        //        PreferenceId = preferenceId,
    //                        //        Status = OrderStatus.Pending,
    //                        //        CreatedAt = DateTime.Now,
    //                        //        UpdatedAt = DateTime.Now,
    //                        //        OrderLines = new List<OrderLine>() // Asegúrate de agregar las líneas de orden si las tienes
    //                        //    };

    //                        //    _dbContext.Orders.Add(order);
    //                        //}

    //                        if (payment.Status == "approved")
    //                        {
    //                            await _orderService.UpdateOrderStatus(order);
    //                            order.UpdatedAt = DateTime.Now;

    //                            // Actualizar stock de productos (ejemplo)
    //                            //foreach (var item in preferenceId.Items)
    //                            //{
    //                            //    var productId = item.Id;
    //                            //    var quantity = item.Quantity;

    //                            //    var product = await _dbContext.Products.FindAsync(productId);

    //                            //    if (product != null)
    //                            //    {
    //                            //        product.Stock -= quantity;

    //                            //        _dbContext.Products.Update(product);
    //                            //    }
    //                            //}
    //                        }

    //                        await _dbContext.SaveChangesAsync();
    //                    }
    //                }
    //                break;

    //            default:

    //                break;
    //        }

    //        return Ok();
    //    }
    //}
}
