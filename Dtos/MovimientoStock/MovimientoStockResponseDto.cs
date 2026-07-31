using sgvf_api.Enums;

namespace sgvf_api.DTOs.MovimientoStock
{
    public class MovimientoStockResponseDto
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public string Producto { get; set; } = string.Empty;

        public string Usuario { get; set; } = string.Empty;

        public TipoMovimientoStock TipoMovimiento { get; set; }

        public MotivoMovimientoStock Motivo { get; set; }

        public int CantidadCajones { get; set; }

        public int StockAnterior { get; set; }

        public int StockPosterior { get; set; }

        public int? VentaId { get; set; }
    }
}