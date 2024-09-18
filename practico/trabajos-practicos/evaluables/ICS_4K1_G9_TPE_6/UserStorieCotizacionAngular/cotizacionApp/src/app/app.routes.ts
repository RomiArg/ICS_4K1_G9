import { Routes } from '@angular/router';
import { CotizacionesComponent } from './cotizaciones/cotizaciones.component.js';
import { AuthGuard } from './security/auth.guard.js';
import { LoginComponent } from './login/login.component.js';
import { PedidosComponent } from './pedidos/pedidos.component.js';
import { PagosComponent } from './pagos/pagos.component.js';
import { DetallePedidoComponent } from './detalle-pedido/detalle-pedido.component.js';
import { CotizacionesTransportistaComponent } from './cotizaciones-transportista/cotizaciones-transportista.component.js';


export const routes: Routes = [


    { path: 'cotizaciones', component: CotizacionesComponent, canActivate: [AuthGuard]  },
    { path: 'login', component: LoginComponent  },
    { path: 'pedidos', component: PedidosComponent  },
    { path: 'pagos', component: PagosComponent  },
    { path: 'detallePedido', component: DetallePedidoComponent  },
    { path: 'cotizacionTransportista', component: CotizacionesTransportistaComponent  }


];
