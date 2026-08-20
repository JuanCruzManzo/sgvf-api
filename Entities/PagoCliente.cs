namespace sgvf_api.Entities
{
    public class PagoCliente
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }

        public DateTime Fecha { get; set; }

        public decimal Monto { get; set; }

        public string? Observaciones { get; set; }

        public string Tipo { get; set; } = string.Empty;

        public int? VentaId { get; set; }

        public Cliente Cliente { get; set; } = null!;

        public Venta? Venta { get; set; }
        public ICollection<AplicacionPagoCliente> Aplicaciones { get; set; }
        = new List<AplicacionPagoCliente>();
    }
}