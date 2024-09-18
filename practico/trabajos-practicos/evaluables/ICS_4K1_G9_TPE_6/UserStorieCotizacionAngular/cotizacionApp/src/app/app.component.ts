import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav'; 
@Component({
  selector: 'app-root',
  standalone: true,  // Marca el componente como standalone
  imports: [CommonModule, RouterOutlet, MatSidenavModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';
}
