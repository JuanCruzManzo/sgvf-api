namespace sgvf_api.DTOs.PagoCliente
{
    public class PagoClienteResponseDto
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }

        public string Cliente { get; set; } = string.Empty;

        public DateTime Fecha { get; set; }

        public decimal Monto { get; set; }

        public string? Observaciones { get; set; }
    }
}