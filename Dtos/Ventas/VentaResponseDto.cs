namespace sgvf_api.DTOs.Ventas
{
    public class VentaResponseDto
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public int? ClienteId { get; set; }

        public string Cliente { get; set; } = string.Empty;

        public int UsuarioId { get; set; }

        public string Usuario { get; set; } = string.Empty;

        public decimal Total { get; set; }

        public string EstadoPago { get; set; } = string.Empty;

        public decimal SaldoPendiente { get; set; }
        public bool Cancelada { get; set; }

        public List<DetalleVentaResponseDto> Detalles { get; set; } = new();
    }
}