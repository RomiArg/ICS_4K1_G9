import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar'; // Importar MatSnackBar
import { SignalRService } from '../services/signalr.service';
import { ApiCotizacionesService } from '../services/api-cotizaciones.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiAuthService } from '../services/apiauth.service';
import { Cotizacion } from '../models/cotizacion';
import { CommonModule } from '@angular/common'; // Importar CommonModule para *ngFor
import { FormsModule } from '@angular/forms';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatButtonModule } from '@angular/material/button';
import { MatCard, MatCardModule } from '@angular/material/card';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';
import { MatToolbar } from '@angular/material/toolbar';
import { AlertDialogComponent } from '../alert-dialog/alert-dialog.component';

@Component({
  selector: 'app-cotizaciones-transportista',
  standalone: true,
  imports: [MatSnackBarModule,MatSidenavModule, CommonModule, MatTableModule, MatInputModule, MatButtonModule, MatDialogModule, MatSnackBarModule, MatCardModule, FormsModule, MatToolbar, MatCard],
  templateUrl: './cotizaciones-transportista.component.html',
  styleUrls: ['./cotizaciones-transportista.component.css']
})
export class CotizacionesTransportistaComponent implements OnInit {
  public lst: any[] | undefined;
  public cotizaciones: Cotizacion[] = [];
  public titulo: string = 'Cotizaciones';
  public displayedColumns: string[] = ['cotizacionId', 'fechaRetiro', 'fechaEntrega', 'estado', 'importe', 'formaPagoEstablecida', 'acciones'];
  public usuario: any; // Almacenar datos del usuario
  public mensajeNotificacion: string | null = null;

  constructor(
    private apiCotizaciones: ApiCotizacionesService,
    private route: ActivatedRoute, // Inyecta ActivatedRoute
    private router: Router,
    private apiAuth: ApiAuthService,
    private signalRService: SignalRService,
    private snackBar: MatSnackBar, // Inyectar MatSnackBar
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.usuario = this.apiAuth.usuarioData;
  
    this.getCotizaciones();
  
    console.log("llegamos a aca 1");
  
    this.signalRService.hubConnection?.on('RecibirNotificacion', (mensaje: string) => {
      console.log("llegamos a aca 2 y el mensaje es ", mensaje);
      
      // Extraer el ID del transportista del mensaje
      const transportistaId = this.extractTransportistaId(mensaje);
      
      // Comparar el ID del transportista con el ID del usuario conectado
      if (transportistaId === this.usuario.id) {
        // Mostrar el mensaje por unos segundos
        this.mensajeNotificacion = mensaje;
        setTimeout(() => {
          this.mensajeNotificacion = null;
        }, 10000);
        // Refrescar el listado de cotizaciones
      this.getCotizaciones();
      }

      // Refrescar el listado de cotizaciones
      this.getCotizaciones();
    });
  }

  // Método para extraer el ID del transportista del mensaje
  private extractTransportistaId(mensaje: string): number | null {
    const match = mensaje.match(/del transportista (\d+)/);
    return match ? parseInt(match[1], 10) : null;
  }

  

  

  getCotizaciones(): void {
    this.apiCotizaciones.getCotizacionesByPersonaId(this.usuario.id).subscribe(response => {
      if (response.exito === 1) {
        this.cotizaciones = response.data;
        console.log("Cotizaciones recibidas:", this.cotizaciones); // Agregar este log
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

  eliminarCotizacion(): void {
    this.abrirAlerta('Esta funcionalidad no pertenece a la UserStorie provista',false);
    
  }

  registrarCotizacion(): void {
    this.abrirAlerta('Esta funcionalidad no pertenece a la UserStorie provista',false);
    
  }

  volverAlMenu(): void {
    this.abrirAlerta('Esta funcionalidad no pertenece a la UserStorie provista',false);
    
  }
}
