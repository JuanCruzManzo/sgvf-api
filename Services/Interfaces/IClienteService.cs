using sgvf_api.DTOs.Clientes;

namespace sgvf_api.Services.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<ClienteResponseDto>> ObtenerTodos();

        Task<ClienteResponseDto?> ObtenerPorId(int id);

        Task<ClienteResponseDto> Crear(ClienteCreateDto clienteDto);

        Task<bool> Actualizar(int id, ClienteUpdateDto clienteDto);

        Task<bool> Eliminar(int id);

        Task<bool> AumentarDeuda(int id, decimal monto);
    }
}