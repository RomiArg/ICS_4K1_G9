using Microsoft.AspNetCore.SignalR;

namespace UserStorieCotizacion.Hubs
{
    public class NotificacionesHub : Hub
    {
        // Métodos que se utilizarán para las notificaciones
        public async Task EnviarNotificacion(string userId, string mensaje)
        {
            // Enviar mensaje al cliente específico
            await Clients.User(userId).SendAsync("RecibirNotificacion", mensaje);
        }

        public async Task TestConnection()
{
    await Clients.All.SendAsync("RecibirNotificacion", "SignalR está funcionando.");
}

    }
}
