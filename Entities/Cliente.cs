namespace sgvf_api.Entities
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public decimal SaldoPendiente { get; set; }
        public DateTime? FechaUltimoCobro { get; set; }
        public decimal? MontoUltimoCobro { get; set; }
        public bool Activo { get; set; }

        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();

        public ICollection<PagoCliente> Pagos { get; set; } = new List<PagoCliente>();
    }
}
