using BackendApi.DTOs;
using BackendApi.Services;
using BackendApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;

namespace BackendApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AddressesController : ControllerBase
{
    private readonly IAddressService _addressService;

    public AddressesController(IAddressService addressService)
    {
        _addressService = addressService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Address>>> GetAddresses()
    {
        if (User.IsInRole("Admin"))
        {
            return Ok(await _addressService.GetAllAddressesAsync());
        }

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim != null && int.TryParse(userIdClaim, out int userId))
        {
            var addresses = await _addressService.GetAllAddressesAsync();
            return Ok(addresses.Where(a => a.UserId == userId));
        }

        return Forbid();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Address>> GetAddress(int id)
    {
        var address = await _addressService.GetAddressByIdAsync(id);
        if (address == null) return NotFound();

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (User.IsInRole("Admin") || (userIdClaim != null && address.UserId.ToString() == userIdClaim))
        {
            return Ok(address);
        }

        return Forbid();
    }

    [HttpPost]
    public async Task<ActionResult<Address>> CreateAddress(CreateAddressDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!User.IsInRole("Admin") && userIdClaim != dto.UserId.ToString())
        {
            return Forbid();
        }

        var address = await _addressService.CreateAddressAsync(dto);
        return CreatedAtAction(nameof(GetAddress), new { id = address.Id }, address);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAddress(int id, UpdateAddressDto dto)
    {
        var address = await _addressService.GetAddressByIdAsync(id);
        if (address == null) return NotFound();

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!User.IsInRole("Admin") && userIdClaim != address.UserId.ToString())
        {
            return Forbid();
        }

        await _addressService.UpdateAddressAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAddress(int id)
    {
        var address = await _addressService.GetAddressByIdAsync(id);
        if (address == null) return NotFound();

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!User.IsInRole("Admin") && userIdClaim != address.UserId.ToString())
        {
            return Forbid();
        }

        await _addressService.DeleteAddressAsync(id);
        return NoContent();
    }
}