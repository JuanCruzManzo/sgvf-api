using Microsoft.AspNetCore.Mvc;
using sgvf_api.Dtos.Auth;
using sgvf_api.Services.Interfaces;

namespace sgvf_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var resultado = await _authService.LoginAsync(request);

            if (resultado == null)
                return Unauthorized();

            return Ok(resultado);
        }
    }
}