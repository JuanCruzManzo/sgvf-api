using sgvf_api.DTOs.Proveedores;

namespace sgvf_api.Services.Interfaces
{
    public interface IProveedorService
    {
        Task<IEnumerable<ProveedorResponseDto>> ObtenerTodos();

        Task<ProveedorResponseDto?> ObtenerPorId(int id);

        Task<ProveedorResponseDto> Crear(ProveedorCreateDto proveedorDto);

        Task<bool> Actualizar(int id, ProveedorUpdateDto proveedorDto);

        Task<bool> Eliminar(int id);
    }
}