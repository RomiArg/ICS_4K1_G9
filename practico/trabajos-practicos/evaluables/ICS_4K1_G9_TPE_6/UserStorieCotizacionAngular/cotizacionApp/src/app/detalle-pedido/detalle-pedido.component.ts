import { Component, OnInit } from '@angular/core';
import { MatSidenavModule } from '@angular/material/sidenav';
import { HttpClient } from '@angular/common/http';
import { ApiCotizacionesService } from '../services/api-cotizaciones.service';
import { Cotizacion } from '../models/cotizacion';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';

@Component({
  selector: 'app-detalle-pedido',
  standalone: true,
  imports: [
    MatSidenavModule,
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatSnackBarModule,
    MatCardModule,
    FormsModule
  ],
  templateUrl: './detalle-pedido.component.html',
  styleUrls: ['./detalle-pedido.component.css']
})
export class DetallePedidoComponent implements OnInit {

  public cotizacionesConfirmadas: Cotizacion[] = [];
  public pedidoId: number | undefined;

  constructor(
    private apiCotizaciones: ApiCotizacionesService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Obtén el pedidoId de los parámetros de consulta
    this.route.queryParams.subscribe(params => {
      this.pedidoId = params['pedidoId'];

      if (this.pedidoId) {
        this.getCotizacionesConfirmadas(this.pedidoId);
      }
    });
  }

  getCotizacionesConfirmadas(pedidoId: number): void {
    this.apiCotizaciones.getCotizacionesByPedidoId(pedidoId).subscribe(response => {
      if (response.exito === 1) {
        // Filtra solo las cotizaciones que tienen estado 'Confirmada'
        this.cotizacionesConfirmadas = response.data.filter((cotizacion: Cotizacion) => cotizacion.estado === 'Confirmada');
      }
    });
  }

  volverAMisPedidos(): void {
    this.router.navigate(['/pedidos']);
  }
}
