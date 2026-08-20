using sgvf_api.DTOs.PagoProveedor;

namespace sgvf_api.Services.Interfaces
{
    public interface IPagoProveedorService
    {
        Task<IEnumerable<PagoProveedorResponseDto>> ObtenerTodos();

        Task<PagoProveedorResponseDto?> ObtenerPorId(int id);
        Task<IEnumerable<PagoProveedorResponseDto>> ObtenerPorProveedor(
            int proveedorId
        );

        Task<PagoProveedorResponseDto> Crear(PagoProveedorCreateDto dto);

        Task<bool> Eliminar(int id);
    }
}