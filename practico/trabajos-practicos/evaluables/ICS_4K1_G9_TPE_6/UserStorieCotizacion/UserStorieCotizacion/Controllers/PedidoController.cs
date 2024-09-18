using Microsoft.AspNetCore.Mvc;
using UserStorieCotizacion.Models;
using UserStorieCotizacion.Models.Request;
using UserStorieCotizacion.Models.Response;
using UserStorieCotizacion.Services;

namespace UserStorieCotizacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly PedidoService _pedidoService;
        private readonly CotizacionService _cotizacionService; // Inyectar CotizacionService

        public PedidoController(PedidoService pedidoService, CotizacionService cotizacionService) // Añadir CotizacionService al constructor
        {
            _pedidoService = pedidoService;
            _cotizacionService = cotizacionService; // Asignar CotizacionService
        }

        [HttpGet]
        public IActionResult Get()
        {
            Respuesta respuesta = new Respuesta();

            try
            {
                var pedidos = _pedidoService.GetAllPedidos();
                respuesta.Exito = 1;
                respuesta.Data = pedidos;
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
            var pedido = _pedidoService.GetPedidoById(id);
            if (pedido == null)
                return NotFound();

            return Ok(pedido);
        }

        [HttpGet("by-persona/{personaId}")]
        public IActionResult GetByPersonaId(long personaId)
        {
            Respuesta respuesta = new Respuesta();

            try
            {
                var pedidos = _pedidoService.GetPedidosByPersonaId(personaId);
                if (pedidos == null || !pedidos.Any())
                {
                    respuesta.Exito = 0; // Indicar que la operación no tuvo éxito
                    respuesta.Mensaje = "No se encontraron pedidos.";
                }
                else
                {
                    respuesta.Exito = 1; // Indicar que la operación tuvo éxito
                    respuesta.Data = pedidos;
                }
            }
            catch (Exception ex)
            {
                respuesta.Exito = 0; // Indicar que la operación no tuvo éxito
                respuesta.Mensaje = ex.Message;
            }

            return Ok(respuesta);
        }




        [HttpPost]
        public IActionResult Post(PedidoRequest pedidoRequest)
        {
            Pedido nuevoPedido = new Pedido
            {
                PersonaId = pedidoRequest.PersonaId,
                EstadoPedido = pedidoRequest.EstadoPedido,
                DomicilioRetiro = pedidoRequest.DomicilioRetiro,
                FechaRetiro = pedidoRequest.FechaRetiro,
                DomicilioEntrega = pedidoRequest.DomicilioEntrega,
                FechaEntrega = pedidoRequest.FechaEntrega,
            };

            _pedidoService.CreatePedido(nuevoPedido);
            return Ok(nuevoPedido);
        }

        [HttpPut("{id}")]
        public IActionResult Put(long id, Pedido pedidoActualizado)
        {
            var pedido = _pedidoService.GetPedidoById(id);
            if (pedido == null)
                return NotFound();

            pedido.PersonaId = pedidoActualizado.PersonaId;
            pedido.EstadoPedido = pedidoActualizado.EstadoPedido;
            pedido.DomicilioRetiro = pedidoActualizado.DomicilioRetiro;
            pedido.FechaRetiro = pedidoActualizado.FechaRetiro;
            pedido.DomicilioEntrega = pedidoActualizado.DomicilioEntrega;
            pedido.FechaEntrega = pedidoActualizado.FechaEntrega;

            _pedidoService.UpdatePedido(pedido);
            return Ok(pedido);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _pedidoService.DeletePedido(id);
            return Ok();
        }



        [HttpGet("{id}/cotizaciones")]
        public IActionResult GetCotizacionesByPedidoId(long id)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                var cotizaciones = _cotizacionService.GetCotizacionesByPedidoId(id);
                if (!cotizaciones.Any())
                {
                    respuesta.Exito = 0;
                    respuesta.Mensaje = "No se encontraron cotizaciones para este pedido.";
                }
                else
                {
                    respuesta.Exito = 1;
                    respuesta.Mensaje = "Se encontraron cotizaciones para este pedido.";
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
