using Microsoft.EntityFrameworkCore;
using sgvf_api.Data;
using sgvf_api.DTOs.PagoProveedor;
using sgvf_api.Entities;
using sgvf_api.Services.Interfaces;

namespace sgvf_api.Services
{
    public class PagoProveedorService : IPagoProveedorService
    {
        private readonly ApplicationDbContext _context;

        public PagoProveedorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PagoProveedorResponseDto>> ObtenerTodos()
        {
            var pagos = await _context.PagosProveedores
                .Include(p => p.Proveedor)
                .OrderByDescending(p => p.Fecha)
                .ToListAsync();

            return pagos.Select(p => new PagoProveedorResponseDto
            {
                Id = p.Id,
                ProveedorId = p.ProveedorId,
                Proveedor = p.Proveedor.Nombre,
                Fecha = p.Fecha,
                Monto = p.Monto,
                Observaciones = p.Observaciones
            });
        }

        public async Task<PagoProveedorResponseDto?> ObtenerPorId(int id)
        {
            var pago = await _context.PagosProveedores
                .Include(p => p.Proveedor)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pago == null)
                return null;

            return new PagoProveedorResponseDto
            {
                Id = pago.Id,
                ProveedorId = pago.ProveedorId,
                Proveedor = pago.Proveedor.Nombre,
                Fecha = pago.Fecha,
                Monto = pago.Monto,
                Observaciones = pago.Observaciones
            };
        }
        public async Task<IEnumerable<PagoProveedorResponseDto>>
            ObtenerPorProveedor(int proveedorId)
        {
            var pagos = await _context.PagosProveedores
                .Include(p => p.Proveedor)
                .Where(p => p.ProveedorId == proveedorId)
                .OrderByDescending(p => p.Fecha)
                .ToListAsync();

            return pagos.Select(p => new PagoProveedorResponseDto
            {
                Id = p.Id,
                ProveedorId = p.ProveedorId,
                Proveedor = p.Proveedor.Nombre,
                Fecha = p.Fecha,
                Monto = p.Monto,
                Observaciones = p.Observaciones
            });
        }

        public async Task<PagoProveedorResponseDto> Crear(PagoProveedorCreateDto dto)
        {
            var proveedor = await _context.Proveedores
                .FirstOrDefaultAsync(
                    p => p.Id == dto.ProveedorId && p.Activo
                );

            if (proveedor == null)
                throw new Exception("El proveedor no existe.");

            if (proveedor.SaldoPendiente <= 0)
                throw new Exception("El proveedor no posee deuda.");

            if (dto.Monto <= 0)
                throw new Exception("El monto debe ser mayor a cero.");

            if (dto.Monto > proveedor.SaldoPendiente)
                throw new Exception("El pago supera la deuda pendiente.");

            var pago = new PagoProveedor
            {
                ProveedorId = dto.ProveedorId,
                Fecha = DateTime.Now,
                Monto = dto.Monto,
                Observaciones = dto.Observaciones
            };

            _context.PagosProveedores.Add(pago);

            proveedor.SaldoPendiente -= dto.Monto;
            proveedor.FechaUltimoPago = DateTime.Now;
            proveedor.MontoUltimoPago = dto.Monto;

            await _context.SaveChangesAsync();

            return (await ObtenerPorId(pago.Id))!;
        }

        public async Task<bool> Eliminar(int id)
        {
            var pago = await _context.PagosProveedores
                .Include(p => p.Proveedor)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pago == null)
                return false;

            var proveedor = pago.Proveedor;

            proveedor.SaldoPendiente += pago.Monto;

            _context.PagosProveedores.Remove(pago);

            var ultimoPago = await _context.PagosProveedores
                .Where(p => p.ProveedorId == proveedor.Id && p.Id != pago.Id)
                .OrderByDescending(p => p.Fecha)
                .ThenByDescending(p => p.Id)
                .FirstOrDefaultAsync();

            if (ultimoPago != null)
            {
                proveedor.FechaUltimoPago = ultimoPago.Fecha;
                proveedor.MontoUltimoPago = ultimoPago.Monto;
            }
            else
            {
                proveedor.FechaUltimoPago = null;
                proveedor.MontoUltimoPago = null;
            }

            await _context.SaveChangesAsync();

            return true;
        }
    }
}