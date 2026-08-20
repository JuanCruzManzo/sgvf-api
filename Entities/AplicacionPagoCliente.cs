namespace sgvf_api.Entities
{
    public class AplicacionPagoCliente
    {
        public int Id { get; set; }

        public int PagoClienteId { get; set; }
        public PagoCliente PagoCliente { get; set; } = null!;

        public int VentaId { get; set; }
        public Venta Venta { get; set; } = null!;

        public decimal MontoAplicado { get; set; }
    }
}