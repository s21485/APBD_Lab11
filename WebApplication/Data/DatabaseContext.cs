using Microsoft.EntityFrameworkCore;
using WebApplication.Models;

namespace WebApplication.Data;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
    
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Prescription_Medicament> Prescription_Medicaments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Prescription_Medicament>()
            .HasKey(pm => new { pm.IdMedicament, pm.IdPrescription });

        modelBuilder.Entity<Prescription_Medicament>()
            .HasOne(pm => pm.Medicament)
            .WithMany(pm => pm.Prescription_Medicaments)
            .HasForeignKey(pm => pm.IdMedicament);
        
        modelBuilder.Entity<Prescription_Medicament>()
            .HasOne(pm => pm.Prescription)
            .WithMany(pm => pm.Prescription_Medicaments)
            .HasForeignKey(pm => pm.IdPrescription);
        
        modelBuilder.Entity<Prescription>()
            .HasOne(pm => pm.Patient)
            .WithMany(pr => pr.Prescriptions)
            .HasForeignKey(pm => pm.IdPatient);
        
        modelBuilder.Entity<Prescription>()
            .HasOne(pm => pm.Doctor)
            .WithMany(pr => pr.Prescriptions)
            .HasForeignKey(pm => pm.IdDoctor);
    }

}