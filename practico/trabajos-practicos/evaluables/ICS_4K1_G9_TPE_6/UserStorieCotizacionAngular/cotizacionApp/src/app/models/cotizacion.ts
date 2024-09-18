export interface Cotizacion {
    cotizacionId: number;
    personaId: number;
    fechaRetiro: string;
    fechaEntrega: string;
    importe: number;
    estado: string;
    formaPagoEstablecida: string;
    calificacionTransportista: number;
    nombreCotizador: string;
  }
  