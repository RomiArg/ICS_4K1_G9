import { Component, OnInit } from '@angular/core';
import { ApiAuthService } from '../services/apiauth.service';
import { MatSidenav, MatSidenavModule } from '@angular/material/sidenav';
import { MatTableModule } from '@angular/material/table';
import { MatInputModule } from '@angular/material/input';
import { MatButton, MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatCardModule } from '@angular/material/card';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    MatSidenavModule,
    MatTableModule,
    MatInputModule,
    MatButtonModule,
    MatDialogModule,
  MatSnackBarModule,
MatCardModule,
FormsModule,
CommonModule
],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {

  public email!: string; 
  public password!: string; 

  public errorMessage: string = ''; // Nueva propiedad para manejar errores
  constructor(public apiauth: ApiAuthService,
     private router: Router){

}

  ngOnInit(): void {
    
  }

  login() {
    this.apiauth.login(this.email, this.password).subscribe({
      next: (response) => {
        if (response.exito === 1) {
          // Verificar si el email es distinto a "dador123@hotmail.com"
          if (response.data.email !== "dador123@hotmail.com") {
            this.router.navigate(['/cotizacionTransportista']);
          } else {
            this.router.navigate(['/pedidos']);
          }
        }
      },
      error: (err) => {
        // Manejar errores del servidor
        console.error('Error en el login:', err);
        this.errorMessage = 'Usuario o contraseña incorrectos'; // Asigna el mensaje de error
      }
    });
  }
}