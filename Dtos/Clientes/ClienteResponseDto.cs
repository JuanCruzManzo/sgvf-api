namespace sgvf_api.DTOs.Clientes
{
    public class ClienteResponseDto
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        public decimal SaldoPendiente { get; set; }

        public DateTime? FechaUltimoCobro { get; set; }

        public decimal? MontoUltimoCobro { get; set; }

        public bool Activo { get; set; }
    }
}