using Microsoft.EntityFrameworkCore;
using sgvf_api.Data;
using sgvf_api.DTOs.PagoCliente;
using sgvf_api.Entities;
using sgvf_api.Services.Interfaces;

namespace sgvf_api.Services
{
    public class PagoClienteService : IPagoClienteService
    {
        private readonly ApplicationDbContext _context;

        public PagoClienteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PagoClienteResponseDto>> ObtenerTodos()
        {
            var pagos = await _context.PagosClientes
                .Include(p => p.Cliente)
                .OrderByDescending(p => p.Fecha)
                .ToListAsync();

            return pagos.Select(p => new PagoClienteResponseDto
            {
                Id = p.Id,
                ClienteId = p.ClienteId,
                Cliente = p.Cliente.Nombre,
                Fecha = p.Fecha,
                Monto = p.Monto,
                Observaciones = p.Observaciones
            });
        }

        public async Task<PagoClienteResponseDto?> ObtenerPorId(int id)
        {
            var pago = await _context.PagosClientes
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pago == null)
                return null;

            return new PagoClienteResponseDto
            {
                Id = pago.Id,
                ClienteId = pago.ClienteId,
                Cliente = pago.Cliente.Nombre,
                Fecha = pago.Fecha,
                Monto = pago.Monto,
                Observaciones = pago.Observaciones
            };
        }

        public async Task<IEnumerable<PagoClienteResponseDto>> ObtenerPorCliente(int clienteId)
        {
            var pagos = await _context.PagosClientes
                .Include(p => p.Cliente)
                .Where(p => p.ClienteId == clienteId)
                .OrderByDescending(p => p.Fecha)
                .ToListAsync();

            return pagos.Select(p => new PagoClienteResponseDto
            {
                Id = p.Id,
                ClienteId = p.ClienteId,
                Cliente = p.Cliente.Nombre,
                Fecha = p.Fecha,
                Monto = p.Monto,
                Observaciones = p.Observaciones
            });
        }

        public async Task<PagoClienteResponseDto> Crear(PagoClienteCreateDto dto)
        {
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Id == dto.ClienteId && c.Activo);

            if (cliente == null)
                throw new Exception("El cliente no existe.");

            if (cliente.SaldoPendiente <= 0)
                throw new Exception("El cliente no posee deuda.");

            if (dto.Monto <= 0)
                throw new Exception("El monto debe ser mayor a cero.");

            if (dto.Monto > cliente.SaldoPendiente)
                throw new Exception("El pago supera la deuda pendiente.");

            var pago = new PagoCliente
            {
                ClienteId = dto.ClienteId,
                Fecha = DateTime.Now,
                Monto = dto.Monto,
                Observaciones = dto.Observaciones
            };

            _context.PagosClientes.Add(pago);

            cliente.SaldoPendiente -= dto.Monto;
            cliente.FechaUltimoCobro = DateTime.Now;
            cliente.MontoUltimoCobro = dto.Monto;

            await _context.SaveChangesAsync();

            return (await ObtenerPorId(pago.Id))!;
        }
        public async Task<PagoClienteResponseDto> CrearDeuda(PagoClienteCreateDto dto)
        {
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Id == dto.ClienteId && c.Activo);

            if (cliente == null)
                throw new Exception("El cliente no existe.");

            if (dto.Monto <= 0)
                throw new Exception("El monto debe ser mayor a cero.");

            cliente.SaldoPendiente += dto.Monto;

            var pago = new PagoCliente
            {
                ClienteId = dto.ClienteId,
                Fecha = DateTime.Now,
                Monto = dto.Monto,
                Observaciones = dto.Observaciones
            };

            _context.PagosClientes.Add(pago);

            await _context.SaveChangesAsync();

            return (await ObtenerPorId(pago.Id))!;
        }
        public async Task<bool> Eliminar(int id)
        {
            var pago = await _context.PagosClientes
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pago == null)
                return false;

            pago.Cliente.SaldoPendiente += pago.Monto;

            _context.PagosClientes.Remove(pago);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}