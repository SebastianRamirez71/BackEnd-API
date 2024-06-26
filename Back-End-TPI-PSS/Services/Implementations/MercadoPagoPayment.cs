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
            MercadoPagoConfig.AccessToken = "APP_USR-4331649052758365-062518-799e38d9934a761bb3caff58a146a352-277825282";
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
            };

            var client = new PreferenceClient();
            Preference preference = await client.CreateAsync(request);
            return preference;
        }
    }
}
