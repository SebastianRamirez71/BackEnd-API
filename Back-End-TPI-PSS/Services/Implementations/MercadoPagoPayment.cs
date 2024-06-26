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
            };

            var client = new PreferenceClient();
            Preference preference = await client.CreateAsync(request);
            return preference;
        }
    }
}
