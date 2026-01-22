using BackendApi.DTOs;
using BackendApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendApi.Services;

public interface IAddressService
{
    Task<IEnumerable<Address>> GetAllAddressesAsync();
    Task<Address?> GetAddressByIdAsync(int id);
    Task<Address> CreateAddressAsync(CreateAddressDto dto);
    Task UpdateAddressAsync(int id, UpdateAddressDto dto);
    Task DeleteAddressAsync(int id);
}