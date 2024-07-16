using Back_End_TPI_PSS.Data.Models;
using Back_End_TPI_PSS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Back_End_TPI_PSS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPut("subscribe")]
        public IActionResult Subscribe([FromBody] string email)
        {
            try
            {
                _emailService.Subscribe(email);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
                
        }

        [HttpPut("unsubscribe")]
        public IActionResult UnSubscribe([FromBody] string email)
        {
            try
            {
                _emailService.UnSubscribe(email);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
    }
}
