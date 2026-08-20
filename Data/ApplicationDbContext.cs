using Microsoft.EntityFrameworkCore;
using sgvf_api.Entities;

namespace sgvf_api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }

    public DbSet<Producto> Productos { get; set; }

    public DbSet<Cliente> Clientes { get; set; }

    public DbSet<Proveedor> Proveedores { get; set; }

    public DbSet<Venta> Ventas { get; set; }

    public DbSet<DetalleVenta> DetallesVenta { get; set; }

    public DbSet<PagoCliente> PagosClientes { get; set; }

    public DbSet<PagoProveedor> PagosProveedores { get; set; }

    public DbSet<MovimientoStock> MovimientosStock { get; set; }

    public DbSet<DeudaProveedor> DeudasProveedores { get; set; }
    public DbSet<AplicacionPagoCliente> AplicacionesPagosClientes { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MovimientoStock>()
            .HasOne(ms => ms.Producto)
            .WithMany(p => p.MovimientosStock)
            .HasForeignKey(ms => ms.ProductoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MovimientoStock>()
            .HasOne(ms => ms.Usuario)
            .WithMany(u => u.MovimientosStock)
            .HasForeignKey(ms => ms.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MovimientoStock>()
            .HasOne(ms => ms.Venta)
            .WithMany()
            .HasForeignKey(ms => ms.VentaId)
            .OnDelete(DeleteBehavior.Restrict);
            
        // Configuración de precisión para valores monetarios
        modelBuilder.Entity<Cliente>()
            .Property(c => c.SaldoPendiente)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Cliente>()
            .Property(c => c.MontoUltimoCobro)
            .HasPrecision(18, 2);

        modelBuilder.Entity<DetalleVenta>()
            .Property(d => d.PrecioUnitario)
            .HasPrecision(18, 2);

        modelBuilder.Entity<DetalleVenta>()
            .Property(d => d.Subtotal)
            .HasPrecision(18, 2);

        modelBuilder.Entity<DeudaProveedor>()
            .Property(d => d.Monto)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PagoCliente>()
            .Property(p => p.Monto)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PagoProveedor>()
            .Property(p => p.Monto)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Proveedor>()
            .Property(p => p.SaldoPendiente)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Proveedor>()
            .Property(p => p.MontoUltimoPago)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Venta>()
            .Property(v => v.SaldoPendiente)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Venta>()
            .Property(v => v.Total)
            .HasPrecision(18, 2);
        
        modelBuilder.Entity<AplicacionPagoCliente>()
            .Property(a => a.MontoAplicado)
            .HasPrecision(18, 2);
    }
}