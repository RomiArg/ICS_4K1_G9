using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UserStorieCotizacion.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;  
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using UserStorieCotizacion.Models;
using UserStorieCotizacion.Services;
using Microsoft.AspNetCore.SignalR;
using UserStorieCotizacion.Hubs;





[ApiController]
[Route("[controller]")]
public class MercadoPagoController : ControllerBase
{
    private const string MercadoPagoUrl = "https://api.mercadopago.com";
    //private const string AccessToken = "APP_USR-5601931937062244-010714-fbc047a78341499e9f70f8d4a81e7219-1626343037";
    private const string AccessToken = "APP_USR-5601931937062244-010714-fbc047a78341499e9f70f8d4a81e7219-1626343037";


    private readonly ISWContext _dbContext;
    private readonly IHubContext<NotificacionesHub> _hubContext; 
    private readonly IEmailService _emailService;

    public MercadoPagoController(ISWContext context, IHubContext<NotificacionesHub> hubContext, IEmailService emailService)
    {
        _dbContext = context;
        _hubContext = hubContext; 
        _emailService = emailService;
    }



    [HttpPost("pagoEfectivo")]
    public async Task<IActionResult> PagoEfectivo(int idUsuario, int cotizacionId, string formaDePago)
    {
        try
        {
            // Buscar la cotización por idCotizacion
            var cotizacion = await _dbContext.Cotizaciones.FirstOrDefaultAsync(c => c.CotizacionId == cotizacionId);

            if (cotizacion == null)
            {
                return NotFound(new { Message = "No se encontró la cotización con el ID proporcionado." });
            }

            if (cotizacion.Estado != "Pendiente")
            {
                return NotFound(new { Message = "La cotización no esta Pendiente, no puedes realizar el pago." });
            }

            // Cambiar el estado de la cotización a "Confirmada"
            cotizacion.Estado = "Confirmada";

            // Establecer la forma de pago según el parámetro formaDePago
            if (formaDePago == "Efectivo al entregar")
            {
                cotizacion.FormaPagoEstablecida = "Efectivo al entregar";
            }
            else if (formaDePago == "Efectivo al retirar")
            {
                cotizacion.FormaPagoEstablecida = "Efectivo al retirar";
            }
            else
            {
                cotizacion.FormaPagoEstablecida = "Efectivo";
            }

            // Guardar los cambios en la base de datos para la cotización confirmada
            await _dbContext.SaveChangesAsync();

            //  cotizaciones con el mismo PedidoId que estén en estado "Pendiente",
            // excluyendo la cotiza actual que acaba de ser confirmada
            var cotizacionesRelacionadas = await _dbContext.Cotizaciones
                .Where(c => c.PedidoId == cotizacion.PedidoId && c.Estado == "Pendiente" && c.CotizacionId != cotizacionId)
                .ToListAsync();

            // Cambiar el estado de las cotizaciones relacionadas a "Cancelada"
            foreach (var cot in cotizacionesRelacionadas)
            {
                cot.Estado = "Cancelada";
            }

            // Guardar los cambios en la base de datos para las cotizaciones canceladas
            await _dbContext.SaveChangesAsync();

            // Obtener el pedido asociado usando el PedidoId de la cotización
            var pedido = await _dbContext.Pedidos.FirstOrDefaultAsync(p => p.PedidoId == cotizacion.PedidoId);

            if (pedido != null)
            {
                // Cambiar el estado del pedido a "Confirmado"
                pedido.EstadoPedido = "Confirmado";

                // Guardar los cambios en la base de datos para el pedido confirmado
                await _dbContext.SaveChangesAsync();
            }

            // Obtener el ID de la persona (transportista) asociada a la cotización
            var idTransportista = cotizacion.PersonaId;

            // Buscar la persona (transportista) por su ID
            var transportista = await _dbContext.Personas.FirstOrDefaultAsync(p => p.PersonaId == idTransportista);

            if (transportista != null)
            {
                // email del transportista
                var emailTransportista = transportista.Email;

                //  utilizamos el SignalR Hub para enviar una notificación al transportista
                var mensajeNotificacion = $"Se confirmó el pago de la cotización {cotizacion.CotizacionId} del transportista {idTransportista}.";

                // Convertir el ID de la persona (transportista) a string, ya que SignalR trabaja con cadenas para los User IDs
                var transportistaId = transportista.PersonaId.ToString();

                // Enviar la notificación a través del HubContext
                //  await _hubContext.Clients.User(transportistaId).SendAsync("RecibirNotificacion", mensajeNotificacion);
                await _hubContext.Clients.All.SendAsync("RecibirNotificacion", mensajeNotificacion);



                //aca empieza lo del envio de email

                try
                {
                    // Guardar los cambios en la base de datos
                    await _dbContext.SaveChangesAsync();
                    _emailService.SendWelcomeEmail(emailTransportista, transportista.Nombre, cotizacion.CotizacionId);
                    return Ok(new { Message = "Email de bienvenida enviado." });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { Message = "Error al enviar el correo electrónico." });
                }

            }
            else
            {
                return StatusCode(500, new { Message = "Error transportista null" });

            }
        }
        catch (Exception ex)
        {
            // Manejo de errores
            return StatusCode(500, new { Message = "Ocurrió un error al procesar el pago en efectivo.", Error = ex.Message });
        }
    }


    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook()
    {
        try
        {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                string requestBody = await reader.ReadToEndAsync();

                // deserializamos el JSON en un objeto C#
                var notification = Newtonsoft.Json.JsonConvert.DeserializeObject<NotificationModel>(requestBody);

                // verifico si notification y notification.Data no son nulos
                if (notification != null && notification.Data != null)
                {
                    // Acceder a las propiedades del objeto C# según la estructura del JSON
                    var action = notification.Action;
                    var dataId = notification.Data.Id;

                    // verifi si es una notificación de pago creado
                    if (action == "payment.created")
                    {
                       

                        // 

                        using (var client = new HttpClient())
                        {
                            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {AccessToken}");

                            var response = await client.GetAsync($"{MercadoPagoUrl}/v1/payments/{dataId}");

                            if (response.IsSuccessStatusCode)
                            {
                                var responseData = await response.Content.ReadAsStringAsync();
                                var paymentInfo = JObject.Parse(responseData);

                                var estado = paymentInfo["status"].ToString();
                                var fechaTransaccion = paymentInfo["date_created"].ToString();
                                var montoTransaccion = decimal.Parse(paymentInfo["transaction_amount"].ToString());
                                var nombrePagador = paymentInfo["payer"]["first_name"].ToString();
                                var dateApproved = paymentInfo["date_approved"]?.ToString();
                                var transactionDetails = paymentInfo["transaction_details"];
                                var externalReference = paymentInfo["external_reference"]?.ToString();
                                //EXCELENTEEEEEEEEEEEE 
                                // 

                                // verifico si el estado es aprobado
                                if (estado != "approved")
                                {
                                    return BadRequest(new { Error = "El pago no fue aprobado por MercadoPago." });
                                }

                                externalReference = externalReference.Replace("\\", "").Replace("\"", "");






                                //busca el externalReference en la base de datos
                                var idPago = Convert.ToInt32(externalReference);
                                var pagoEncontrado = await _dbContext.Pagos.FirstOrDefaultAsync(p => p.PagoId == idPago);

                                if (pagoEncontrado == null)
                                {
                                    //si entra aca signfica que ese idTransaccion que puso el usuario o no existe y no tiene un externalid o ya fue removido de la bd porq ya paso por aca, y fue pagado.
                                    return BadRequest(new { Error = "El externalReference no corresponde a un pago registrado o el pago ya fue registrado anteriormente." });
                                }

                                

                                if (pagoEncontrado.EstadoPago != "Pendiente")
                                {
                                    return BadRequest(new { Error = "Esta transaccion fue finalizada." });
                                }
                                // Actualizar el estado de la transacción a "Pagado"
                                pagoEncontrado.EstadoPago = "Pagado";
                                pagoEncontrado.NumeroDePagoDePasarelaDePago = long.Parse(dataId);

                                // Guardar los cambios en la base de datos
                                await _dbContext.SaveChangesAsync();




                                if (pagoEncontrado.EstadoPago == "Pagado")
                                {
                                    var idCotizacion = pagoEncontrado.CotizacionId;

                                    // Buscar la cotización por idCotizacion
                                    var cotizacion = await _dbContext.Cotizaciones.FirstOrDefaultAsync(c => c.CotizacionId == idCotizacion);

                                    if (cotizacion != null)
                                    {

                                        if (cotizacion.Estado != "Pendiente")
                                        {
                                            return NotFound(new { Message = "La cotización no esta Pendiente, no puedes realizar el pago." });
                                        }


                                        // Cambiar el estado de la cotización a "Confirmada" y la forma de pago en este caso debito
                                        cotizacion.Estado = "Confirmada";
                                        cotizacion.FormaPagoEstablecida = "Debito";

                                        // busca todas las cotizaciones con el mismo PedidoId que estén en estado "Pendiente",
                                        // excluyendo la cotización actual que acaba de ser confirmada
                                        var cotizacionesRelacionadas = await _dbContext.Cotizaciones
                                            .Where(c => c.PedidoId == cotizacion.PedidoId && c.Estado == "Pendiente" && c.CotizacionId != idCotizacion)
                                            .ToListAsync();

                                        // Cambiar el estado de las cotizaciones relacionadas a "Cancelada"
                                        foreach (var cot in cotizacionesRelacionadas)
                                        {
                                            cot.Estado = "Cancelada";
                                        }

                                        // Guardar los cambios en la base de datos
                                        await _dbContext.SaveChangesAsync();




                                        // Obtener el pedido asociado usando el pedidoAsociadoId de la cotización
                                        var pedido = await _dbContext.Pedidos.FirstOrDefaultAsync(p => p.PedidoId == cotizacion.PedidoId);

                                        if (pedido != null)
                                        {
                                            // cambiar el estado del pedido a "Confirmado"
                                            pedido.EstadoPedido = "Confirmado";

                                            // obtener el ID de la persona (transportista) asociada a la cotización
                                            var idTransportista = cotizacion.PersonaId;

                                            // Buscar la persona (transportista) por su ID
                                            var transportista = await _dbContext.Personas.FirstOrDefaultAsync(p => p.PersonaId == idTransportista);

                                            if (transportista != null)
                                            {
                                                // obtener el email del transportista
                                                var emailTransportista = transportista.Email;


                                               

                                                // utilizamos el SignalR Hub para enviar una notificación al transportista
                                                var mensajeNotificacion = $"Se confirmó el pago de la cotización {cotizacion.CotizacionId} del transportista {idTransportista}.";

                                                // convertimos el ID de la persona (transportista) a string, ya que SignalR trabaja con cadenas para los User IDs
                                                var transportistaId = transportista.PersonaId.ToString();

                                                // Enviar la notificación a través del HubContext
                                              //  await _hubContext.Clients.User(transportistaId).SendAsync("RecibirNotificacion", mensajeNotificacion);
                                                await _hubContext.Clients.All.SendAsync("RecibirNotificacion", mensajeNotificacion);



                                                //aca empieza lo del envio de email

                                                try
                                                {
                                                    // guarda los cambios en la base de datos
                                                    await _dbContext.SaveChangesAsync();
                                                    _emailService.SendWelcomeEmail(emailTransportista, transportista.Nombre, cotizacion.CotizacionId);
                                                    return Ok(new { Message = "Email de bienvenida enviado." });
                                                }
                                                catch (Exception ex)
                                                {
                                                    return StatusCode(500, new { Message = "Error al enviar el correo electrónico." });
                                                }



                                            }

                                            
                                        }
                                        else
                                        {
                                            // Manejo de error si no se encuentra el pedido
                                            throw new Exception("No se encontró el pedido asociado a la cotización.");
                                        }
                                    }
                                    else
                                    {
                                        // Manejo de error si no se encuentra la cotización
                                        throw new Exception("No se encontró la cotización con el ID proporcionado.");
                                    }
                                }

                                







                               


                                //  resultado exitoso
                                return Ok(new { Status = "Pago procesado correctamente", PagoExitoso = true });
                            }
                        }
                    }
                }
            }

            // devolvemos un resultado exitoso si no se cumple ninguna condición especial
            return Ok(new { Status = "Webhook procesado correctamente" });
        }
        catch (Exception ex)
        {
            //  _logger.LogError($"Error en el webhook: {ex.Message}");
            Console.WriteLine($"Error en el webhook: {ex.Message}");
            return StatusCode(500, new { error = "Error en el webhook", details = ex.Message });
        }
    }















    [HttpPost("create_preference")]
    public async Task<ActionResult<string>> CreatePreference([FromBody] OrderData orderData, int idUsuario, int cotizacionId)
    {
        try
        {
            // Buscar la cotización por idCotizacion
            var cotizacion = await _dbContext.Cotizaciones.FirstOrDefaultAsync(c => c.CotizacionId == cotizacionId);

            if (cotizacion.Estado != "Pendiente")
            {
                return NotFound(new { Message = "La cotizacion no esta disponible para pagar." });
            }


            using (var client = new HttpClient())
            {

                var nuevoPago = new Pago
                {


                    //PagoId = nextIdPago,
                    CotizacionId = cotizacionId,
                    FormaPagoId = 1,
                    EstadoPago = "Pendiente",
                    FechaPago = DateTime.Now,
                    NumeroDePagoDePasarelaDePago = null,



                };

                _dbContext.Pagos.Add(nuevoPago);

                await _dbContext.SaveChangesAsync();


                //var nextIdPago = _dbContext.Pagos.Select(p => p.PagoId).DefaultIfEmpty().Max() + 1;
                var nextIdPago = nuevoPago.PagoId; // este nextIdPago se lo asigno mas abajo al external reference

                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {AccessToken}");

                var response = await client.PostAsJsonAsync($"{MercadoPagoUrl}/checkout/preferences", new
                {
                    items = new[]
                    {
                    new
                    {
                        title = orderData.Title,
                        quantity = orderData.Quantity,
                        unit_price = orderData.Price,
                        currency_id = "ARS",
                    }
                },
                    back_urls = new
                    {
                        // success = "https://tn.com.ar/",
                        //failure = "https://www.cronista.com/",
                        //pending = "http://localhost:4200/comprar",

                        success = "http://localhost:4200/pedidos",
                        failure = "http://localhost:4200/cotizacion",
                        pending = "http://localhost:4200/pedidos",
                    },
                    auto_return = "approved",
                    notification_url = "https://6cdc-201-231-67-242.ngrok-free.app/MercadoPago/webhook", // Agrega tu URL de Webhook aquí STEPHAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAN
                    external_reference = Convert.ToInt32(nextIdPago),
                });

                var responseData = await response.Content.ReadAsStringAsync();
                var result = JObject.Parse(responseData);

                // Verifica si el objeto tiene la clave "id" y "init_point"
                if (result["id"] != null && result["init_point"] != null)
                {
                    var preferenceId = result["id"].ToString();
                    var initPoint = result["init_point"].ToString();

                    return Ok(new { id = preferenceId, url = initPoint });
                }
                else
                {
                    // OJO ACA, una vez me paso que entro aca porq el server de mercadopago estaba caido jaja
                    // maneja el caso en el que no se reciban los datos esperados
                    return StatusCode(500, new { error = "Error en la respuesta de MercadoPago", details = "No se encontraron los campos 'id' o 'init_point' en la respuesta." });
                }

            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Error al crear la preferencia", details = ex.Message });
        }
    }



    [HttpGet("consultar_estado_transaccion")]
    public async Task<ActionResult<string>> ConsultarEstadoTransaccion(string idComprobante)
    {

       
        if (true)
        {
            return Ok();
        }
    }


}



//modelos que necesitaba

public class OrderData
{
    public string Title { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

public class VerificarTransaccionRequest
{
    public string PreferenceId { get; set; }
}


public class NotificationModel
{
    public string Action { get; set; }
    public string ApiVersion { get; set; }
    public NotificationData Data { get; set; }
    public DateTime DateCreated { get; set; }
    public long Id { get; set; }
    public bool LiveMode { get; set; }
    public string Type { get; set; }
    public string UserId { get; set; }
}

public class NotificationData
{
    public string Id { get; set; }
    // Puedes agregar más propiedades según la estructura del JSON
}

