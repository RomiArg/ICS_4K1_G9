using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using UserStorieCotizacion.Models.Request;
using UserStorieCotizacion.Models.Response;
using UserStorieCotizacion.Services;

namespace UserStorieCotizacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        private IUserService _userService;
        private readonly IEmailService _emailService;
        public UsuarioController(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
           _emailService = emailService;

        }





        [HttpPost("send-welcome-email/{email}/{name}")]
        public IActionResult SendWelcomeEmail(string email, string name, long cotizacionId)
        {
            try
            {
                _emailService.SendWelcomeEmail(email, name,cotizacionId);
                return Ok(new { Message = "Email de bienvenida enviado." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error al enviar el correo electrónico." });
            }
        }




        [HttpPost("login")]
        public IActionResult Autentificar(AuthRequest model)
        {
            Respuesta respuesta = new Respuesta();

            var userResponse = _userService.Auth(model);

            if (userResponse == null)
            {
                respuesta.Exito = 0;
                respuesta.Mensaje = "Usuario o contraseña incorrectos";
                return BadRequest(respuesta);
            }

            respuesta.Exito = 1;
            respuesta.Data = userResponse;

            return Ok(respuesta);
        }

    }
}
