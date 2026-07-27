using Microsoft.AspNetCore.Mvc;
using sgvf_api.DTOs.Productos;
using sgvf_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace sgvf_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductoController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var productos = await _productoService.ObtenerTodos();
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var producto = await _productoService.ObtenerPorId(id);

            if (producto == null)
                return NotFound();

            return Ok(producto);
        }
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Crear(ProductoCreateDto productoDto)
        {
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (usuarioIdClaim == null)
                return Unauthorized();

            int usuarioId = int.Parse(usuarioIdClaim.Value);

            var producto = await _productoService.Crear(productoDto, usuarioId);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = producto.Id },
                producto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, ProductoUpdateDto productoDto)
        {
            var actualizado = await _productoService.Actualizar(id, productoDto);

            if (!actualizado)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var eliminado = await _productoService.Eliminar(id);

            if (!eliminado)
                return NotFound();

            return NoContent();
        }
    }
}