namespace sgvf_api.Entities
{
    public class Proveedor
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        public decimal SaldoPendiente { get; set; }

        public DateTime? FechaUltimoPago { get; set; }

        public decimal? MontoUltimoPago { get; set; }

        public bool Activo { get; set; } = true;

        public ICollection<PagoProveedor> Pagos { get; set; }
            = new List<PagoProveedor>();

        public ICollection<DeudaProveedor> Deudas { get; set; }
            = new List<DeudaProveedor>();
    }
}
