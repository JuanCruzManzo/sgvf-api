using sgvf_api.DTOs.DeudaProveedor;

namespace sgvf_api.Services.Interfaces
{
    public interface IDeudaProveedorService
    {
        Task<IEnumerable<DeudaProveedorResponseDto>> ObtenerTodos();

        Task<IEnumerable<DeudaProveedorResponseDto>> ObtenerPorProveedor(int proveedorId);

        Task<DeudaProveedorResponseDto?> ObtenerPorId(int id);

        Task<DeudaProveedorResponseDto> Crear(
            int proveedorId,
            DeudaProveedorCreateDto dto
        );

        Task<bool> Eliminar(int id);
    }
}