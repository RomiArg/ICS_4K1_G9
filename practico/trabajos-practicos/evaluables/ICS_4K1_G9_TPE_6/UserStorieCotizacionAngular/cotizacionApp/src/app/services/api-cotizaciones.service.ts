import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {Response} from '../models/response'
import { Cotizacion } from '../models/cotizacion';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json'
  })
};



@Injectable({
  providedIn: 'root'
})
export class ApiCotizacionesService {


url: string = 'https://localhost:7147/api/Cotizacion';



  constructor(

    private _http: HttpClient

  ) { }

  getCotizaciones(): Observable<Response> {
    return this._http.get<Response>(this.url, httpOptions)
  }

  

  // Método para obtener las cotizaciones por pedidoId
  getCotizacionesByPedidoId(pedidoId: number): Observable<Response> {
    return this._http.get<Response>(`${this.url}/by-pedido/${pedidoId}`, httpOptions);
  }

    // Método para obtener una cotización por su ID
  getCotizacionById(id: number): Observable<Response> {
    return this._http.get<Response>(`${this.url}/${id}`, httpOptions);
  }


  // Método para obtener las cotizaciones por personaId
getCotizacionesByPersonaId(personaId: number): Observable<Response> {
  return this._http.get<Response>(`${this.url}/by-persona/${personaId}`, httpOptions);
}




  
}

