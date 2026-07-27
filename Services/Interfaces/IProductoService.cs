using sgvf_api.DTOs.Productos;

namespace sgvf_api.Services.Interfaces
{
    public interface IProductoService
    {
        Task<IEnumerable<ProductoResponseDto>> ObtenerTodos();

        Task<ProductoResponseDto?> ObtenerPorId(int id);

        Task<ProductoResponseDto> Crear(ProductoCreateDto productoDto, int usuarioId);

        Task<bool> Actualizar(int id, ProductoUpdateDto productoDto);

        Task<bool> Eliminar(int id);
    }
}