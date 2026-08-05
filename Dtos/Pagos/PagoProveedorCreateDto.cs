namespace sgvf_api.DTOs.PagoProveedor
{
    public class PagoProveedorCreateDto
    {
        public int ProveedorId { get; set; }

        public decimal Monto { get; set; }

        public string? Observaciones { get; set; }
    }
}