namespace sgvf_api.Entities
{
    public class Venta
    {
        public int Id { get; set; }
        public int? ClienteId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string EstadoPago { get; set; } = string.Empty;
        public decimal SaldoPendiente { get; set; }
        public bool Cancelada { get; set; } = false;
        public Cliente? Cliente { get; set; }

        public Usuario Usuario { get; set; } = null!;

        public ICollection<DetalleVenta> DetallesVenta { get; set; } = new List<DetalleVenta>();
        public ICollection<AplicacionPagoCliente> AplicacionesPagos { get; set; }
        = new List<AplicacionPagoCliente>();
    }
}
