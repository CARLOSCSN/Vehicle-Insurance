using Insurance.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Infrastructure.Data;

/// <summary>
/// Contexto do banco de dados da aplicação
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Insured> Insureds { get; set; } = null!;
    public DbSet<Vehicle> Vehicles { get; set; } = null!;
    public DbSet<InsurancePolicy> InsurancePolicies { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração da entidade Insured
        modelBuilder.Entity<Insured>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Cpf).IsRequired().HasMaxLength(11);
            entity.HasIndex(e => e.Cpf).IsUnique();
            entity.Property(e => e.Age).IsRequired();
        });

        // Configuração da entidade Vehicle
        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Value).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.Model).IsRequired().HasMaxLength(100);
        });

        // Configuração da entidade InsurancePolicy
        modelBuilder.Entity<InsurancePolicy>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.RiskRate).IsRequired().HasColumnType("decimal(18,6)");
            entity.Property(e => e.RiskPremium).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.PurePremium).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.CommercialPremium).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.InsuranceValue).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.CreatedAt).IsRequired();

            // Relacionamentos
            entity.HasOne(e => e.Vehicle)
                .WithMany(v => v.InsurancePolicies)
                .HasForeignKey(e => e.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Insured)
                .WithMany(i => i.InsurancePolicies)
                .HasForeignKey(e => e.InsuredId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
