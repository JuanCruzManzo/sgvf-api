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
        /// <param name="id">Id del cliente.</param>
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
        /// <param name="clienteDto">Datos del cliente.</param>
        [HttpPost]
        public async Task<IActionResult> Crear(ClienteCreateDto clienteDto)
        {
            var cliente = await _clienteService.Crear(clienteDto);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = cliente.Id },
                cliente);
        }

        /// <summary>
        /// Actualiza un cliente.
        /// </summary>
        /// <param name="id">Id del cliente.</param>
        /// <param name="clienteDto">Datos actualizados.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, ClienteUpdateDto clienteDto)
        {
            var actualizado = await _clienteService.Actualizar(id, clienteDto);

            if (!actualizado)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Elimina un cliente.
        /// </summary>
        /// <param name="id">Id del cliente.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var eliminado = await _clienteService.Eliminar(id);

            if (!eliminado)
                return NotFound();

            return NoContent();
        }
    }
}