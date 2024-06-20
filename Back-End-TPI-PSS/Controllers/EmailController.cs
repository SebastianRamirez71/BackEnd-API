using Back_End_TPI_PSS.Data.Models;
using Back_End_TPI_PSS.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Back_End_TPI_PSS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }
        [HttpPost("SendEmail")]
        public IActionResult SendEmail()
        {
            //_emailService.Notify(); // va en productService - cuando se agregue un producto con precio "bajo"
            return Ok();
        }

        [HttpPost("Subscribe")]
        public IActionResult Subscribe([FromBody]string email)
        {
            _emailService.Subscribe(email);
            return Ok();
        }

        [HttpDelete("UnSubscribe")]
        public IActionResult UnSubscribe([FromBody]string email)
        {
            _emailService.UnSubscribe(email);
            return Ok();
        }
    }
}
