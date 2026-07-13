using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sgvf_api.DTOs.Proveedores;
using sgvf_api.Services.Interfaces;

namespace sgvf_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProveedorController : ControllerBase
    {
        private readonly IProveedorService _proveedorService;

        public ProveedorController(IProveedorService proveedorService)
        {
            _proveedorService = proveedorService;
        }

        /// <summary>
        /// Obtiene todos los proveedores.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var proveedores = await _proveedorService.ObtenerTodos();
            return Ok(proveedores);
        }

        /// <summary>
        /// Obtiene un proveedor por su identificador.
        /// </summary>
        /// <param name="id">Id del proveedor.</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var proveedor = await _proveedorService.ObtenerPorId(id);

            if (proveedor == null)
                return NotFound();

            return Ok(proveedor);
        }

        /// <summary>
        /// Crea un nuevo proveedor.
        /// </summary>
        /// <param name="proveedorDto">Datos del proveedor.</param>
        [HttpPost]
        public async Task<IActionResult> Crear(ProveedorCreateDto proveedorDto)
        {
            var proveedor = await _proveedorService.Crear(proveedorDto);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = proveedor.Id },
                proveedor);
        }

        /// <summary>
        /// Actualiza un proveedor existente.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, ProveedorUpdateDto proveedorDto)
        {
            var actualizado = await _proveedorService.Actualizar(id, proveedorDto);

            if (!actualizado)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Elimina un proveedor.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var eliminado = await _proveedorService.Eliminar(id);

            if (!eliminado)
                return NotFound();

            return NoContent();
        }
    }
}