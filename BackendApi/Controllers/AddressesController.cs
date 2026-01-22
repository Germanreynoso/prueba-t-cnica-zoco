using BackendApi.DTOs;
using BackendApi.Services;
using BackendApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendApi.Controllers;

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
        var addresses = await _addressService.GetAllAddressesAsync();
        return Ok(addresses);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Address>> GetAddress(int id)
    {
        var address = await _addressService.GetAddressByIdAsync(id);
        if (address == null) return NotFound();
        return Ok(address);
    }

    [HttpPost]
    public async Task<ActionResult<Address>> CreateAddress(CreateAddressDto dto)
    {
        var address = await _addressService.CreateAddressAsync(dto);
        return CreatedAtAction(nameof(GetAddress), new { id = address.Id }, address);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAddress(int id, UpdateAddressDto dto)
    {
        await _addressService.UpdateAddressAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAddress(int id)
    {
        await _addressService.DeleteAddressAsync(id);
        return NoContent();
    }
}