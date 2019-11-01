using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P01_HospitalDatabase.Data
{
    public class HospitalContext : DbContext
    {
        public HospitalContext()
        {
        }

        public HospitalContext(DbContextOptions options) 
            : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Visitation> Visitations { get; set; }

        public DbSet<Diagnose> Diagnoses { get; set; }

        public DbSet<Medicament> Medicaments { get; set; }

        public DbSet<PatientMedicament> PatientMedicaments { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.DefaultConnection);
            }
        }
     
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigurationOnPatient(modelBuilder);
            ConfigureVisitation(modelBuilder);
            ConfigureDiagnose(modelBuilder);
            ConfigureMedicament(modelBuilder);
            ConfigurePatientMedicament(modelBuilder);
        }

        public void ConfigurationOnPatient(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Patient>()
                .HasKey(p => p.PatientId);

            modelBuilder
                .Entity<Patient>()
                .Property(p => p.FirstName)
                .HasMaxLength(50)
                .IsUnicode();

            modelBuilder
               .Entity<Patient>()
               .Property(p => p.LastName)
               .HasMaxLength(50)
               .IsUnicode();

            modelBuilder
                .Entity<Patient>()
                .Property(p => p.Address)
                .HasMaxLength(250)
                .IsUnicode();

            modelBuilder
                .Entity<Patient>()
                .Property(p => p.Email)
                .HasMaxLength(80)
                .IsUnicode(false);

            modelBuilder
                .Entity<Patient>()
                .Property(p => p.HasInsurance);

            modelBuilder
                .Entity<Patient>()
                .HasMany(p => p.Prescriptions)
                .WithOne(p => p.Patient)
                .HasForeignKey(f => f.PatientId);

            modelBuilder
                .Entity<Patient>()
                .HasMany(d => d.Diagnoses)
                .WithOne(p => p.Patient)
                .HasForeignKey(f => f.PatientId);

            modelBuilder
                .Entity<Patient>()
                .HasMany(v => v.Visitations)
                .WithOne(p => p.Patient)
                .HasForeignKey(f => f.PatientId);

        }

        public void ConfigureVisitation(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Visitation>()
                .HasKey(v => v.VisitationId);

            modelBuilder
                .Entity<Visitation>()
                .Property(v => v.Comments)
                .HasMaxLength(250)
                .IsUnicode();

            modelBuilder
                .Entity<Visitation>()
                .HasOne(p => p.Patient)
                .WithMany(v => v.Visitations)
                .HasForeignKey(f => f.PatientId);

        }

        public void ConfigureDiagnose(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Diagnose>()
                .HasKey(d => d.DiagnoseId);

            modelBuilder
                .Entity<Diagnose>()
                .Property(d => d.Name)
                .HasMaxLength(50)
                .IsUnicode();

            modelBuilder
                .Entity<Diagnose>()
                .Property(d => d.Comments)
                .HasMaxLength(250)
                .IsUnicode();

            modelBuilder
                .Entity<Diagnose>()
                .HasOne(p => p.Patient)
                .WithMany(d => d.Diagnoses)
                .HasForeignKey(k => k.PatientId);
        }

        public void ConfigureMedicament(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Medicament>()
                .HasKey(m => m.MedicamentId);

            modelBuilder
                .Entity<Medicament>()
                .Property(m => m.Name)
                .HasMaxLength(50)
                .IsUnicode();

            modelBuilder
                .Entity<Medicament>()
                .HasMany(p => p.Prescriptions)
                .WithOne(p => p.Medicament)
                .HasForeignKey(k => k.MedicamentId);
        }

        public void ConfigurePatientMedicament(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<PatientMedicament>()
                .HasKey(sc => new
                {
                    sc.PatientId,
                    sc.MedicamentId
                });

            modelBuilder
                .Entity<PatientMedicament>()
                .HasOne(p => p.Patient)
                .WithMany(pm => pm.Prescriptions)
                .HasForeignKey(k => k.PatientId);

            modelBuilder
                .Entity<PatientMedicament>()
                .HasOne(m => m.Medicament)
                .WithMany(pm => pm.Prescriptions)
                .HasForeignKey(m => m.MedicamentId);
        }

        public void ConfigureDoctor(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Doctor>()
                .HasKey(k => k.DoctorId);

            modelBuilder
                .Entity<Doctor>()
                .Property(p => p.Name)
                .HasMaxLength(100)
                .IsUnicode();

            modelBuilder
                .Entity<Doctor>()
                .Property(p => p.Specialty)
                .HasMaxLength(100)
                .IsUnicode();

            modelBuilder
                .Entity<Doctor>()
                .HasMany(v => v.Visitations)
                .WithOne(d => d.Doctor)
                .HasForeignKey(d => d.DoctorId);
        }
    }
}
