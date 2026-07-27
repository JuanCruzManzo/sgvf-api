namespace sgvf_api.DTOs.Ventas
{
    public class DetalleVentaCreateDto
    {
        public int ProductoId { get; set; }

        public int CantidadCajones { get; set; }

        public decimal PrecioUnitario { get; set; }
    }
}