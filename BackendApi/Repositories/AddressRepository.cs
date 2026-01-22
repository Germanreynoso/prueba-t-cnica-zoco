using BackendApi.Models;

namespace BackendApi.Repositories;

public class AddressRepository : Repository<Address>, IAddressRepository
{
    public AddressRepository(ApplicationDbContext context) : base(context) { }
}