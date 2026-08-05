using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sgvf_api.DTOs.PagoProveedor;
using sgvf_api.Services.Interfaces;

namespace sgvf_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PagoProveedorController : ControllerBase
    {
        private readonly IPagoProveedorService _pagoProveedorService;

        public PagoProveedorController(IPagoProveedorService pagoProveedorService)
        {
            _pagoProveedorService = pagoProveedorService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var pagos = await _pagoProveedorService.ObtenerTodos();
            return Ok(pagos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var pago = await _pagoProveedorService.ObtenerPorId(id);

            if (pago == null)
                return NotFound();

            return Ok(pago);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(PagoProveedorCreateDto dto)
        {
            var pago = await _pagoProveedorService.Crear(dto);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = pago.Id },
                pago);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var eliminado = await _pagoProveedorService.Eliminar(id);

            if (!eliminado)
                return NotFound();

            return NoContent();
        }
    }
}