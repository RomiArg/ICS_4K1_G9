using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserStorieCotizacion.Models;
using UserStorieCotizacion.Models.Request;
using UserStorieCotizacion.Models.Response;
using UserStorieCotizacion.Services;

namespace UserStorieCotizacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    // al implementar JWT con este authorize me aseguro que nadie pueda acceder a este controller sin tener un token
    //tenes que mandar el token en los encabezados para que funcione cualquiera de estos endpoints
    //en postman para probarlo mas a Authorization, Bearer Token y pegas el token


    //[Authorize] 
    public class PagoController : ControllerBase
    {
        private readonly PagoService _pagoService;

        public PagoController(PagoService pagoService)
        {
            _pagoService = pagoService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            Respuesta respuesta = new Respuesta();

            try
            {
                var pagos = _pagoService.GetAllPagos();
                respuesta.Exito = 1;
                respuesta.Data = pagos;
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
            var pago = _pagoService.GetPagoById(id);
            if (pago == null)
                return NotFound();

            return Ok(pago);
        }

        [HttpPost]
        public IActionResult Post(PagoRequest pagoRequest)
        {
            Pago nuevoPago = new Pago
            {
                CotizacionId = pagoRequest.CotizacionId,
                FormaPagoId = pagoRequest.FormaPagoId,
                EstadoPago = pagoRequest.EstadoPago,
                FechaPago = pagoRequest.FechaPago,
                NumeroDePagoDePasarelaDePago = pagoRequest.NumeroDePagoDePasarelaDePago,
           
            };
            

            _pagoService.CreatePago(nuevoPago);
            return Ok(nuevoPago);
        }

        [HttpPut("{id}")]
        public IActionResult Put(long id, Pago pagoActualizado)
        {
            var pago = _pagoService.GetPagoById(id);
            if (pago == null)
                return NotFound();

            pago.CotizacionId = pagoActualizado.CotizacionId;
            pago.FormaPagoId = pagoActualizado.FormaPagoId;
            pago.EstadoPago = pagoActualizado.EstadoPago;
            pago.FechaPago = pagoActualizado.FechaPago;
            pago.NumeroDePagoDePasarelaDePago = pagoActualizado.NumeroDePagoDePasarelaDePago;

            _pagoService.UpdatePago(pago);
            return Ok(pago);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _pagoService.DeletePago(id);
            return Ok();
        }
    }
}
