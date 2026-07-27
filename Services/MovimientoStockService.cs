using Microsoft.EntityFrameworkCore;
using sgvf_api.Data;
using sgvf_api.DTOs.MovimientoStock;
using sgvf_api.Services.Interfaces;

namespace sgvf_api.Services
{
    public class MovimientoStockService : IMovimientoStockService
    {
        private readonly ApplicationDbContext _context;

        public MovimientoStockService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MovimientoStockResponseDto>> ObtenerTodos()
        {
            return await _context.MovimientosStock
                .Include(m => m.Producto)
                .Include(m => m.Usuario)
                .Include(m => m.Venta)
                .OrderByDescending(m => m.Fecha)
                .Select(m => new MovimientoStockResponseDto
                {
                    Id = m.Id,
                    Fecha = m.Fecha,
                    Producto = m.Producto.Nombre,
                    Usuario = $"{m.Usuario.Nombre} {m.Usuario.Apellido}",
                    TipoMovimiento = m.TipoMovimiento,
                    Motivo = m.Motivo,
                    CantidadCajones = m.CantidadCajones,
                    StockAnterior = m.StockAnterior,
                    StockPosterior = m.StockPosterior,
                    VentaId = m.VentaId
                })
                .ToListAsync();
        }
    }
}