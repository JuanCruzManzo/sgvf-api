namespace sgvf_api.Entities
{
    public class DeudaProveedor
    {
        public int Id { get; set; }

        public int ProveedorId { get; set; }
        public Proveedor Proveedor { get; set; } = null!;

        public DateTime Fecha { get; set; }

        public decimal Monto { get; set; }

        public string? Observaciones { get; set; }
    }
}