using BackendApi.DTOs;
using BackendApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApi.Services;

public class MockAddressService : IAddressService
{
    private static readonly List<Address> _addresses = new List<Address>();

    public async Task<IEnumerable<Address>> GetAllAddressesAsync() => _addresses;

    public async Task<Address?> GetAddressByIdAsync(int id) => _addresses.FirstOrDefault(a => a.Id == id);

    public async Task<Address> CreateAddressAsync(CreateAddressDto dto)
    {
        var address = new Address
        {
            Id = _addresses.Count > 0 ? _addresses.Max(a => a.Id) + 1 : 1,
            Street = dto.Street,
            City = dto.City,
            State = dto.State,
            ZipCode = dto.ZipCode,
            Country = dto.Country,
            UserId = dto.UserId
        };
        _addresses.Add(address);
        return address;
    }

    public async Task UpdateAddressAsync(int id, UpdateAddressDto dto)
    {
        var address = _addresses.FirstOrDefault(a => a.Id == id);
        if (address != null)
        {
            if (!string.IsNullOrEmpty(dto.Street)) address.Street = dto.Street;
            if (!string.IsNullOrEmpty(dto.City)) address.City = dto.City;
            if (!string.IsNullOrEmpty(dto.State)) address.State = dto.State;
            if (!string.IsNullOrEmpty(dto.ZipCode)) address.ZipCode = dto.ZipCode;
            if (!string.IsNullOrEmpty(dto.Country)) address.Country = dto.Country;
        }
    }

    public async Task DeleteAddressAsync(int id)
    {
        var address = _addresses.FirstOrDefault(a => a.Id == id);
        if (address != null) _addresses.Remove(address);
    }
}
