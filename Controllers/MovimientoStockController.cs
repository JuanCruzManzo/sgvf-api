using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sgvf_api.Services.Interfaces;

namespace sgvf_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MovimientoStockController : ControllerBase
    {
        private readonly IMovimientoStockService _movimientoStockService;

        public MovimientoStockController(IMovimientoStockService movimientoStockService)
        {
            _movimientoStockService = movimientoStockService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var movimientos = await _movimientoStockService.ObtenerTodos();

            return Ok(movimientos);
        }
    }
}