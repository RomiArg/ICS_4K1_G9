import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";
import { Response } from "../models/response";
import { Usuario } from "../models/usuario";
import { map } from "rxjs/operators";

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json'
  })
};

@Injectable({
  providedIn: 'root'
})
export class ApiAuthService {
  url: string = 'https://localhost:7147/api/Usuario/login';

  private usuarioSubject!: BehaviorSubject<Usuario | null>; // Permitir valores null

  constructor(
    private _http: HttpClient
  ) {
    const storedUsuario = localStorage.getItem('usuario');
    this.usuarioSubject = new BehaviorSubject<Usuario | null>(
      storedUsuario ? JSON.parse(storedUsuario) : null
    );
  }

  login(email: string, password: string): Observable<Response> {
    return this._http.post<Response>(this.url, { email, password }, httpOptions).pipe(
      map(res => {
        if (res.exito === 1) {
          const usuario: Usuario = res.data;
          localStorage.setItem('usuario', JSON.stringify(usuario));
          this.usuarioSubject.next(usuario);
        }
        return res;
      })
    );
  }

  logout() {
    localStorage.removeItem('usuario');
    this.usuarioSubject.next(null); // Esto ya no causará un error
  }

  get usuarioData(): Usuario | null {
    return this.usuarioSubject.value;
  }
}
