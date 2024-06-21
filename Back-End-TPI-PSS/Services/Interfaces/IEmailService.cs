using Back_End_TPI_PSS.Data.Entities;
using Back_End_TPI_PSS.Data.Models;
using Back_End_TPI_PSS.Data.Models.ProductDTOs;

namespace Back_End_TPI_PSS.Services.Interfaces
{
    public interface IEmailService
    {
        string ErrorDescription { get; set; }
        public void Notify(Product productDto);
        public void SendEmail(Product product, User user);
        void Subscribe(string email);
        void UnSubscribe(string email);
    }
}
