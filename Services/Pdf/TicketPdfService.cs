using Microsoft.EntityFrameworkCore;
using sgvf_api.Services.Pdf;
using sgvf_api.Data;
using QuestPDF.Fluent;
namespace sgvf_api.Services.Pdf
{
    public class TicketPdfService : ITicketPdfService
    {
        private readonly ApplicationDbContext _context;

        public TicketPdfService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<byte[]> GenerarTicketAsync(int ventaId)
        {
            var venta = await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.DetallesVenta)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(v => v.Id == ventaId);

            if (venta == null)
                throw new Exception("La venta no existe.");

            var document = new TicketDocument(venta);

            return document.GeneratePdf();
        }
    }
}