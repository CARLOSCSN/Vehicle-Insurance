using Insurance.Domain.Entities;

namespace Insurance.Domain.Interfaces;

/// <summary>
/// Repositório para operações com veículos
/// </summary>
public interface IVehicleRepository
{
    Task<Vehicle> AddAsync(Vehicle vehicle);
}
