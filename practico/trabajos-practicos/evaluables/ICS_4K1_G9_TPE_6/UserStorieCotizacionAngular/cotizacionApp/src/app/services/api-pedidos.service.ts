import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Response } from '../models/response';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json'
  })
};

@Injectable({
  providedIn: 'root'
})
export class ApiPedidosService {

  url: string = 'https://localhost:7147/api/Pedido';

  constructor(
    private _http: HttpClient
  ) {}

  // Cambia este método si tienes un endpoint específico para obtener los pedidos de una persona por su ID
  getPedidosByPersonaId(personaId: number): Observable<Response> {
    return this._http.get<Response>(`${this.url}/by-persona/${personaId}`, httpOptions);
  }

 // Nuevo método para obtener la cotización por pedidoId
consultarCotizacion(pedidoId: number): Observable<Response> {
    return this._http.get<Response>(`${this.url}/${pedidoId}/cotizaciones`, httpOptions); // Ajustar la URL
  }
}