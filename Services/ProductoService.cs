using Microsoft.EntityFrameworkCore;
using sgvf_api.Data;
using sgvf_api.DTOs.Productos;
using sgvf_api.Entities;
using sgvf_api.Services.Interfaces;
using sgvf_api.Enums;

namespace sgvf_api.Services
{
    public class ProductoService : IProductoService
    {
        private readonly ApplicationDbContext _context;

        public ProductoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductoResponseDto>> ObtenerTodos()
        {
            var productos = await _context.Productos
                .Select(p => new ProductoResponseDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Stock = p.Stock,
                    StockMinimo = p.StockMinimo,
                    Activo = p.Activo
                })
                .ToListAsync();

            return productos;
        }

        public async Task<ProductoResponseDto?> ObtenerPorId(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
                return null;

            return new ProductoResponseDto
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Stock = producto.Stock,
                StockMinimo = producto.StockMinimo,
                Activo = producto.Activo
            };
        }

        public async Task<ProductoResponseDto> Crear(ProductoCreateDto productoDto, int usuarioId)
        {
            var producto = new Producto
            {
                Nombre = productoDto.Nombre,
                Descripcion = productoDto.Descripcion,
                Stock = productoDto.Stock,
                StockMinimo = productoDto.StockMinimo,
                Activo = true
            };

            _context.Productos.Add(producto);

            await _context.SaveChangesAsync();

            var movimiento = new MovimientoStock
            {
                ProductoId = producto.Id,
                UsuarioId = usuarioId,
                Fecha = DateTime.Now,
                TipoMovimiento = TipoMovimientoStock.Entrada,
                Motivo = MotivoMovimientoStock.AltaProducto,
                CantidadCajones = producto.Stock,
                StockAnterior = 0,
                StockPosterior = producto.Stock
            };

            _context.MovimientosStock.Add(movimiento);

            await _context.SaveChangesAsync();

            return new ProductoResponseDto
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Stock = producto.Stock,
                StockMinimo = producto.StockMinimo,
                Activo = producto.Activo
            };
        }

        public async Task<bool> Actualizar(int id, ProductoUpdateDto productoDto)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
                return false;

            producto.Nombre = productoDto.Nombre;
            producto.Descripcion = productoDto.Descripcion;
            producto.Stock = productoDto.Stock;
            producto.StockMinimo = productoDto.StockMinimo;
            producto.Activo = productoDto.Activo;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
                return false;

            _context.Productos.Remove(producto);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}