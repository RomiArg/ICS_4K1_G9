import { Component, OnInit } from '@angular/core';
import { MatSidenavModule } from '@angular/material/sidenav'; 
import { NodeJsService } from '../services/node-js.service'; 
import { ApiAuthService } from '../services/apiauth.service'; 
import { ApiCotizacionesService } from '../services/api-cotizaciones.service'; 
import { CommonModule } from '@angular/common'; 
import { MatTableModule } from '@angular/material/table';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { Router, ActivatedRoute } from '@angular/router';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatOption } from '@angular/material/core';
import { MatDialog } from '@angular/material/dialog';
import { AlertDialogComponent } from '../alert-dialog/alert-dialog.component'; // Importa el componente de diálogo
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-pagos',
  standalone: true,
  imports: [MatOption, MatFormField, MatLabel, MatSidenavModule, CommonModule, MatSidenavModule, MatTableModule, MatButtonModule, MatCardModule, FormsModule,MatProgressSpinnerModule],
  templateUrl: './pagos.component.html',
  styleUrls: ['./pagos.component.css']
})
export class PagosComponent implements OnInit {

  public orderData = {
    title: '',
    quantity: 1,
    price: 0
  }; 
  public cotizacionId!: number; 
  public cotizacion: any; 
  public selectedPaymentMethod: string | null = null;
  public isProcessing = false; // Variable para controlar el estado de carga

  constructor(
    private nodeJsService: NodeJsService,
    private apiAuth: ApiAuthService, 
    private apiCotizaciones: ApiCotizacionesService, 
    private dialog: MatDialog, // Inyecta MatDialog
    private router: Router,
    private route: ActivatedRoute 
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.cotizacionId = params['cotizacionId'];
  
      if (this.cotizacionId) {
        this.apiCotizaciones.getCotizacionById(this.cotizacionId).subscribe(response => {
          if (response.exito === 1) {
            this.cotizacion = response.data;
            this.orderData.price = this.cotizacion.importe;
            this.orderData.quantity = 1;
          } else {
            console.error('Error al obtener la cotización:', response.mensaje);
            this.showDialog('Error al obtener la cotización', 'error'); // Mensaje de error
          }
        });
      }
    });
  }

  selectPaymentMethod(method: string): void {
    this.selectedPaymentMethod = method;
    this.isProcessing = true; // Mostrar indicador de carga

    const usuario = this.apiAuth.usuarioData;

    if (!usuario) {
      this.showDialog('Debes estar logueado para realizar el pago', 'error'); // Mensaje de error
      
      return;
    }

    if (method === 'debito') {
      this.crearPreferencia();
    } else if (method === 'efectivo-entrega' || method === 'efectivo-retiro') {
      let formaDePago = '';
      if (method === 'efectivo-entrega') {
        formaDePago = 'Efectivo al entregar';
      } else if (method === 'efectivo-retiro') {
        formaDePago = 'Efectivo al retirar';
      }
      this.nodeJsService.processEfectivoPayment(usuario.id, this.cotizacionId, formaDePago)
        .subscribe(response => {
          this.isProcessing = false; // Ocultar indicador de carga
          this.showDialog('Pago en efectivo procesado correctamente', 'success'); // Mensaje de éxito
          this.isProcessing = false; // Ocultar indicador de carga
        }, error => {
          this.isProcessing = false; // Ocultar indicador de carga
          console.error('Error al procesar el pago en efectivo:', error);
          this.showDialog('Error al procesar el pago en efectivo', 'error'); // Mensaje de error
          this.isProcessing = false; // Ocultar indicador de carga
        });
    }
  }

  volverACotizaciones(): void {
    this.router.navigate(['/pedidos']);
  }
  
  crearPreferencia(): void {
    const usuario = this.apiAuth.usuarioData;

    if (usuario) {
      this.nodeJsService.createPreference(this.orderData, usuario.id, this.cotizacionId)
        .subscribe(response => {
          if (response && response.url) {
            setTimeout(() => {
              window.location.href = response.url; 
            }, 5000); 
          }
          this.isProcessing = false; // Ocultar indicador de carga
        }, error => {
          console.error('Error al crear preferencia:', error);
          this.showDialog('Error al crear la preferencia (cotización confirmada o cancelada)', 'error'); // Mensaje de error
          this.isProcessing = false; // Ocultar indicador de carga
        });
    } else {
      this.showDialog('Debes seleccionar un método de pago y estar logueado para realizar el pago', 'error'); // Mensaje de error
      this.isProcessing = false; // Ocultar indicador de carga
    }
  }

  private showDialog(message: string, type: 'success' | 'error'): void {
    this.dialog.open(AlertDialogComponent, {
      data: { message, type },  // Pasar dinámicamente el tipo de mensaje
      disableClose: true  // Evita que el diálogo se cierre haciendo clic fuera del cuadro
    });
  }
}
