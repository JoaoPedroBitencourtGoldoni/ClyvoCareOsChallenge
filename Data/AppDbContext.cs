using Microsoft.EntityFrameworkCore;
using ClyvoCareOSChallenge.Models;

namespace ClyvoCareOSChallenge.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Tutor> Tutores { get; set; }
    public DbSet<Pet> Pets { get; set; }
    public DbSet<Vacina> Vacinas { get; set; }
    public DbSet<Consulta> Consultas { get; set; }
    public DbSet<Tratamento> Tratamentos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tutor>(entity =>
        {
            entity.ToTable("TB_TUTORES");
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Id).UseIdentityColumn();
            entity.Property(t => t.NomeCompleto).IsRequired().HasMaxLength(150);
            entity.Property(t => t.Email).IsRequired().HasMaxLength(100);
            entity.HasIndex(t => t.Email).IsUnique();
        });

        modelBuilder.Entity<Pet>(entity =>
        {
            entity.ToTable("TB_PETS");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).UseIdentityColumn();
            entity.Property(p => p.PesoKg).HasColumnType("NUMBER(5,2)");
            entity.HasOne(p => p.Tutor)
                  .WithMany(t => t.Pets)
                  .HasForeignKey(p => p.TutorId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Vacina>(entity =>
        {
            entity.ToTable("TB_VACINAS");
            entity.HasKey(v => v.Id);
            entity.Property(v => v.Id).UseIdentityColumn();
            entity.HasOne(v => v.Pet)
                  .WithMany(p => p.Vacinas)
                  .HasForeignKey(v => v.PetId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Consulta>(entity =>
        {
            entity.ToTable("TB_CONSULTAS");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).UseIdentityColumn();
            entity.Property(c => c.PesoNaConsulta).HasColumnType("NUMBER(5,2)");
            entity.HasOne(c => c.Pet)
                  .WithMany(p => p.Consultas)
                  .HasForeignKey(c => c.PetId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Tratamento>(entity =>
        {
            entity.ToTable("TB_TRATAMENTOS");
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Id).UseIdentityColumn();
            entity.HasOne(t => t.Pet)
                  .WithMany(p => p.Tratamentos)
                  .HasForeignKey(t => t.PetId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
