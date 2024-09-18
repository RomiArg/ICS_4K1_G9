import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NodeJsService {
  private apiUrl = 'https://localhost:7147/MercadoPago'; // Ajusta la URL según tu configuración

  constructor(private http: HttpClient) { }

  // Método para crear una preferencia de pago
  createPreference(orderData: any, idUsuario: number, cotizacionId: number): Observable<any> {
    console.log('Datos enviados a createPreference:', orderData);
    
    const url = `${this.apiUrl}/create_preference`;

    // Establecemos los parámetros idUsuario y cotizacionId en la llamada HTTP
    const params = new HttpParams()
      .set('idUsuario', idUsuario.toString())
      .set('cotizacionId', cotizacionId.toString());

    // Hacemos la petición POST y devolvemos la respuesta
    return this.http.post(url, orderData, { params });
  }


  // Método para procesar el pago en efectivo
  processEfectivoPayment(idUsuario: number, cotizacionId: number, formaDePago: string): Observable<any> {
    const url = `${this.apiUrl}/pagoEfectivo`;

    // Establecemos los parámetros idUsuario, cotizacionId y formaDePago en la llamada HTTP
    const params = new HttpParams()
      .set('idUsuario', idUsuario.toString())
      .set('cotizacionId', cotizacionId.toString())
      .set('formaDePago', formaDePago);

    // Hacemos la petición POST y devolvemos la respuesta
    return this.http.post(url, {}, { params });
  }
}
