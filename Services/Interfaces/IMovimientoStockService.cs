using sgvf_api.DTOs.MovimientoStock;

namespace sgvf_api.Services.Interfaces
{
    public interface IMovimientoStockService
    {
        Task<IEnumerable<MovimientoStockResponseDto>> ObtenerTodos();
    }
}