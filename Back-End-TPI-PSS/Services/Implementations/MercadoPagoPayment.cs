using Back_End_TPI_PSS.Models;
using Back_End_TPI_PSS.Services.Interfaces;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
namespace Back_End_TPI_PSS.Services.Implementations
{
    public class MercadoPagoPayment : IMercadoPagoPayment
    {
        public MercadoPagoPayment()
        {
            // Test: APP_USR-4870971039960-062618-60e3119bca2338c0da52557538693711-1872136931
            // Prod: APP_USR-4331649052758365-062518-799e38d9934a761bb3caff58a146a352-277825282
            MercadoPagoConfig.AccessToken = "APP_USR-4870971039960-062618-60e3119bca2338c0da52557538693711-1872136931";
        }
        public async Task<Preference> CreatePreferenceRequest(List<CartItem> items)
        {
            var request = new PreferenceRequest
            {
                Items = items.Select(item => new PreferenceItemRequest
                {
                    Title = item.Name,
                    Description = $"Color: {item.Color}, Size: {item.SizeId}",
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
                NotificationUrl = "https://tu-domino.com/api/mercadopago/webhook", // URL de tu endpoint de MercadoPago

                // Agregar metadata
                Metadata = new Dictionary<string, object>
                {
                    { "preference_id", Guid.NewGuid().ToString() } // Generar un ID único para preference_id
                }
            };

            var client = new PreferenceClient();
            Preference preference = await client.CreateAsync(request);

            // Actualizar el preference_id con el ID de la preferencia creada
            request.Metadata["preference_id"] = preference.Id.ToString();

            // Actualizar la preferencia con el nuevo metadata
            await client.UpdateAsync(preference.Id, request);

            return preference;

        }
    }
}