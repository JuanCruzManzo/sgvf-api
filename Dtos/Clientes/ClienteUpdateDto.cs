using System.ComponentModel.DataAnnotations;

namespace sgvf_api.DTOs.Clientes
{
    public class ClienteUpdateDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [Phone(ErrorMessage = "El teléfono no tiene un formato válido.")]
        [StringLength(20, ErrorMessage = "El teléfono no puede superar los 20 caracteres.")]
        public string Telefono { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "El saldo pendiente no puede ser negativo.")]
        public decimal SaldoPendiente { get; set; }

        public DateTime? FechaUltimoCobro { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El monto del último cobro no puede ser negativo.")]
        public decimal? MontoUltimoCobro { get; set; }

        public bool Activo { get; set; }
    }
}