using System;
using System.Collections.Generic;
using AssignmentDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace AssignmentDAL.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost; Database=Hospital; Username=postgres; Password=2002; Integrated Security=true; Pooling=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.DoctorId).HasName("Doctor_pkey");

            entity.ToTable("Doctor");

            entity.Property(e => e.DoctorId)
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(null, null, null, 999999L, true, null);
            entity.Property(e => e.Specialist).HasColumnType("character varying");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Patient_pkey");

            entity.ToTable("Patient");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(null, null, null, 999999L, true, null);
            entity.Property(e => e.Disease).HasColumnType("character varying");
            entity.Property(e => e.Email).HasColumnType("character varying");
            entity.Property(e => e.Firstname).HasColumnType("character varying");
            entity.Property(e => e.Gender).HasColumnType("character varying");
            entity.Property(e => e.Lastname).HasColumnType("character varying");
            entity.Property(e => e.Specialist).HasColumnType("character varying");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Patients)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fkey_doctorid");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
