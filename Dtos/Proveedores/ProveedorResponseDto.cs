namespace sgvf_api.DTOs.Proveedores
{
    public class ProveedorResponseDto
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        public decimal SaldoPendiente { get; set; }

        public DateTime? FechaUltimoPago { get; set; }

        public decimal? MontoUltimoPago { get; set; }
    }
}