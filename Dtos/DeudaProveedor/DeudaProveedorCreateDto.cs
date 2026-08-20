using System.ComponentModel.DataAnnotations;

namespace sgvf_api.DTOs.DeudaProveedor
{
    public class DeudaProveedorCreateDto
    {
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a cero.")]
        public decimal Monto { get; set; }

        [StringLength(250, ErrorMessage = "Las observaciones no pueden superar los 250 caracteres.")]
        public string? Observaciones { get; set; }
    }
}