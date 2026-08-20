using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sgvf_api.DTOs.DeudaProveedor;
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
        private readonly IDeudaProveedorService _deudaProveedorService;

        public ProveedorController(
            IProveedorService proveedorService,
            IDeudaProveedorService deudaProveedorService)
        {
            _proveedorService = proveedorService;
            _deudaProveedorService = deudaProveedorService;
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
        [HttpPost]
        public async Task<IActionResult> Crear(
            ProveedorCreateDto proveedorDto)
        {
            var proveedor =
                await _proveedorService.Crear(proveedorDto);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = proveedor.Id },
                proveedor);
        }

        /// <summary>
        /// Registra una nueva deuda para un proveedor.
        /// </summary>
        [HttpPost("{id}/deuda")]
        public async Task<IActionResult> RegistrarDeuda(
            int id,
            DeudaProveedorCreateDto dto)
        {
            try
            {
                var deuda =
                    await _deudaProveedorService.Crear(id, dto);

                return Ok(deuda);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensaje = ex.Message
                });
            }
        }
        /// <summary>
        /// Obtiene las deudas registradas de un proveedor.
        /// </summary>
        [HttpGet("{id}/deudas")]
        public async Task<IActionResult> ObtenerDeudas(int id)
        {
            var deudas =
                await _deudaProveedorService.ObtenerPorProveedor(id);

            return Ok(deudas);
        }

        /// <summary>
        /// Actualiza un proveedor existente.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(
            int id,
            ProveedorUpdateDto proveedorDto)
        {
            var actualizado =
                await _proveedorService.Actualizar(
                    id,
                    proveedorDto);

            if (!actualizado)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Desactiva un proveedor.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var eliminado =
                await _proveedorService.Eliminar(id);

            if (!eliminado)
                return NotFound();

            return NoContent();
        }
    }
}