namespace sgvf_api.DTOs.Ventas
{
    public class VentaCreateDto
    {
        public int? ClienteId { get; set; }

        public int UsuarioId { get; set; }

        public string EstadoPago { get; set; } = string.Empty;

        public List<DetalleVentaCreateDto> Detalles { get; set; } = new();
    }
}