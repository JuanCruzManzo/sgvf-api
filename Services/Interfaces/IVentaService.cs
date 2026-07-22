using sgvf_api.DTOs.Ventas;

public interface IVentaService
{
    Task<IEnumerable<VentaResponseDto>> ObtenerTodas();

    Task<VentaResponseDto?> ObtenerPorId(int id);

    Task<VentaResponseDto> RegistrarVenta(VentaCreateDto dto, int usuarioId);

    Task<bool> CancelarVenta(int id);
}