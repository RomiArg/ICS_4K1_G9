using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace UserStorieCotizacion.Models
{
    public partial class ISWContext : DbContext
    {
        public ISWContext()
        {
        }

        public ISWContext(DbContextOptions<ISWContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cotizacion> Cotizaciones { get; set; } = null!;
        public virtual DbSet<FormaPago> FormaPagos { get; set; } = null!;
        public virtual DbSet<Pago> Pagos { get; set; } = null!;
        public virtual DbSet<Persona> Personas { get; set; } = null!;

        public virtual DbSet<Pedido> Pedidos { get; set; } = null!;

        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-KHCIPNL;Database=ISW;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cotizacion>(entity =>
            {
                entity.ToTable("cotizacion");

                entity.Property(e => e.CotizacionId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("cotizacionId");

                entity.Property(e => e.Estado)
                    .HasMaxLength(50)
                    .HasColumnName("estado");

                entity.Property(e => e.FechaEntrega)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaEntrega");

                entity.Property(e => e.FechaRetiro)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaRetiro");

                entity.Property(e => e.FormaPagoEstablecida)
                    .HasMaxLength(100)
                    .HasColumnName("formaPagoEstablecida");

                entity.Property(e => e.Importe)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("importe");

                entity.Property(e => e.PersonaId).HasColumnName("personaId");

                entity.Property(e => e.PedidoId).HasColumnName("pedidoId");
                entity.Property(e => e.CalificacionTransportista).HasColumnName("calificacionTransportista");

                entity.Property(e => e.NombreCotizador).HasColumnName("nombreCotizador");

                // entity.HasOne(d => d.Persona)
                //   .WithMany(p => p.Cotizacions)
                //  .HasForeignKey(d => d.PersonaId)
                // .OnDelete(DeleteBehavior.ClientSetNull)
                //.HasConstraintName("FK_cotizacion_persona");
            });

            modelBuilder.Entity<FormaPago>(entity =>
            {
                entity.ToTable("formaPago");

                entity.Property(e => e.FormaPagoId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("formaPagoId");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .HasColumnName("descripcion");
            });

            modelBuilder.Entity<Pago>(entity =>
            {
                entity.ToTable("pagos");

                entity.Property(e => e.PagoId).ValueGeneratedOnAdd();

                entity.Property(e => e.CotizacionId).HasColumnName("cotizacionId");

                entity.Property(e => e.EstadoPago)
                    .HasMaxLength(100)
                    .HasColumnName("estadoPago");

                entity.Property(e => e.FechaPago)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaPago");

                entity.Property(e => e.FormaPagoId).HasColumnName("formaPagoId");

                entity.Property(e => e.NumeroDePagoDePasarelaDePago).HasColumnName("numeroDePagoDePasarelaDePago");

               // entity.HasOne(d => d.Cotizacion)
                //    .WithMany(p => p.Pagos)
                 //   .HasForeignKey(d => d.CotizacionId)
                  //  .OnDelete(DeleteBehavior.ClientSetNull)
                   // .HasConstraintName("FK_pagos_cotizacion");

               // entity.HasOne(d => d.FormaPago)
                //    .WithMany(p => p.Pagos)
                  //  .HasForeignKey(d => d.FormaPagoId)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    //.HasConstraintName("FK_pagos_formaPago");
            });

            modelBuilder.Entity<Persona>(entity =>
            {
                entity.ToTable("persona");

                entity.Property(e => e.PersonaId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("personaId");

                entity.Property(e => e.Calificacion).HasColumnName("calificacion");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnName("email");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .HasColumnName("nombre");

                entity.Property(e => e.OtrosCampos)
                    .HasMaxLength(100)
                    .HasColumnName("otrosCampos");

                entity.Property(e => e.Rol)
                    .HasMaxLength(50)
                    .HasColumnName("rol")
                    .IsFixedLength();
            });

            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.ToTable("pedido");

                entity.Property(e => e.PedidoId).ValueGeneratedOnAdd();

                entity.Property(e => e.PersonaId).HasColumnName("personaId");

                entity.Property(e => e.EstadoPedido)
                    .HasMaxLength(100)
                    .HasColumnName("estadoPedido");

                entity.Property(e => e.DomicilioRetiro)
                    .HasMaxLength(100)
                    .HasColumnName("domicilioRetiro");

                entity.Property(e => e.FechaRetiro)
                    .HasColumnType("date")
                    .HasColumnName("fechaRetiro");

                entity.Property(e => e.DomicilioEntrega)
                    .HasMaxLength(100)
                    .HasColumnName("domicilioEntrega");

                entity.Property(e => e.FechaEntrega)
                    .HasColumnType("date")
                    .HasColumnName("fechaEntrega");

                // Configuración de las relaciones con otras entidades, si las hay
                // Ejemplo de relación con Persona (si corresponde):
                // entity.HasOne(d => d.Persona)
                //     .WithMany(p => p.Pedidos)
                //     .HasForeignKey(d => d.PersonaId)
                //     .OnDelete(DeleteBehavior.ClientSetNull)
                //     .HasConstraintName("FK_pedidos_persona");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuario"); // Nombre de la tabla en la base de datos

                entity.HasKey(e => e.Id); // Clave primaria

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id"); // Campo de la base de datos

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("email"); // Campo de la base de datos

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("password"); // Campo de la base de datos

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("nombre"); // Campo de la base de datos
            });



            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
