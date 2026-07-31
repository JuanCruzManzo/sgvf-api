namespace sgvf_api.DTOs.Ventas
{
    public class DetalleVentaResponseDto
    {
        public int Id { get; set; }

        public int ProductoId { get; set; }

        public string Producto { get; set; } = string.Empty;

        public int CantidadCajones { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal Subtotal { get; set; }
    }
}