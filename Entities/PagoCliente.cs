namespace sgvf_api.Entities
{
    public class PagoCliente
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string? Observaciones { get; set; }

        public Cliente Cliente { get; set; } = null!;
    }
}
