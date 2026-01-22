using BackendApi.DTOs;
using BackendApi.Models;
using BackendApi.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendApi.Services;

public class AddressService : IAddressService
{
    private readonly IAddressRepository _addressRepository;
    private readonly IUserRepository _userRepository;

    public AddressService(IAddressRepository addressRepository, IUserRepository userRepository)
    {
        _addressRepository = addressRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<Address>> GetAllAddressesAsync()
    {
        return await _addressRepository.GetAllAsync();
    }

    public async Task<Address?> GetAddressByIdAsync(int id)
    {
        return await _addressRepository.GetByIdAsync(id);
    }

    public async Task<Address> CreateAddressAsync(CreateAddressDto dto)
    {
        // Check if user exists
        var user = await _userRepository.GetByIdAsync(dto.UserId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        var address = new Address
        {
            Street = dto.Street,
            City = dto.City,
            State = dto.State,
            ZipCode = dto.ZipCode,
            Country = dto.Country,
            UserId = dto.UserId
        };

        return await _addressRepository.AddAsync(address);
    }

    public async Task UpdateAddressAsync(int id, UpdateAddressDto dto)
    {
        var address = await _addressRepository.GetByIdAsync(id);
        if (address == null)
        {
            throw new KeyNotFoundException("Address not found.");
        }

        if (!string.IsNullOrEmpty(dto.Street)) address.Street = dto.Street;
        if (!string.IsNullOrEmpty(dto.City)) address.City = dto.City;
        if (!string.IsNullOrEmpty(dto.State)) address.State = dto.State;
        if (!string.IsNullOrEmpty(dto.ZipCode)) address.ZipCode = dto.ZipCode;
        if (!string.IsNullOrEmpty(dto.Country)) address.Country = dto.Country;

        await _addressRepository.UpdateAsync(address);
    }

    public async Task DeleteAddressAsync(int id)
    {
        var address = await _addressRepository.GetByIdAsync(id);
        if (address == null)
        {
            throw new KeyNotFoundException("Address not found.");
        }

        await _addressRepository.DeleteAsync(id);
    }
}