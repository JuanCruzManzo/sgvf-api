using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sgvf_api.DTOs.Ventas;
using sgvf_api.Services.Interfaces;
using sgvf_api.Services.Pdf;
using System.Security.Claims;

namespace sgvf_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VentaController : ControllerBase
    {
        private readonly IVentaService _ventaService;
        private readonly ITicketPdfService _ticketPdfService;
        public VentaController(IVentaService ventaService, ITicketPdfService ticketPdfService)
        {
            _ventaService = ventaService;
            _ticketPdfService = ticketPdfService;
        }

        // GET: api/venta
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VentaResponseDto>>> GetVentas()
        {
            var ventas = await _ventaService.ObtenerTodas();
            return Ok(ventas);
        }

        // GET: api/venta/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VentaResponseDto>> GetVenta(int id)
        {
            var venta = await _ventaService.ObtenerPorId(id);

            if (venta == null)
                return NotFound(new { mensaje = "Venta no encontrada." });

            return Ok(venta);
        }

        // POST: api/venta
        [HttpPost]
        public async Task<ActionResult<VentaResponseDto>> RegistrarVenta(VentaCreateDto dto)
        {
            try
            {
                var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                if (usuarioIdClaim == null)
                    return Unauthorized();

                int usuarioId = int.Parse(usuarioIdClaim.Value);

                var venta = await _ventaService.RegistrarVenta(dto, usuarioId);

                return CreatedAtAction(
                    nameof(GetVenta),
                    new { id = venta.Id },
                    venta);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensaje = ex.Message
                });
            }
        }

        // DELETE: api/venta/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelarVenta(int id)
        {
            try
            {
                var resultado = await _ventaService.CancelarVenta(id);

                if (!resultado)
                    return NotFound(new
                    {
                        mensaje = "Venta no encontrada."
                    });

                return Ok(new
                {
                    mensaje = "Venta cancelada correctamente."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensaje = ex.Message
                });
            }
        }
        [HttpGet("{id}/ticket")]
        public async Task<IActionResult> DescargarTicket(int id)
        {
            var pdf = await _ticketPdfService.GenerarTicketAsync(id);

            return File(
     pdf,
     "application/pdf",
     $"Ticket-Venta-{id}-{DateTime.Now:yyyyMMdd}.pdf");
        }
    }
}