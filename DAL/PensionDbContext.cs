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

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Component> Components { get; set; }

    public virtual DbSet<ComponentRate> ComponentRates { get; set; }

    public virtual DbSet<DmlHistory> DmlHistories { get; set; }

    public virtual DbSet<LifeCertificate> LifeCertificates { get; set; }

    public virtual DbSet<Nominee> Nominees { get; set; }

    public virtual DbSet<Pensioner> Pensioners { get; set; }

    public virtual DbSet<PpoBill> PpoBills { get; set; }

    public virtual DbSet<PpoBillBytransfer> PpoBillBytransfers { get; set; }

    public virtual DbSet<PpoBillComponent> PpoBillComponents { get; set; }

    public virtual DbSet<PpoIdSequence> PpoIdSequences { get; set; }

    public virtual DbSet<PpoReceipt> PpoReceipts { get; set; }

    public virtual DbSet<PpoReceiptSequence> PpoReceiptSequences { get; set; }

    public virtual DbSet<PpoStatusFlag> PpoStatusFlags { get; set; }

    public virtual DbSet<PrimaryCategory> PrimaryCategories { get; set; }

    public virtual DbSet<SubCategory> SubCategories { get; set; }

    public virtual DbSet<UploadedFile> UploadedFiles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConnectionStrings:DBConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BankAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("bank_accounts_pkey");

            entity.ToTable("bank_accounts", "cts_pension", tb => tb.HasComment("PensionModuleSchema"));

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("categories_pkey");

            entity.ToTable("categories", "cts_pension", tb => tb.HasComment("PensionModuleSchema"));

            entity.Property(e => e.CategoryName).HasComment("primary_category_name - sub_category_name");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.PrimaryCategory).WithMany(p => p.Categories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("categories_primary_category_id_fkey");

            entity.HasOne(d => d.SubCategory).WithMany(p => p.Categories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("categories_sub_category_id_fkey");
        });

        modelBuilder.Entity<Component>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("components_pkey");

            entity.ToTable("components", "cts_pension", tb => tb.HasComment("PensionModuleSchema"));

            entity.Property(e => e.Id).HasComment("BreakupId");
            entity.Property(e => e.ComponentType).HasComment("P - Payment; D - Deduction;");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.ReliefFlag).HasComment("Relief Allowed (Yes/No)");
        });

        modelBuilder.Entity<ComponentRate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("component_rates_pkey");

            entity.ToTable("component_rates", "cts_pension", tb => tb.HasComment("PensionModuleSchema"));

            entity.Property(e => e.Id).HasComment("RateId");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.RateType).HasComment("P - Percentage; A - Amount;");

            entity.HasOne(d => d.Breakup).WithMany(p => p.ComponentRates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("component_rates_breakup_id_fkey");

            entity.HasOne(d => d.Category).WithMany(p => p.ComponentRates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("component_rates_category_id_fkey");
        });

        modelBuilder.Entity<DmlHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("dml_history_pkey");

            entity.ToTable("dml_history", "cts_pension", tb => tb.HasComment("PensionModuleSchema"));

            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<LifeCertificate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("life_certificates_pkey");

            entity.ToTable("life_certificates", "cts_pension", tb => tb.HasComment("PensionModuleSchema"));

            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("true");
            entity.Property(e => e.CertificateFlag).HasDefaultValueSql("false");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Nominee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("nominees_pkey");

            entity.ToTable("nominees", "cts_pension", tb => tb.HasComment("PensionModuleSchema"));

            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("true");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.PhotoFile).WithMany(p => p.NomineePhotoFiles).HasConstraintName("nominees_photo_file_id_fkey");

            entity.HasOne(d => d.SignatureFile).WithMany(p => p.NomineeSignatureFiles).HasConstraintName("nominees_signature_file_id_fkey");
        });

        modelBuilder.Entity<Pensioner>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pensioners_pkey");

            entity.ToTable("pensioners", "cts_pension", tb => tb.HasComment("PensionModuleSchema"));

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.PhotoFile).WithMany(p => p.PensionerPhotoFiles).HasConstraintName("pensioners_photo_file_id_fkey");

            entity.HasOne(d => d.SignatureFile).WithMany(p => p.PensionerSignatureFiles).HasConstraintName("pensioners_signature_file_id_fkey");
        });

        modelBuilder.Entity<PpoBill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ppo_bills_pkey");

            entity.ToTable("ppo_bills", "cts_pension", tb => tb.HasComment("PensionModuleSchema"));

            entity.Property(e => e.BillType).HasComment("F - First Bill; R - Regular Bill;");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<PpoBillBytransfer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ppo_bill_bytransfers_pkey");

            entity.ToTable("ppo_bill_bytransfers", "cts_pension", tb => tb.HasComment("PensionModuleSchema"));

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Bill).WithMany(p => p.PpoBillBytransfers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ppo_bill_bytransfers_bill_id_fkey");
        });

        modelBuilder.Entity<PpoBillComponent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ppo_bill_components_pkey");

            entity.ToTable("ppo_bill_components", "cts_pension", tb => tb.HasComment("PensionModuleSchema"));

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Bill).WithMany(p => p.PpoBillComponents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ppo_bill_components_bill_id_fkey");

            entity.HasOne(d => d.Rate).WithMany(p => p.PpoBillComponents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ppo_bill_components_rate_id_fkey");
        });

        modelBuilder.Entity<PpoIdSequence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ppo_id_sequences_pkey");

            entity.ToTable("ppo_id_sequences", "cts_pension", tb => tb.HasComment("PensionModuleSchema"));

            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("true");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<PpoReceipt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ppo_receipts_pkey");

            entity.ToTable("ppo_receipts", "cts_pension", tb => tb.HasComment("PensionModuleSchema"));

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<PpoReceiptSequence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ppo_receipt_sequences_pkey");

            entity.ToTable("ppo_receipt_sequences", "cts_pension", tb => tb.HasComment("PensionModuleSchema"));

            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("true");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<PpoStatusFlag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ppo_status_flags_pkey");

            entity.ToTable("ppo_status_flags", "cts_pension", tb => tb.HasComment("PensionModuleSchema"));

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<PrimaryCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("primary_categories_pkey");

            entity.ToTable("primary_categories", "cts_pension", tb => tb.HasComment("PensionModuleSchema"));

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.HoaId).HasComment("Head of Account: 2071 - 01 - 109 - 00 - 001 - V - 04 - 00");
        });

        modelBuilder.Entity<SubCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sub_categories_pkey");

            entity.ToTable("sub_categories", "cts_pension", tb => tb.HasComment("PensionModuleSchema"));

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<UploadedFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("uploaded_files_pkey");

            entity.ToTable("uploaded_files", "cts_pension", tb => tb.HasComment("PensionModuleSchema"));

            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("true");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
