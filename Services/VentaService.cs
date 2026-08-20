using Microsoft.EntityFrameworkCore;
using sgvf_api.Data;
using sgvf_api.DTOs.Ventas;
using sgvf_api.Entities;
using sgvf_api.Enums;
using sgvf_api.Services.Interfaces;
namespace sgvf_api.Services
{
    public class VentaService : IVentaService
    {
        private readonly ApplicationDbContext _context;

        public VentaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<VentaResponseDto>> ObtenerTodas()
        {
            var ventas = await _context.Ventas
                .Where(v => !v.Cancelada)
                .Include(v => v.Cliente)
                .Include(v => v.Usuario)
                .Include(v => v.DetallesVenta)
                    .ThenInclude(d => d.Producto)
                .ToListAsync();

            return ventas.Select(v => new VentaResponseDto
            {
                Id = v.Id,
                Fecha = v.Fecha,
                ClienteId = v.ClienteId,
                Cliente = v.Cliente != null ? v.Cliente.Nombre : "Consumidor Final",
                UsuarioId = v.UsuarioId,
                Usuario = $"{v.Usuario.Nombre} {v.Usuario.Apellido}",
                Total = v.Total,
                EstadoPago = v.EstadoPago,
                SaldoPendiente = v.SaldoPendiente,

                Detalles = v.DetallesVenta.Select(d => new DetalleVentaResponseDto
                {
                    Id = d.Id,
                    ProductoId = d.ProductoId,
                    Producto = d.Producto.Nombre,
                    CantidadCajones = d.CantidadCajones,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal
                }).ToList()
            });
        }

        public async Task<VentaResponseDto?> ObtenerPorId(int id)
        {
            var venta = await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Usuario)
                .Include(v => v.DetallesVenta)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(v => v.Id == id && !v.Cancelada);

            if (venta == null)
                return null;

            return new VentaResponseDto
            {
                Id = venta.Id,
                Fecha = venta.Fecha,
                ClienteId = venta.ClienteId,
                Cliente = venta.Cliente != null ? venta.Cliente.Nombre : "Consumidor Final",
                UsuarioId = venta.UsuarioId,
                Usuario = $"{venta.Usuario.Nombre} {venta.Usuario.Apellido}",
                Total = venta.Total,
                EstadoPago = venta.EstadoPago,
                SaldoPendiente = venta.SaldoPendiente,

                Detalles = venta.DetallesVenta.Select(d => new DetalleVentaResponseDto
                {
                    Id = d.Id,
                    ProductoId = d.ProductoId,
                    Producto = d.Producto.Nombre,
                    CantidadCajones = d.CantidadCajones,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal
                }).ToList()
            };
        }
        public async Task<VentaResponseDto> RegistrarVenta(VentaCreateDto dto, int usuarioId)
        {
            // Validar cliente
            if (dto.ClienteId.HasValue)
            {
                var cliente = await _context.Clientes.FindAsync(dto.ClienteId.Value);

                if (cliente == null)
                    throw new Exception("El cliente especificado no existe.");
            }

            if (dto.Detalles == null || !dto.Detalles.Any())
                throw new Exception("La venta debe contener al menos un producto.");

            decimal total = 0;

            var detallesVenta = new List<DetalleVenta>();

            foreach (var item in dto.Detalles)
            {
                var producto = await _context.Productos.FindAsync(item.ProductoId);

                if (producto == null)
                    throw new Exception($"El producto con ID {item.ProductoId} no existe.");

                if (!producto.Activo)
                    throw new Exception($"El producto {producto.Nombre} está inactivo.");

                if (item.CantidadCajones <= 0)
                    throw new Exception($"Cantidad inválida para {producto.Nombre}.");

                if (item.PrecioUnitario <= 0)
                    throw new Exception($"Precio inválido para {producto.Nombre}.");

                if (producto.Stock < item.CantidadCajones)
                    throw new Exception($"Stock insuficiente para {producto.Nombre}.");

                decimal subtotal = item.CantidadCajones * item.PrecioUnitario;

                total += subtotal;

                int stockAnterior = producto.Stock;

                producto.Stock -= item.CantidadCajones;

                int stockPosterior = producto.Stock;

                detallesVenta.Add(new DetalleVenta
                {
                    ProductoId = item.ProductoId,
                    CantidadCajones = item.CantidadCajones,
                    PrecioUnitario = item.PrecioUnitario,
                    Subtotal = subtotal
                });

                _context.MovimientosStock.Add(new MovimientoStock
                {
                    ProductoId = producto.Id,
                    UsuarioId = usuarioId,
                    Fecha = DateTime.Now,
                    TipoMovimiento = TipoMovimientoStock.Salida,
                    Motivo = MotivoMovimientoStock.Venta,
                    CantidadCajones = item.CantidadCajones,
                    StockAnterior = stockAnterior,
                    StockPosterior = stockPosterior
                });
            }

            decimal saldoPendiente = 0;

            if (dto.EstadoPago == "Pendiente")
                saldoPendiente = total;

            var venta = new Venta
            {
                ClienteId = dto.ClienteId,
                UsuarioId = usuarioId,
                Fecha = DateTime.Now,
                EstadoPago = dto.EstadoPago,
                Total = total,
                SaldoPendiente = saldoPendiente,
                DetallesVenta = detallesVenta
            };

            _context.Ventas.Add(venta);

            await _context.SaveChangesAsync();

            var movimientosSinVenta = await _context.MovimientosStock
                .Where(m => m.VentaId == null &&
                            m.UsuarioId == usuarioId &&
                            m.Motivo == MotivoMovimientoStock.Venta)
                .OrderByDescending(m => m.Fecha)
                .Take(detallesVenta.Count)
                .ToListAsync();

            foreach (var movimiento in movimientosSinVenta)
            {
                movimiento.VentaId = venta.Id;
            }
            if (dto.ClienteId.HasValue && dto.EstadoPago == "Pendiente")
            {
                var cliente = await _context.Clientes.FindAsync(dto.ClienteId.Value);

                if (cliente != null)
                {
                    cliente.SaldoPendiente += total;

                    var movimientoDeuda = new PagoCliente
                    {
                        ClienteId = cliente.Id,
                        Fecha = venta.Fecha,
                        Monto = total,
                        Observaciones = $"Venta #{venta.Id}",
                        Tipo = "Deuda",
                        VentaId = venta.Id
                    };

                    _context.PagosClientes.Add(movimientoDeuda);
                }
            }

            await _context.SaveChangesAsync();

            return (await ObtenerPorId(venta.Id))!;
        }

        public async Task<bool> CancelarVenta(int id)
        {
            var venta = await _context.Ventas
                .Include(v => v.DetallesVenta)
                .Include(v => v.Cliente)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venta == null)
                return false;

            if (venta.Cancelada)
                throw new Exception("La venta ya fue cancelada.");

            // =========================
            // DEVOLVER STOCK
            // =========================

            foreach (var detalle in venta.DetallesVenta)
            {
                var producto = await _context.Productos
                    .FindAsync(detalle.ProductoId);

                if (producto != null)
                {
                    producto.Stock += detalle.CantidadCajones;
                }
            }

            // =========================
            // REVERTIR DEUDA DEL CLIENTE
            // =========================

            if (
                venta.EstadoPago == "Pendiente" &&
                venta.Cliente != null &&
                venta.SaldoPendiente > 0
            )
            {
                venta.Cliente.SaldoPendiente -= venta.SaldoPendiente;

                // Protección para evitar saldo negativo
                if (venta.Cliente.SaldoPendiente < 0)
                {
                    venta.Cliente.SaldoPendiente = 0;
                }

                venta.SaldoPendiente = 0;
            }

            // =========================
            // CANCELAR VENTA
            // =========================

            var movimientoVenta = await _context.PagosClientes
                .FirstOrDefaultAsync(p =>
                    p.VentaId == venta.Id &&
                    p.Tipo == "Deuda");

            if (movimientoVenta != null)
            {
                _context.PagosClientes.Remove(movimientoVenta);
            }
            venta.Cancelada = true;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}