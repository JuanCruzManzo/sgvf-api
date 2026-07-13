using System.ComponentModel.DataAnnotations;

namespace sgvf_api.DTOs.Proveedores
{
    public class ProveedorCreateDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Phone(ErrorMessage = "El teléfono no tiene un formato válido.")]
        [StringLength(20)]
        public string? Telefono { get; set; }
    }
}