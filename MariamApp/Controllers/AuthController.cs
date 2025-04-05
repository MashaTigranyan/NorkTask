using MariamApp.DTOs.Auth;
using MariamApp.Interfaces;
using MariamApp.JWT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MariamApp.Controllers;

[Route("api/auth")]
[ApiController]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var token = await _authService.Authenticate(model.Username, model.Password);
        if (token == null)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }
        return Ok(new { Token = token });
    }
}