namespace sgvf_api.Entities
{
    public class DetalleVenta
    {
        public int Id { get; set; }
        public int VentaId { get; set; }
        public int ProductoId { get; set; }
        public int CantidadCajones { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }

        public Venta Venta { get; set; } = null!;

        public Producto Producto { get; set; } = null!;
    }
}
