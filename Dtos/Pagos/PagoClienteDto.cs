namespace sgvf_api.DTOs.PagoCliente
{
    public class PagoClienteCreateDto
    {
        public int ClienteId { get; set; }
        public decimal Monto { get; set; }
        public string? Observaciones { get; set; }
    }
}