using System;
using System.Collections.Generic;
using CTS_BE.DAL.Entities.Pension;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL;

public partial class PensionDbContext : DbContext
{
    public PensionDbContext(DbContextOptions<PensionDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BankAccount> BankAccounts { get; set; }

    public virtual DbSet<Breakup> Breakups { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<ComponentRate> ComponentRates { get; set; }

    public virtual DbSet<DmlHistory> DmlHistories { get; set; }

    public virtual DbSet<LifeCertificate> LifeCertificates { get; set; }

    public virtual DbSet<Nominee> Nominees { get; set; }

    public virtual DbSet<Pensioner> Pensioners { get; set; }

    public virtual DbSet<PpoBill> PpoBills { get; set; }

    public virtual DbSet<PpoBillBreakup> PpoBillBreakups { get; set; }

    public virtual DbSet<PpoBillBytransfer> PpoBillBytransfers { get; set; }

    public virtual DbSet<PpoComponentRate> PpoComponentRates { get; set; }

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

            entity.ToTable("bank_accounts", "cts_pension", tb => tb.HasComment("PensionModuleSchema v1"));

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Pensioner).WithMany(p => p.BankAccounts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("bank_accounts_pensioner_id_fkey");
        });

        modelBuilder.Entity<Breakup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("breakups_pkey");

            entity.ToTable("breakups", "cts_pension", tb => tb.HasComment("PensionModuleSchema v1"));

            entity.Property(e => e.Id).HasComment("BreakupId");
            entity.Property(e => e.ComponentType).HasComment("P - Payment; D - Deduction;");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.ReliefFlag).HasComment("Relief Allowed (Yes/No)");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("categories_pkey");

            entity.ToTable("categories", "cts_pension", tb => tb.HasComment("PensionModuleSchema v1"));

            entity.Property(e => e.CategoryName).HasComment("primary_category_name - sub_category_name");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.PrimaryCategory).WithMany(p => p.Categories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("categories_primary_category_id_fkey");

            entity.HasOne(d => d.SubCategory).WithMany(p => p.Categories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("categories_sub_category_id_fkey");
        });

        modelBuilder.Entity<ComponentRate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("component_rates_pkey");

            entity.ToTable("component_rates", "cts_pension", tb => tb.HasComment("PensionModuleSchema v1"));

            entity.Property(e => e.Id).HasComment("RateId will identify the component rate revised or introduced");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.EffectiveFromDate).HasComment("Effective from date the component rate is revised or introduced");
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

            entity.ToTable("dml_history", "cts_pension", tb => tb.HasComment("PensionModuleSchema v1"));

            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<LifeCertificate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("life_certificates_pkey");

            entity.ToTable("life_certificates", "cts_pension", tb => tb.HasComment("PensionModuleSchema v1"));

            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("true");
            entity.Property(e => e.CertificateFlag).HasDefaultValueSql("false");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Pensioner).WithMany(p => p.LifeCertificates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("life_certificates_pensioner_id_fkey");
        });

        modelBuilder.Entity<Nominee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("nominees_pkey");

            entity.ToTable("nominees", "cts_pension", tb => tb.HasComment("PensionModuleSchema v1"));

            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("true");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Pensioner).WithMany(p => p.Nominees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("nominees_pensioner_id_fkey");

            entity.HasOne(d => d.PhotoFile).WithMany(p => p.NomineePhotoFiles).HasConstraintName("nominees_photo_file_id_fkey");

            entity.HasOne(d => d.SignatureFile).WithMany(p => p.NomineeSignatureFiles).HasConstraintName("nominees_signature_file_id_fkey");
        });

        modelBuilder.Entity<Pensioner>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pensioners_pkey");

            entity.ToTable("pensioners", "cts_pension", tb => tb.HasComment("PensionModuleSchema v1"));

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Gender).HasComment("M - Male; F - Female;");
            entity.Property(e => e.PpoSubType).HasComment("E - Employed; L - Widow Daughter; U - Unmarried Daughter; V - Divorced Daughter; N - Minor Son; R - Minor Daughter; P - Handicapped Son; G - Handicapped Daughter; J - Dependent Father; K - Dependent Mother; H - Husband; W - Wife;");
            entity.Property(e => e.PpoType).HasComment("P - Pension; F - Family Pension; C - CPF;");
            entity.Property(e => e.Religion).HasComment("H - Hindu; M - Muslim; O - Other;");

            entity.HasOne(d => d.Category).WithMany(p => p.Pensioners)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pensioners_category_id_fkey");

            entity.HasOne(d => d.PhotoFile).WithMany(p => p.PensionerPhotoFiles).HasConstraintName("pensioners_photo_file_id_fkey");

            entity.HasOne(d => d.Receipt).WithMany(p => p.Pensioners)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pensioners_receipt_id_fkey");

            entity.HasOne(d => d.SignatureFile).WithMany(p => p.PensionerSignatureFiles).HasConstraintName("pensioners_signature_file_id_fkey");
        });

        modelBuilder.Entity<PpoBill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ppo_bills_pkey");

            entity.ToTable("ppo_bills", "cts_pension", tb => tb.HasComment("PensionModuleSchema v1"));

            entity.Property(e => e.BillNo).HasComment("BillNo is to identify the treasury bill");
            entity.Property(e => e.BillType).HasComment("F - First Bill; R - Regular Bill;");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UtrAt).HasComment("UTRAt timestamp when the UTR is received");
            entity.Property(e => e.UtrNo).HasComment("UTRNo to refer to the actual transaction of the payment");
        });

        modelBuilder.Entity<PpoBillBreakup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ppo_bill_breakups_pkey");

            entity.ToTable("ppo_bill_breakups", "cts_pension", tb => tb.HasComment("PensionModuleSchema v1"));

            entity.Property(e => e.BillId).HasComment("BillId is to identify the bill on which the actual payment made");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.RateId).HasComment("RateId is to identify the component rate applied on the bill");

            entity.HasOne(d => d.Bill).WithMany(p => p.PpoBillBreakups)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ppo_bill_breakups_bill_id_fkey");

            entity.HasOne(d => d.Rate).WithMany(p => p.PpoBillBreakups)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ppo_bill_breakups_rate_id_fkey");
        });

        modelBuilder.Entity<PpoBillBytransfer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ppo_bill_bytransfers_pkey");

            entity.ToTable("ppo_bill_bytransfers", "cts_pension", tb => tb.HasComment("PensionModuleSchema v1"));

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Bill).WithMany(p => p.PpoBillBytransfers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ppo_bill_bytransfers_bill_id_fkey");
        });

        modelBuilder.Entity<PpoComponentRate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ppo_component_rates_pkey");

            entity.ToTable("ppo_component_rates", "cts_pension", tb => tb.HasComment("PensionModuleSchema v1"));

            entity.Property(e => e.Id).HasComment("RevisionId");
            entity.Property(e => e.AmountPerMonth).HasComment("Amount per month is the actual amount paid for the mentioned period");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.FromDate).HasComment("From date is the Date of Commencement of pension of the pensioner");
            entity.Property(e => e.ToDate).HasComment("To date (will be null for regular active bills)");

            entity.HasOne(d => d.Breakup).WithMany(p => p.PpoComponentRates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ppo_component_rates_breakup_id_fkey");
        });

        modelBuilder.Entity<PpoIdSequence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ppo_id_sequences_pkey");

            entity.ToTable("ppo_id_sequences", "cts_pension", tb => tb.HasComment("PensionModuleSchema v1"));

            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("true");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<PpoReceipt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ppo_receipts_pkey");

            entity.ToTable("ppo_receipts", "cts_pension", tb => tb.HasComment("PensionModuleSchema v1"));

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<PpoReceiptSequence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ppo_receipt_sequences_pkey");

            entity.ToTable("ppo_receipt_sequences", "cts_pension", tb => tb.HasComment("PensionModuleSchema v1"));

            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("true");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<PpoStatusFlag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ppo_status_flags_pkey");

            entity.ToTable("ppo_status_flags", "cts_pension", tb => tb.HasComment("PensionModuleSchema v1"));

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Pensioner).WithMany(p => p.PpoStatusFlags)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ppo_status_flags_pensioner_id_fkey");
        });

        modelBuilder.Entity<PrimaryCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("primary_categories_pkey");

            entity.ToTable("primary_categories", "cts_pension", tb => tb.HasComment("PensionModuleSchema v1"));

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.HoaId).HasComment("Head of Account: 2071 - 01 - 109 - 00 - 001 - V - 04 - 00");
        });

        modelBuilder.Entity<SubCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sub_categories_pkey");

            entity.ToTable("sub_categories", "cts_pension", tb => tb.HasComment("PensionModuleSchema v1"));

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<UploadedFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("uploaded_files_pkey");

            entity.ToTable("uploaded_files", "cts_pension", tb => tb.HasComment("PensionModuleSchema v1"));

            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("true");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
