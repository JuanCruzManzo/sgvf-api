using sgvf_api.DTOs.PagoCliente;

namespace sgvf_api.Services.Interfaces
{
    public interface IPagoClienteService
    {
        Task<IEnumerable<PagoClienteResponseDto>> ObtenerTodos();

        Task<PagoClienteResponseDto?> ObtenerPorId(int id);

        Task<IEnumerable<PagoClienteResponseDto>> ObtenerPorCliente(int clienteId);

        Task<PagoClienteResponseDto> Crear(PagoClienteCreateDto dto);

        Task<bool> Eliminar(int id);
    }
}