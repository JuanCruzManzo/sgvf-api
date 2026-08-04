using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sgvf_api.DTOs.PagoCliente;
using sgvf_api.Services.Interfaces;

namespace sgvf_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PagoClienteController : ControllerBase
    {
        private readonly IPagoClienteService _pagoClienteService;

        public PagoClienteController(IPagoClienteService pagoClienteService)
        {
            _pagoClienteService = pagoClienteService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var pagos = await _pagoClienteService.ObtenerTodos();
            return Ok(pagos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var pago = await _pagoClienteService.ObtenerPorId(id);

            if (pago == null)
                return NotFound();

            return Ok(pago);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(PagoClienteCreateDto dto)
        {
            var pago = await _pagoClienteService.Crear(dto);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = pago.Id },
                pago);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var eliminado = await _pagoClienteService.Eliminar(id);

            if (!eliminado)
                return NotFound();

            return NoContent();
        }
    }
}