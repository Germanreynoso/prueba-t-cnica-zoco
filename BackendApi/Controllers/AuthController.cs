using BackendApi.DTOs;
using BackendApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BackendApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly SessionService _sessionService;

    public AuthController(AuthService authService, SessionService sessionService)
    {
        _authService = authService;
        _sessionService = sessionService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var user = await _authService.Authenticate(loginDto);
        if (user == null)
        {
            return Unauthorized(new { message = "Invalid username or password" });
        }

        var token = await _authService.GenerateJwtToken(user);
        
        // Log the session
        _sessionService.LogLogin(user.Id);

        return Ok(new { token, user });
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
        {
            _sessionService.LogLogout(userId);
        }
        
        return Ok(new { message = "Logged out successfully" });
    }
}