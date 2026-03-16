using Insurance.Domain.Entities;
using Insurance.Domain.Interfaces;
using Insurance.Infrastructure.Data;

namespace Insurance.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório de veículos
/// </summary>
public class VehicleRepository : IVehicleRepository
{
    private readonly ApplicationDbContext _context;

    public VehicleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Vehicle> AddAsync(Vehicle vehicle)
    {
        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync();
        return vehicle;
    }
}
