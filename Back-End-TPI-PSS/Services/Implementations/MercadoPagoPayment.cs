using Back_End_TPI_PSS.Context;
using Back_End_TPI_PSS.Data.Entities;
using Back_End_TPI_PSS.Models;
using Back_End_TPI_PSS.Services.Interfaces;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using Microsoft.EntityFrameworkCore;

public class MercadoPagoPayment : IMercadoPagoPayment
{
    private readonly IOrderService _orderService;
    private readonly PPSContext _context; // Agregar el contexto aquí si no está presente

    public MercadoPagoPayment(IOrderService orderService, PPSContext context)
    {
        _orderService = orderService;
        _context = context; // Asignar el contexto aquí si no está presente
        MercadoPagoConfig.AccessToken = "APP_USR-4870971039960-062618-60e3119bca2338c0da52557538693711-1872136931";
    }

    public async Task<Preference> CreatePreferenceRequest(List<CartItem> items, int userId)
    {

        var request = new PreferenceRequest
        {
            Items = items.Select(item => new PreferenceItemRequest
            {
                Title = item.Name,
                Description = $"Color: {item.Color}, Size: {item.SizeName}",
                PictureUrl = item.Image,
                Quantity = item.Quantity,
                CurrencyId = "ARS",
                UnitPrice = item.Price
            }).ToList(),

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
                { "preference_id", Guid.NewGuid().ToString() } // Generar un ID único para cada preferencia
            }
        };

        // Verificar si ya existe una preferencia con el mismo preferenceId en la base de datos
        var existingOrder = await _context.Orders.FirstOrDefaultAsync(o => o.PreferenceId == request.Metadata["preference_id"].ToString());

        if (existingOrder != null)
        {
            // Manejar la existencia de la preferencia, por ejemplo, lanzar una excepción o actualizar la existente si es necesario
            throw new Exception($"Preference with ID {request.Metadata["preference_id"]} already exists.");
        }


        var client = new PreferenceClient();
        try
        {
            Console.WriteLine("Creating MercadoPago preference request...");
            Preference preference = await client.CreateAsync(request);
            Console.WriteLine($"Preference created with ID: {preference.Id}");

            request.Metadata["preference_id"] = preference.Id.ToString(); // Actualizar la metadata con el ID real
            await client.UpdateAsync(preference.Id, request);

            // Crear la orden con la preferencia correctamente asignada
            var order = new Order
            {
                UserId = userId,
                PreferenceId = preference.Id.ToString(),
                Status = "Pending",
                ProductQuantity = items.Sum(item => item.Quantity),
                OrderLines = items.Select(item => new OrderLine
                {
                    Description = item.Name,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price,
                    ColorId = item.ColorId,
                    SizeId = item.SizeId
                }).ToList()
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
