using Insurance.Domain.Entities;
using Insurance.Domain.Interfaces;
using Insurance.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório de apólices de seguro
/// </summary>
public class InsuranceRepository : IInsuranceRepository
{
    private readonly ApplicationDbContext _context;

    public InsuranceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<InsurancePolicy?> GetByIdAsync(int id)
    {
        return await _context.InsurancePolicies
            .Include(i => i.Vehicle)
            .Include(i => i.Insured)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<IEnumerable<InsurancePolicy>> GetAllAsync()
    {
        return await _context.InsurancePolicies
            .Include(i => i.Vehicle)
            .Include(i => i.Insured)
            .ToListAsync();
    }

    public async Task<InsurancePolicy> AddAsync(InsurancePolicy insurance)
    {
        _context.InsurancePolicies.Add(insurance);
        await _context.SaveChangesAsync();
        
        // Recarregar com as navegações
        await _context.Entry(insurance)
            .Reference(i => i.Vehicle)
            .LoadAsync();
        await _context.Entry(insurance)
            .Reference(i => i.Insured)
            .LoadAsync();
        
        return insurance;
    }

    public async Task<int> CountAsync()
    {
        return await _context.InsurancePolicies.CountAsync();
    }

    public async Task<decimal> GetAverageInsuranceValueAsync()
    {
        if (!await _context.InsurancePolicies.AnyAsync())
            return 0;

        return await _context.InsurancePolicies.AverageAsync(i => i.InsuranceValue);
    }

    public async Task<decimal> GetAverageVehicleValueAsync()
    {
        if (!await _context.InsurancePolicies.AnyAsync())
            return 0;

        return await _context.InsurancePolicies
            .Include(i => i.Vehicle)
            .Where(i => i.Vehicle != null)
            .AverageAsync(i => i.Vehicle.Value);
    }
}
