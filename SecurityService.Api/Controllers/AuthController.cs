using Microsoft.AspNetCore.Mvc;
using SecurityService.Domain.DTOs;
using SecurityService.Domain.Interfaces;

namespace SecurityService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) => _authService = authService;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] LoginRequest request)
    {
        var result = await _authService.RegisterAsync(request);
        return result ? Ok("Usuario creado") : BadRequest("El usuario ya existe");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);
        return response != null ? Ok(response) : Unauthorized("Credenciales inválidas");
    }
}