import { Component, OnInit } from '@angular/core';
import { MatSidenavModule } from '@angular/material/sidenav'; 
import { HttpClient } from '@angular/common/http';
import { ApiCotizacionesService } from '../services/api-cotizaciones.service';
import {Cotizacion} from '../models/cotizacion'
import {Response} from '../models/response'
import { CommonModule } from '@angular/common'; // Importar CommonModule para *ngFor
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialogModule } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';

@Component({
  selector: 'app-cotizaciones',
  standalone: true,
  imports: [MatSidenavModule,CommonModule,MatSidenavModule,
    MatTableModule,
    MatInputModule,
    MatButtonModule,
    MatDialogModule,
  MatSnackBarModule,
MatCardModule,
FormsModule],
  templateUrl: './cotizaciones.component.html',
  styleUrl: './cotizaciones.component.css'
})
export class CotizacionesComponent  implements OnInit{

  public lst: any[] | undefined;
  public cotizaciones: Cotizacion[] = [];
  public pedidoId: number | undefined;
  public titulo: string = 'Cotizaciones';
  public displayedColumns: string[] = ['cotizacionId','nombreCotizador', 'fechaRetiro', 'fechaEntrega', 'estado', 'importe', 'calificacionTransportista', 'acciones'];



  

  constructor(
    private apiCotizaciones: ApiCotizacionesService,
    private route: ActivatedRoute, // Inyecta ActivatedRoute
    private router: Router
  ) {}

  ngOnInit(): void {
    
    // Obtén el pedidoId de los parámetros de consulta
    this.route.queryParams.subscribe(params => {
      this.pedidoId = params['pedidoId'];

      console.log(this.pedidoId)
      if (this.pedidoId) {
        
        this.getCotizaciones(this.pedidoId);
      }
    });
  }

  getCotizaciones(pedidoId: number): void {
    this.apiCotizaciones.getCotizacionesByPedidoId(pedidoId).subscribe(response => {
      if (response.exito === 1) {
        this.cotizaciones = response.data;
        console.log("llegue xdd")
        this.titulo = `Cotizaciones del pedido número ${pedidoId}`;

        console.log(response)
      } else {
        console.error(response.mensaje);
      }
    });
  }

  pagar(cotizacionId: number): void {
    // Redirige al componente de pagos y pasa cotizacionId como parámetro
    this.router.navigate(['/pagos'], { queryParams: { cotizacionId } });
  }
  
  

  volverAMisPedidos(): void {
    this.router.navigate(['/pedidos']); // Cambia la ruta según sea necesario
  }
}
