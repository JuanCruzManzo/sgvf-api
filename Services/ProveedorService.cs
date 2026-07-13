using Microsoft.EntityFrameworkCore;
using sgvf_api.Data;
using sgvf_api.DTOs.Proveedores;
using sgvf_api.Entities;
using sgvf_api.Services.Interfaces;

namespace sgvf_api.Services
{
    public class ProveedorService : IProveedorService
    {
        private readonly ApplicationDbContext _context;

        public ProveedorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProveedorResponseDto>> ObtenerTodos()
        {
            var proveedores = await _context.Proveedores
                .Select(p => new ProveedorResponseDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Telefono = p.Telefono,
                    SaldoPendiente = p.SaldoPendiente,
                    FechaUltimoPago = p.FechaUltimoPago,
                    MontoUltimoPago = p.MontoUltimoPago
                })
                .ToListAsync();

            return proveedores;
        }

        public async Task<ProveedorResponseDto?> ObtenerPorId(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);

            if (proveedor == null)
                return null;

            return new ProveedorResponseDto
            {
                Id = proveedor.Id,
                Nombre = proveedor.Nombre,
                Telefono = proveedor.Telefono,
                SaldoPendiente = proveedor.SaldoPendiente,
                FechaUltimoPago = proveedor.FechaUltimoPago,
                MontoUltimoPago = proveedor.MontoUltimoPago
            };
        }

        public async Task<ProveedorResponseDto> Crear(ProveedorCreateDto proveedorDto)
        {
            var proveedor = new Proveedor
            {
                Nombre = proveedorDto.Nombre,
                Telefono = proveedorDto.Telefono ?? string.Empty,
                SaldoPendiente = 0,
                FechaUltimoPago = null,
                MontoUltimoPago = null
            };

            _context.Proveedores.Add(proveedor);

            await _context.SaveChangesAsync();

            return new ProveedorResponseDto
            {
                Id = proveedor.Id,
                Nombre = proveedor.Nombre,
                Telefono = proveedor.Telefono,
                SaldoPendiente = proveedor.SaldoPendiente,
                FechaUltimoPago = proveedor.FechaUltimoPago,
                MontoUltimoPago = proveedor.MontoUltimoPago
            };
        }

        public async Task<bool> Actualizar(int id, ProveedorUpdateDto proveedorDto)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);

            if (proveedor == null)
                return false;

            proveedor.Nombre = proveedorDto.Nombre;
            proveedor.Telefono = proveedorDto.Telefono ?? string.Empty;
            proveedor.SaldoPendiente = proveedorDto.SaldoPendiente;
            proveedor.FechaUltimoPago = proveedorDto.FechaUltimoPago;
            proveedor.MontoUltimoPago = proveedorDto.MontoUltimoPago;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);

            if (proveedor == null)
                return false;

            _context.Proveedores.Remove(proveedor);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}