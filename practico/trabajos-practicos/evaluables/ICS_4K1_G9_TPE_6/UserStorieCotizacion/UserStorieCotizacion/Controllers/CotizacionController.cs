using Microsoft.AspNetCore.Mvc;
using UserStorieCotizacion.Models;
using UserStorieCotizacion.Services;
using UserStorieCotizacion.Models.Response;
using UserStorieCotizacion.Models.Request;
using Microsoft.AspNetCore.Authorization;

namespace UserStorieCotizacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    // al implementar JWT con este authorize me aseguro que nadie pueda acceder a este controller sin tener un token
    //tenes que mandar el token en los encabezados para que funcione cualquiera de estos endpoints
    //en postman para probarlo mas a Authorization, Bearer Token y pegas el token


   // [Authorize] 

    public class CotizacionController : ControllerBase
    {
        private readonly CotizacionService _cotizacionService;

        public CotizacionController(CotizacionService cotizacionService)
        {
            _cotizacionService = cotizacionService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            Respuesta respuesta = new Respuesta();

            try
            {
                var cotizaciones = _cotizacionService.GetAllCotizaciones();
                respuesta.Exito = 1;
                respuesta.Data = cotizaciones;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }

            return Ok(respuesta);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {

            Respuesta respuesta = new Respuesta();
            try
            {
                var cotizacion = _cotizacionService.GetCotizacionById(id);
                if (cotizacion == null)
                    return NotFound();
                respuesta.Exito = 1;
                respuesta.Data = cotizacion;
               
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }
            return Ok(respuesta);


        }

        [HttpGet("by-persona/{personaId}")]
        public IActionResult GetCotizacionesByPersonaId(long personaId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                var cotizaciones = _cotizacionService.GetCotizacionesByPersonaId(personaId);
                if (cotizaciones == null || !cotizaciones.Any())
                    return NotFound();

                respuesta.Exito = 1;
                respuesta.Data = cotizaciones;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }
            return Ok(respuesta);
        }





        [HttpPost]
        public IActionResult Post(CotizacionRequest request)
        {
            Cotizacion nuevaCotizacion = new Cotizacion
            {
                PersonaId = request.PersonaId,
                FechaRetiro = request.FechaRetiro,
                FechaEntrega = request.FechaEntrega,
                Importe = request.Importe,
                Estado = request.Estado,
                FormaPagoEstablecida = request.FormaPagoEstablecida
            };

            _cotizacionService.CreateCotizacion(nuevaCotizacion);

            return Ok(nuevaCotizacion);
        }

        [HttpPut("{id}")]
        public IActionResult Put(long id, CotizacionRequest request)
        {
            var cotizacion = _cotizacionService.GetCotizacionById(id);

            if (cotizacion == null)
                return NotFound();

            cotizacion.FechaRetiro = request.FechaRetiro;
            cotizacion.FechaEntrega = request.FechaEntrega;
            cotizacion.Importe = request.Importe;
            cotizacion.Estado = request.Estado;
            cotizacion.FormaPagoEstablecida = request.FormaPagoEstablecida;

            _cotizacionService.UpdateCotizacion(cotizacion);

            return Ok(cotizacion);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _cotizacionService.DeleteCotizacion(id);
            return Ok();
        }








        [HttpGet("by-pedido/{pedidoId}")]
        public IActionResult GetCotizacionesByPedidoId(long pedidoId)
        {
            Respuesta respuesta = new Respuesta();

            try
            {
                var cotizaciones = _cotizacionService.GetCotizacionesByPedidoId(pedidoId);
                if (cotizaciones == null || !cotizaciones.Any())
                {
                    respuesta.Exito = 0;
                    respuesta.Mensaje = "No se encontraron cotizaciones para este pedido.";
                }
                else
                {
                    respuesta.Exito = 1;
                    respuesta.Data = cotizaciones;
                }
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }

            return Ok(respuesta);
        }






    }
}
