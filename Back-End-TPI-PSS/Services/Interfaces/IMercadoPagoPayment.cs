using Back_End_TPI_PSS.Models;
using MercadoPago.Client.Preference;
using MercadoPago.Resource.Preference;

namespace Back_End_TPI_PSS.Services.Interfaces
{
    public interface IMercadoPagoPayment
    {
        public Task<Preference> CreatePreferenceRequest(List<CartItem> items);
    }
}
