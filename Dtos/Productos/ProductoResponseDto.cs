namespace sgvf_api.DTOs.Productos
{
    public class ProductoResponseDto
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        public int Stock { get; set; }

        public int StockMinimo { get; set; }

        public bool Activo { get; set; }
    }
}