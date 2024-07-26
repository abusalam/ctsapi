using System;
using System.Collections.Generic;
using CTS_BE.DAL.Entities.Pension;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL;

public partial class PensionDbContext : DbContext
{
    public PensionDbContext()
    {
    }

    public PensionDbContext(DbContextOptions<PensionDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BankAccount> BankAccounts { get; set; }

    public virtual DbSet<DmlHistory> DmlHistories { get; set; }

    public virtual DbSet<LifeCertificate> LifeCertificates { get; set; }

    public virtual DbSet<Nominee> Nominees { get; set; }

    public virtual DbSet<Pensioner> Pensioners { get; set; }

    public virtual DbSet<PpoBill> PpoBills { get; set; }

    public virtual DbSet<PpoBillBytransfer> PpoBillBytransfers { get; set; }

    public virtual DbSet<PpoBillComponent> PpoBillComponents { get; set; }

    public virtual DbSet<PpoComponent> PpoComponents { get; set; }

    public virtual DbSet<PpoIdSequence> PpoIdSequences { get; set; }

    public virtual DbSet<PpoPayment> PpoPayments { get; set; }

    public virtual DbSet<PpoReceipt> PpoReceipts { get; set; }

    public virtual DbSet<PpoReceiptSequence> PpoReceiptSequences { get; set; }

    public virtual DbSet<PpoStatusFlag> PpoStatusFlags { get; set; }

    public virtual DbSet<UploadedFile> UploadedFiles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConnectionStrings:DBConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BankAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("bank_accounts_pkey");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<DmlHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("dml_history_pkey");

            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<LifeCertificate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("life_certificates_pkey");

            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("true");
            entity.Property(e => e.CertificateFlag).HasDefaultValueSql("false");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Nominee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("nominees_pkey");

            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("true");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.PhotoFile).WithMany(p => p.NomineePhotoFiles).HasConstraintName("nominees_photo_file_id_fkey");

            entity.HasOne(d => d.SignatureFile).WithMany(p => p.NomineeSignatureFiles).HasConstraintName("nominees_signature_file_id_fkey");
        });

        modelBuilder.Entity<Pensioner>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pensioners_pkey");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.PhotoFile).WithMany(p => p.PensionerPhotoFiles).HasConstraintName("pensioners_photo_file_id_fkey");

            entity.HasOne(d => d.SignatureFile).WithMany(p => p.PensionerSignatureFiles).HasConstraintName("pensioners_signature_file_id_fkey");
        });

        modelBuilder.Entity<PpoBill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ppo_bills_pkey");

            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("true");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<PpoBillBytransfer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ppo_bill_bytransfers_pkey");

            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("true");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Bill).WithMany(p => p.PpoBillBytransfers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ppo_bill_bytransfers_bill_id_fkey");
        });

        modelBuilder.Entity<PpoBillComponent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ppo_bill_components_pkey");

            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("true");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Bill).WithMany(p => p.PpoBillComponents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ppo_bill_components_bill_id_fkey");

            entity.HasOne(d => d.Component).WithMany(p => p.PpoBillComponents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ppo_bill_components_component_id_fkey");
        });

        modelBuilder.Entity<PpoComponent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ppo_components_pkey");

            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("true");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<PpoIdSequence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ppo_id_sequences_pkey");

            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("true");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<PpoPayment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ppo_payments_pkey");

            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("true");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<PpoReceipt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ppo_receipts_pkey");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<PpoReceiptSequence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ppo_receipt_sequences_pkey");

            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("true");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<PpoStatusFlag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ppo_status_flags_pkey");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<UploadedFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("uploaded_files_pkey");

            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("true");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
