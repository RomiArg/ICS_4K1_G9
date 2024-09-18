import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  public hubConnection: HubConnection | undefined;

  constructor() {
    this.startConnection();
    this.registerOnServerEvents();
  }

  private startConnection() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('https://localhost:7147/notificacionesHub') // Cambia el URL según sea necesario
      .configureLogging(LogLevel.Information)
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('SignalR conectado papi'))
      .catch(err => console.error('Error al conectar con SignalR: ', err));
  }

  private registerOnServerEvents() {
    if (this.hubConnection) {
      this.hubConnection.on('RecibirNotificacion', (mensaje: string) => {
        console.log('Mensaje recibido: ', mensaje);
        // Aquí puedes emitir el mensaje a otros componentes si lo necesitas
      });
    }
  }
}
