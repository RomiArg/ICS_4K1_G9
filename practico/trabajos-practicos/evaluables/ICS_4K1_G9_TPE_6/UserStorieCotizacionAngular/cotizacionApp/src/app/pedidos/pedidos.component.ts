import { Component, OnInit } from '@angular/core';
import { MatSidenavModule } from '@angular/material/sidenav'; 
import { ApiPedidosService } from '../services/api-pedidos.service';
import { ApiAuthService } from '../services/apiauth.service'; // Servicio de autenticación
import { ApiCotizacionesService } from '../services/api-cotizaciones.service'; // Servicio de autenticación
import { Response } from '../models/response';
import { Cotizacion } from '../models/cotizacion';
import { Pedido } from '../models/pedido'; // Importar la interface Pedido
import { CommonModule } from '@angular/common'; // Importar CommonModule para *ngFor
import { MatTableModule } from '@angular/material/table';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCard, MatCardModule } from '@angular/material/card';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { MatToolbar } from '@angular/material/toolbar';
import { AlertDialogComponent } from '../alert-dialog/alert-dialog.component';

@Component({
  selector: 'app-pedidos',
  standalone: true,
  imports: [MatCard,MatToolbar,MatSidenavModule, CommonModule, MatSidenavModule, MatTableModule, MatInputModule, MatButtonModule, MatDialogModule, MatSnackBarModule, MatCardModule, FormsModule],
  templateUrl: './pedidos.component.html',
  styleUrls: ['./pedidos.component.css']
})
export class PedidosComponent implements OnInit {

  public lst: Pedido[] | undefined; // Usar Pedido[] en lugar de any[]
  public pedidosPendientes: Pedido[] = []; // Usar Pedido[] en lugar de any[]
  public pedidosConfirmados: Pedido[] = []; // Usar Pedido[] en lugar de any[]
  public displayedColumns: string[] = [
    'pedidoId', 
    'estadoPedido', 
    'domicilioRetiro', 
    'fechaRetiro', 
    'domicilioEntrega', 
    'fechaEntrega',
    'consultarCotizacion'
  ];

  public cotizaciones: Cotizacion[] = []; // Array para almacenar las cotizaciones
  public usuario: any; // Almacenar datos del usuario

  constructor(
    private apiPedido: ApiPedidosService,
    private apiAuth: ApiAuthService,// Inyectar servicio de autenticación
    private router: Router,
    private apiCotizacion: ApiCotizacionesService,
    private dialog: MatDialog,
  ) {}

  ngOnInit(): void {
    this.usuario = this.apiAuth.usuarioData; // Obtener datos del usuario
    this.getPedidos();
  }

  getPedidos(): void {
    const usuario = this.apiAuth.usuarioData;
    if (usuario) {
      this.apiPedido.getPedidosByPersonaId(usuario.id).subscribe(response => {
        if (response.exito === 1) {
          const pedidos: Pedido[] = response.data; // Asegurarse de que los datos sean un array de Pedido
          this.pedidosPendientes = pedidos.filter(pedido => pedido.estadoPedido === 'Pendiente');
          this.pedidosConfirmados = pedidos.filter(pedido => pedido.estadoPedido === 'Confirmado');
        } else {
          // Manejo de error o mensaje de usuario
          console.error(response.mensaje);
        }
      });
    }
  }

  consultarCotizacion(pedidoId: number): void {
    this.apiPedido.consultarCotizacion(pedidoId).subscribe(response => {
      if (response.exito === 1) {
        this.cotizaciones = response.data;
        this.router.navigate(['/cotizaciones'], { queryParams: { pedidoId } });
      } else {
        console.error(response.mensaje);
      }
    });
  }

  consultarEstadoPedido(pedidoId: number): void {
   

   this.apiPedido.consultarCotizacion(pedidoId).subscribe(response => {
    if (response.exito === 1) {
      this.cotizaciones = response.data;
      this.router.navigate(['/detallePedido'], { queryParams: { pedidoId } });
    } else {
      console.error(response.mensaje);
    }
  });
  }

  cerrarSesion(): void {
    this.apiAuth.logout();
    this.router.navigate(['/login']);
    
  }

  abrirAlerta(mensaje: string, navigateOnClose: boolean): void {
    this.dialog.open(AlertDialogComponent, {
      data: {
        message: mensaje,
        type: 'error',
        navigateOnClose: navigateOnClose
      }
    });
  }

  registrarPedido(): void {
    // Aquí navega después de cerrar
    this.abrirAlerta('Esta funcionalidad no pertenece a la UserStorie provista', true);
  }

  menuPrincipal(): void {
    // Aquí no navega después de cerrar
    this.abrirAlerta('Esta funcionalidad no pertenece a la UserStorie provista', false);
  }
}