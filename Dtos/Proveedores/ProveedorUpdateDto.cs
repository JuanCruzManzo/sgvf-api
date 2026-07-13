using System.ComponentModel.DataAnnotations;

namespace sgvf_api.DTOs.Proveedores
{
    public class ProveedorUpdateDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Phone(ErrorMessage = "El teléfono no tiene un formato válido.")]
        [StringLength(20)]
        public string? Telefono { get; set; }

        [Range(0, double.MaxValue)]
        public decimal SaldoPendiente { get; set; }

        public DateTime? FechaUltimoPago { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? MontoUltimoPago { get; set; }
    }
}