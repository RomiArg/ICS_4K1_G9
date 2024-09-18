import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-alert-dialog',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './alert-dialog.component.html',
  styleUrls: ['./alert-dialog.component.css']
})
export class AlertDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<AlertDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { message: string, type: 'success' | 'error', navigateOnClose: boolean },
    private router: Router
  ) {}

  onClose(): void {
    this.dialogRef.close();
    if (this.data.navigateOnClose) {
      this.router.navigate(['/pedidos']); // Navega a /pedidos solo si navigateOnClose es true
    }
  }
}
