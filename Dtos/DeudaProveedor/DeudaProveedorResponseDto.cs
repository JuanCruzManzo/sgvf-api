namespace sgvf_api.DTOs.DeudaProveedor
{
    public class DeudaProveedorResponseDto
    {
        public int Id { get; set; }

        public int ProveedorId { get; set; }

        public string Proveedor { get; set; } = string.Empty;

        public DateTime Fecha { get; set; }

        public decimal Monto { get; set; }

        public string? Observaciones { get; set; }
    }
}