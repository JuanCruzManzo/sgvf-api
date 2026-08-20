using System.ComponentModel.DataAnnotations;

namespace sgvf_api.DTOs.Proveedores
{
    public class ProveedorUpdateDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(
            100,
            ErrorMessage = "El nombre no puede superar los 100 caracteres."
        )]
        public string Nombre { get; set; } = string.Empty;

        [Phone(ErrorMessage = "El teléfono no tiene un formato válido.")]
        [StringLength(
            20,
            ErrorMessage = "El teléfono no puede superar los 20 caracteres."
        )]
        public string? Telefono { get; set; }
    }
}