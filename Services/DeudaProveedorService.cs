using Microsoft.EntityFrameworkCore;
using sgvf_api.Data;
using sgvf_api.DTOs.DeudaProveedor;
using sgvf_api.Entities;
using sgvf_api.Services.Interfaces;

namespace sgvf_api.Services
{
    public class DeudaProveedorService : IDeudaProveedorService
    {
        private readonly ApplicationDbContext _context;

        public DeudaProveedorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DeudaProveedorResponseDto>> ObtenerTodos()
        {
            var deudas = await _context.DeudasProveedores
                .Include(d => d.Proveedor)
                .OrderByDescending(d => d.Fecha)
                .ToListAsync();

            return deudas.Select(d => new DeudaProveedorResponseDto
            {
                Id = d.Id,
                ProveedorId = d.ProveedorId,
                Proveedor = d.Proveedor.Nombre,
                Fecha = d.Fecha,
                Monto = d.Monto,
                Observaciones = d.Observaciones
            });
        }

        public async Task<IEnumerable<DeudaProveedorResponseDto>>
            ObtenerPorProveedor(int proveedorId)
        {
            var deudas = await _context.DeudasProveedores
                .Include(d => d.Proveedor)
                .Where(d => d.ProveedorId == proveedorId)
                .OrderByDescending(d => d.Fecha)
                .ToListAsync();

            return deudas.Select(d => new DeudaProveedorResponseDto
            {
                Id = d.Id,
                ProveedorId = d.ProveedorId,
                Proveedor = d.Proveedor.Nombre,
                Fecha = d.Fecha,
                Monto = d.Monto,
                Observaciones = d.Observaciones
            });
        }

        public async Task<DeudaProveedorResponseDto?> ObtenerPorId(int id)
        {
            var deuda = await _context.DeudasProveedores
                .Include(d => d.Proveedor)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (deuda == null)
                return null;

            return new DeudaProveedorResponseDto
            {
                Id = deuda.Id,
                ProveedorId = deuda.ProveedorId,
                Proveedor = deuda.Proveedor.Nombre,
                Fecha = deuda.Fecha,
                Monto = deuda.Monto,
                Observaciones = deuda.Observaciones
            };
        }

        public async Task<DeudaProveedorResponseDto> Crear(
            int proveedorId,
            DeudaProveedorCreateDto dto
        )
        {
            var proveedor = await _context.Proveedores
                .FirstOrDefaultAsync(
                    p => p.Id == proveedorId && p.Activo
                );

            if (proveedor == null)
                throw new Exception("El proveedor no existe o está inactivo.");

            if (dto.Monto <= 0)
                throw new Exception("El monto debe ser mayor a cero.");

            var deuda = new DeudaProveedor
            {
                ProveedorId = proveedorId,
                Fecha = DateTime.Now,
                Monto = dto.Monto,
                Observaciones = dto.Observaciones
            };

            _context.DeudasProveedores.Add(deuda);

            proveedor.SaldoPendiente += dto.Monto;

            await _context.SaveChangesAsync();

            return (await ObtenerPorId(deuda.Id))!;
        }

        public async Task<bool> Eliminar(int id)
        {
            var deuda = await _context.DeudasProveedores
                .Include(d => d.Proveedor)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (deuda == null)
                return false;

            var proveedor = deuda.Proveedor;

            proveedor.SaldoPendiente -= deuda.Monto;

            if (proveedor.SaldoPendiente < 0)
                proveedor.SaldoPendiente = 0;

            _context.DeudasProveedores.Remove(deuda);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}