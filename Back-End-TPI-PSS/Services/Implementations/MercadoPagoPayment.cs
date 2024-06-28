using Back_End_TPI_PSS.Data.Entities;
using Back_End_TPI_PSS.Data.Models.OrderDTOs;
using Back_End_TPI_PSS.Models;
using Back_End_TPI_PSS.Services.Interfaces;
using MercadoPago.Client.Payment;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_TPI_PSS.Services.Implementations
{
    public class MercadoPagoPayment : IMercadoPagoPayment
    {
        private readonly IOrderService _orderService;

        public MercadoPagoPayment(IOrderService orderService)
        {
            MercadoPagoConfig.AccessToken = "APP_USR-4870971039960-062618-60e3119bca2338c0da52557538693711-1872136931";
            _orderService = orderService;
        }

        public async Task<Preference> CreatePreferenceRequest(List<CartItem> items)
        {
            var item = items.First(); // Solo toma el primer elemento para simplificar
            var request = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest>
        {
            new PreferenceItemRequest
            {
                Title = item.Name,
                Description = $"Color: {item.Color}, Size: {item.SizeName}",
                PictureUrl = item.Image,
                Quantity = item.Quantity,
                CurrencyId = "ARS",
                UnitPrice = item.Price
            }
        },
                BackUrls = new PreferenceBackUrlsRequest
                {
                    Success = "http://localhost:3000",
                    Failure = "http://localhost:3000",
                    Pending = "http://localhost:3000",
                },
                AutoReturn = "approved",
                NotificationUrl = "https://tu-domino.com/api/mercadopago/webhook",

                Metadata = new Dictionary<string, object>
        {
            { "preference_id", Guid.NewGuid().ToString() }
        }
            };

            var client = new PreferenceClient();
            try
            {
                Console.WriteLine("Creating MercadoPago preference request...");
                Preference preference = await client.CreateAsync(request);
                Console.WriteLine($"Preference created with ID: {preference.Id}");

                request.Metadata["preference_id"] = preference.Id.ToString();
                await client.UpdateAsync(preference.Id, request);

                Console.WriteLine("Creating order...");
                var order = new Order
                {
                    PreferenceId = preference.Id.ToString(),
                    ProductQuantity = item.Quantity, // Total quantity of products in the order
                    OrderLines = new List<OrderLine>
            {
                new OrderLine
                {
                    Description = item.Name,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price,
                    ColorId = item.ColorId,
                    SizeId = item.SizeId
                }
            }
                };

                await _orderService.AddOrder(order);
                Console.WriteLine("Order created successfully");
                return preference;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating preference: {ex.Message}");
                throw new Exception("Error creating MercadoPago preference", ex);
            }
        }

    }
}
