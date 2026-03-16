using Insurance.Domain.Entities;
using Insurance.Domain.Interfaces;
using Insurance.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório de segurados
/// </summary>
public class InsuredRepository : IInsuredRepository
{
    private readonly ApplicationDbContext _context;

    public InsuredRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Insured?> GetByCpfAsync(string cpf)
    {
        return await _context.Insureds
            .FirstOrDefaultAsync(i => i.Cpf == cpf);
    }

    public async Task<Insured> AddAsync(Insured insured)
    {
        _context.Insureds.Add(insured);
        await _context.SaveChangesAsync();
        return insured;
    }
}
