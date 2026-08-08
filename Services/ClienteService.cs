using Microsoft.EntityFrameworkCore;
using sgvf_api.Data;
using sgvf_api.DTOs.Clientes;
using sgvf_api.Entities;
using sgvf_api.Services.Interfaces;

namespace sgvf_api.Services
{
    public class ClienteService : IClienteService
    {
        private readonly ApplicationDbContext _context;

        public ClienteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ClienteResponseDto>> ObtenerTodos()
        {
            var clientes = await _context.Clientes
                .Select(c => new ClienteResponseDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Telefono = c.Telefono,
                    SaldoPendiente = c.SaldoPendiente,
                    FechaUltimoCobro = c.FechaUltimoCobro,
                    MontoUltimoCobro = c.MontoUltimoCobro,
                    Activo = c.Activo
                })
                .ToListAsync();

            return clientes;
        }

        public async Task<ClienteResponseDto?> ObtenerPorId(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return null;

            return new ClienteResponseDto
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Telefono = cliente.Telefono,
                SaldoPendiente = cliente.SaldoPendiente,
                FechaUltimoCobro = cliente.FechaUltimoCobro,
                MontoUltimoCobro = cliente.MontoUltimoCobro,
                Activo = cliente.Activo
            };
        }

        public async Task<ClienteResponseDto> Crear(ClienteCreateDto clienteDto)
        {
            var cliente = new Cliente
            {
                Nombre = clienteDto.Nombre,
                Telefono = clienteDto.Telefono,
                SaldoPendiente = 0,
                FechaUltimoCobro = null,
                MontoUltimoCobro = null,
                Activo = true
            };

            _context.Clientes.Add(cliente);

            await _context.SaveChangesAsync();

            return new ClienteResponseDto
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Telefono = cliente.Telefono,
                SaldoPendiente = cliente.SaldoPendiente,
                FechaUltimoCobro = cliente.FechaUltimoCobro,
                MontoUltimoCobro = cliente.MontoUltimoCobro,
                Activo = cliente.Activo
            };
        }

        public async Task<bool> Actualizar(int id, ClienteUpdateDto clienteDto)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return false;

            cliente.Nombre = clienteDto.Nombre;
            cliente.Telefono = clienteDto.Telefono;
            cliente.SaldoPendiente = clienteDto.SaldoPendiente;
            cliente.FechaUltimoCobro = clienteDto.FechaUltimoCobro;
            cliente.MontoUltimoCobro = clienteDto.MontoUltimoCobro;
            cliente.Activo = clienteDto.Activo;

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> AumentarDeuda(int id, decimal monto)
        {
            if (monto <= 0)
                throw new Exception("El monto debe ser mayor que cero.");

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Id == id && c.Activo);

            if (cliente == null)
                return false;

            cliente.SaldoPendiente += monto;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return false;

            _context.Clientes.Remove(cliente);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}