import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CotizacionesTransportistaComponent } from './cotizaciones-transportista.component';

describe('CotizacionesTransportistaComponent', () => {
  let component: CotizacionesTransportistaComponent;
  let fixture: ComponentFixture<CotizacionesTransportistaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CotizacionesTransportistaComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CotizacionesTransportistaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
