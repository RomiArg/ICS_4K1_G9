import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiAuthService } from '../services/apiauth.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

    constructor(private apiAuthService: ApiAuthService) {}

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        // Obtén el usuario y el token desde el ApiAuthService
        const usuario = this.apiAuthService.usuarioData;

        // Si el usuario está autenticado, clona la solicitud e incluye el token en las cabeceras
        if (usuario && usuario.token) {
            request = request.clone({
                setHeaders: {
                    Authorization: `Bearer ${usuario.token}` // Envía el token como cabecera Authorization
                }
            });
        }

        // Continúa la ejecución de la petición
        return next.handle(request);
    }
}
