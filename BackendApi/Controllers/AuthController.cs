using BackendApi.DTOs;
using BackendApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginDto loginDto)
    {
        var user = _authService.Authenticate(loginDto);
        if (user == null)
        {
            return Unauthorized(new { message = "Invalid username or password" });
        }

        var token = _authService.GenerateJwtToken(user);
        return Ok(new { token, user });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        return Ok(new { message = "Logged out successfully" });
    }
}