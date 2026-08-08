using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sgvf_api.DTOs.Clientes;
using sgvf_api.Services.Interfaces;

namespace sgvf_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        /// <summary>
        /// Obtiene todos los clientes.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var clientes = await _clienteService.ObtenerTodos();

            return Ok(clientes);
        }

        /// <summary>
        /// Obtiene un cliente por su identificador.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var cliente = await _clienteService.ObtenerPorId(id);

            if (cliente == null)
                return NotFound();

            return Ok(cliente);
        }

        /// <summary>
        /// Crea un nuevo cliente.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Crear(
            ClienteCreateDto clienteDto)
        {
            var cliente =
                await _clienteService.Crear(clienteDto);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = cliente.Id },
                cliente);
        }

        /// <summary>
        /// Actualiza los datos de un cliente.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(
            int id,
            ClienteUpdateDto clienteDto)
        {
            var actualizado =
                await _clienteService.Actualizar(
                    id,
                    clienteDto);

            if (!actualizado)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Aumenta la deuda pendiente de un cliente.
        /// </summary>
        [HttpPost("{id}/deuda")]
        public async Task<IActionResult> AumentarDeuda(
            int id,
            [FromBody] decimal monto)
        {
            try
            {
                var resultado =
                    await _clienteService.AumentarDeuda(
                        id,
                        monto);

                if (!resultado)
                    return NotFound(new
                    {
                        mensaje = "Cliente no encontrado."
                    });

                return Ok(new
                {
                    mensaje = "Deuda registrada correctamente."
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

        /// <summary>
        /// Elimina un cliente.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var eliminado =
                await _clienteService.Eliminar(id);

            if (!eliminado)
                return NotFound();

            return NoContent();
        }
    }
}