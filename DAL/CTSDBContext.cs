using System;
using System.Collections.Generic;
using CTS_BE.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL;

public partial class CTSDBContext : DbContext
{
    public CTSDBContext()
    {
    }

    public CTSDBContext(DbContextOptions<CTSDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActiveHoaMst> ActiveHoaMsts { get; set; }

    public virtual DbSet<AdvanceVoucherDetail> AdvanceVoucherDetails { get; set; }

    public virtual DbSet<BillBtdetail> BillBtdetails { get; set; }

    public virtual DbSet<BillDetail> BillDetails { get; set; }

    public virtual DbSet<BillSubdetailInfo> BillSubdetailInfos { get; set; }

    public virtual DbSet<BtDetail> BtDetails { get; set; }

    public virtual DbSet<ChallanEntry> ChallanEntries { get; set; }

    public virtual DbSet<Ddo> Ddos { get; set; }

    public virtual DbSet<DdoAllotmentActualrelease> DdoAllotmentActualreleases { get; set; }

    public virtual DbSet<DdoAllotmentTransaction> DdoAllotmentTransactions { get; set; }

    public virtual DbSet<DdoWallet> DdoWallets { get; set; }

    public virtual DbSet<DdoWalletActualrelease> DdoWalletActualreleases { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<DetailHead> DetailHeads { get; set; }

    public virtual DbSet<FinancialYearMaster> FinancialYearMasters { get; set; }

    public virtual DbSet<GobalObjection> GobalObjections { get; set; }

    public virtual DbSet<LocalObjection> LocalObjections { get; set; }

    public virtual DbSet<MajorHead> MajorHeads { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<SubDetailHead> SubDetailHeads { get; set; }

    public virtual DbSet<Test> Tests { get; set; }

    public virtual DbSet<Token> Tokens { get; set; }

    public virtual DbSet<TokenEntry> TokenEntries { get; set; }

    public virtual DbSet<TokenFlow> TokenFlows { get; set; }

    public virtual DbSet<TokenHasObjection> TokenHasObjections { get; set; }

    public virtual DbSet<TrMaster> TrMasters { get; set; }

    public virtual DbSet<Treasury> Treasuries { get; set; }

    public virtual DbSet<VoucherEntry> VoucherEntries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       => optionsBuilder.UseNpgsql("Name=ConnectionStrings:DBConnection");
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActiveHoaMst>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("active_hoa_mst_new_pk");

            entity.Property(e => e.DemandNo).IsFixedLength();
            entity.Property(e => e.DeptCode).IsFixedLength();
            entity.Property(e => e.DetailHead).IsFixedLength();
            entity.Property(e => e.MajorHead).IsFixedLength();
            entity.Property(e => e.MinorHead).IsFixedLength();
            entity.Property(e => e.PlanStatus).IsFixedLength();
            entity.Property(e => e.SchemeHead).IsFixedLength();
            entity.Property(e => e.SubdetailHead).IsFixedLength();
            entity.Property(e => e.SubmajorHead).IsFixedLength();
        });

        modelBuilder.Entity<AdvanceVoucherDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("advance_voucher_details_pkey");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<BillBtdetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tp_btdetail_pkey");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.DdoCode).IsFixedLength();
            entity.Property(e => e.TreasuryCode).IsFixedLength();

            entity.HasOne(d => d.Bill).WithMany(p => p.BillBtdetails).HasConstraintName("Fk_bill_id");

            entity.HasOne(d => d.BtSerialNavigation).WithMany(p => p.BillBtdetails)
                .HasPrincipalKey(p => p.BtSerial)
                .HasForeignKey(d => d.BtSerial)
                .HasConstraintName("bill_btdetail_bt_serial_fkey");

            entity.HasOne(d => d.DdoCodeNavigation).WithMany(p => p.BillBtdetails)
                .HasPrincipalKey(p => p.Code)
                .HasForeignKey(d => d.DdoCode)
                .HasConstraintName("bill_btdetail_ddo_code_fkey");

            entity.HasOne(d => d.FinancialYearNavigation).WithMany(p => p.BillBtdetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("bill_btdetail_financial_year_fkey");

            entity.HasOne(d => d.TreasuryCodeNavigation).WithMany(p => p.BillBtdetails)
                .HasPrincipalKey(p => p.Code)
                .HasForeignKey(d => d.TreasuryCode)
                .HasConstraintName("bill_btdetail_treasury_code_fkey");
        });

        modelBuilder.Entity<BillDetail>(entity =>
        {
            entity.HasKey(e => e.BillId).HasName("TP_Bill_pkey");

            entity.Property(e => e.BillNo).IsFixedLength();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.DdoCode).IsFixedLength();
            entity.Property(e => e.Demand).IsFixedLength();
            entity.Property(e => e.DetailHead).IsFixedLength();
            entity.Property(e => e.FormRevisionNo).HasDefaultValueSql("1");
            entity.Property(e => e.FormVersion).HasDefaultValueSql("1");
            entity.Property(e => e.MajorHead).IsFixedLength();
            entity.Property(e => e.MinorHead).IsFixedLength();
            entity.Property(e => e.PlanStatus).IsFixedLength();
            entity.Property(e => e.ReferenceNo).IsFixedLength();
            entity.Property(e => e.SchemeHead).IsFixedLength();
            entity.Property(e => e.SubMajorHead).IsFixedLength();
            entity.Property(e => e.TreasuryCode).IsFixedLength();
            entity.Property(e => e.VersionNo).HasDefaultValueSql("1");

            entity.HasOne(d => d.DdoCodeNavigation).WithMany(p => p.BillDetails)
                .HasPrincipalKey(p => p.Code)
                .HasForeignKey(d => d.DdoCode)
                .HasConstraintName("bill_details_ddo_code_fkey");

            entity.HasOne(d => d.DemandNavigation).WithMany(p => p.BillDetails)
                .HasPrincipalKey(p => p.DemandCode)
                .HasForeignKey(d => d.Demand)
                .HasConstraintName("dept_demand_code_fkey");

            entity.HasOne(d => d.DetailHeadNavigation).WithMany(p => p.BillDetails)
                .HasPrincipalKey(p => p.Code)
                .HasForeignKey(d => d.DetailHead)
                .HasConstraintName("bill_details_detail_head_fkey");

            entity.HasOne(d => d.FinancialYearNavigation).WithMany(p => p.BillDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("bill_details_financial_year_fkey");

            entity.HasOne(d => d.MajorHeadNavigation).WithMany(p => p.BillDetails)
                .HasPrincipalKey(p => p.Code)
                .HasForeignKey(d => d.MajorHead)
                .HasConstraintName("bill_details_major_head_fkey");

            entity.HasOne(d => d.TrMaster).WithMany(p => p.BillDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TP_Bill_tr_master_id_fkey");

            entity.HasOne(d => d.TreasuryCodeNavigation).WithMany(p => p.BillDetails)
                .HasPrincipalKey(p => p.Code)
                .HasForeignKey(d => d.TreasuryCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("bill_details_treasury_code_fkey");
        });

        modelBuilder.Entity<BillSubdetailInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TP_subdetailInfo_pkey");

            entity.Property(e => e.DdoCode).IsFixedLength();
            entity.Property(e => e.TreasuryCode).IsFixedLength();

            entity.HasOne(d => d.ActiveHoa).WithMany(p => p.BillSubdetailInfos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TP_subdetailInfo_active_hoa_id_fkey");

            entity.HasOne(d => d.Bill).WithMany(p => p.BillSubdetailInfos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("id");

            entity.HasOne(d => d.DdoCodeNavigation).WithMany(p => p.BillSubdetailInfos)
                .HasPrincipalKey(p => p.Code)
                .HasForeignKey(d => d.DdoCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("bill_subdetail_info_ddo_code_fkey");

            entity.HasOne(d => d.FinancialYearNavigation).WithMany(p => p.BillSubdetailInfos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("bill_subdetail_info_financial_year_fkey");

            entity.HasOne(d => d.TreasuryCodeNavigation).WithMany(p => p.BillSubdetailInfos)
                .HasPrincipalKey(p => p.Code)
                .HasForeignKey(d => d.TreasuryCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("bill_subdetail_info_treasury_code_fkey");

            entity.HasOne(d => d.DdoWallet).WithMany(p => p.BillSubdetailInfos)
                .HasPrincipalKey(p => new { p.ActiveHoaId, p.TreasuryCode })
                .HasForeignKey(d => new { d.ActiveHoaId, d.TreasuryCode })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ddo_wallet_active_hoa_id_treasury_code_fkey");
        });

        modelBuilder.Entity<BtDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("bt_details_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.CreatedBy).IsFixedLength();
            entity.Property(e => e.Demand).IsFixedLength();
            entity.Property(e => e.Detailhead).IsFixedLength();
            entity.Property(e => e.Major).IsFixedLength();
            entity.Property(e => e.Minorhead).IsFixedLength();
            entity.Property(e => e.Planstatus).IsFixedLength();
            entity.Property(e => e.Schemehead).IsFixedLength();
            entity.Property(e => e.Subdetail).IsFixedLength();
            entity.Property(e => e.Submajor).IsFixedLength();
        });

        modelBuilder.Entity<ChallanEntry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("challan_entry_pkey");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.TreasuryCode).IsFixedLength();
        });

        modelBuilder.Entity<Ddo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ddo_pkey");

            entity.Property(e => e.Code).IsFixedLength();
            entity.Property(e => e.Phone).IsFixedLength();
            entity.Property(e => e.TreasuryCode).IsFixedLength();
        });

        modelBuilder.Entity<DdoAllotmentActualrelease>(entity =>
        {
            entity.HasKey(e => e.AllotmentId).HasName("ddo_transactions_pkey");

            entity.Property(e => e.AllotmentId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.DemandNo).IsFixedLength();
            entity.Property(e => e.DeptCode).IsFixedLength();
            entity.Property(e => e.DetailHead).IsFixedLength();
            entity.Property(e => e.FinancialYear).IsFixedLength();
            entity.Property(e => e.MajorHead).IsFixedLength();
            entity.Property(e => e.MinorHead).IsFixedLength();
            entity.Property(e => e.PlanStatus).IsFixedLength();
            entity.Property(e => e.ReceiverSaoDdoCode).IsFixedLength();
            entity.Property(e => e.SchemeHead).IsFixedLength();
            entity.Property(e => e.SenderSaoDdoCode).IsFixedLength();
            entity.Property(e => e.SubdetailHead).IsFixedLength();
            entity.Property(e => e.SubmajorHead).IsFixedLength();
            entity.Property(e => e.TreasuryCode).IsFixedLength();
        });

        modelBuilder.Entity<DdoAllotmentTransaction>(entity =>
        {
            entity.HasKey(e => e.AllotmentId).HasName("ddo_transactions_pkey");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.DemandNo).IsFixedLength();
            entity.Property(e => e.DeptCode).IsFixedLength();
            entity.Property(e => e.DetailHead).IsFixedLength();
            entity.Property(e => e.FinancialYear).IsFixedLength();
            entity.Property(e => e.MajorHead).IsFixedLength();
            entity.Property(e => e.MinorHead).IsFixedLength();
            entity.Property(e => e.PlanStatus).IsFixedLength();
            entity.Property(e => e.ReceiverSaoDdoCode).IsFixedLength();
            entity.Property(e => e.SchemeHead).IsFixedLength();
            entity.Property(e => e.SenderSaoDdoCode).IsFixedLength();
            entity.Property(e => e.SubdetailHead).IsFixedLength();
            entity.Property(e => e.SubmajorHead).IsFixedLength();
            entity.Property(e => e.TreasuryCode).IsFixedLength();
        });

        modelBuilder.Entity<DdoWallet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ddo_wallet_pk");

            entity.Property(e => e.AugmentAmount).HasDefaultValueSql("0");
            entity.Property(e => e.BudgetAllotedAmount).HasDefaultValueSql("0");
            entity.Property(e => e.CeilingAmount).HasDefaultValueSql("0");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.DemandNo).IsFixedLength();
            entity.Property(e => e.DeptCode).IsFixedLength();
            entity.Property(e => e.DetailHead).IsFixedLength();
            entity.Property(e => e.MajorHead).IsFixedLength();
            entity.Property(e => e.MinorHead).IsFixedLength();
            entity.Property(e => e.PlanStatus).IsFixedLength();
            entity.Property(e => e.ReappropriatedAmount).HasDefaultValueSql("0");
            entity.Property(e => e.RevisedAmount).HasDefaultValueSql("0");
            entity.Property(e => e.SaoDdoCode).IsFixedLength();
            entity.Property(e => e.SchemeHead).IsFixedLength();
            entity.Property(e => e.SubdetailHead).IsFixedLength();
            entity.Property(e => e.SubmajorHead).IsFixedLength();
            entity.Property(e => e.SurrenderAmount).HasDefaultValueSql("0");
            entity.Property(e => e.TreasuryCode).IsFixedLength();
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<DdoWalletActualrelease>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ddo_wallet_pk");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.DemandNo).IsFixedLength();
            entity.Property(e => e.DeptCode).IsFixedLength();
            entity.Property(e => e.DetailHead).IsFixedLength();
            entity.Property(e => e.MajorHead).IsFixedLength();
            entity.Property(e => e.MinorHead).IsFixedLength();
            entity.Property(e => e.PlanStatus).IsFixedLength();
            entity.Property(e => e.SaoDdoCode).IsFixedLength();
            entity.Property(e => e.SchemeHead).IsFixedLength();
            entity.Property(e => e.SubdetailHead).IsFixedLength();
            entity.Property(e => e.SubmajorHead).IsFixedLength();
            entity.Property(e => e.TreasuryCode).IsFixedLength();
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("department_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).IsFixedLength();
            entity.Property(e => e.DemandCode).IsFixedLength();
        });

        modelBuilder.Entity<DetailHead>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("detail_head_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).IsFixedLength();
        });

        modelBuilder.Entity<FinancialYearMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("financial_year_master_pkey");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.FinancialYear).IsFixedLength();
            entity.Property(e => e.IsActive).HasDefaultValueSql("true");
        });

        modelBuilder.Entity<GobalObjection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("gobal_objection_pkey");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<LocalObjection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("local_objection_pkey");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.TreasuryCode).IsFixedLength();
        });

        modelBuilder.Entity<MajorHead>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("major_head_pkey");

            entity.Property(e => e.Code).IsFixedLength();
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("status_pkey");

            entity.Property(e => e.Type).HasComment("1 = token flow ");
        });

        modelBuilder.Entity<SubDetailHead>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sub_detail_head_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).IsFixedLength();

            entity.HasOne(d => d.DetailHead).WithMany(p => p.SubDetailHeads).HasConstraintName("FK_Detail_head");
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("test_pkey");
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("token_pkey");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.DdoCode).IsFixedLength();
            entity.Property(e => e.ReferenceNo).IsFixedLength();
            entity.Property(e => e.TreasuryCode).IsFixedLength();

            entity.HasOne(d => d.Bill).WithMany(p => p.Tokens)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tp_bill_bill_id_fky");

            entity.HasOne(d => d.TokenFlow).WithMany(p => p.Tokens).HasConstraintName("token_token_flow_id_fkey");
        });

        modelBuilder.Entity<TokenEntry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("token_entry_pkey");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.FinancialYear).IsFixedLength();
            entity.Property(e => e.TokenNo).HasDefaultValueSql("1");
            entity.Property(e => e.TreasuryCode).IsFixedLength();
        });

        modelBuilder.Entity<TokenFlow>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("token_flow_pkey");

            entity.Property(e => e.ActionTakenAt).HasDefaultValueSql("now()");
            entity.Property(e => e.ReferenceNo).IsFixedLength();
            entity.Property(e => e.TokenOwnerType).HasComment("1 = Front Office Clerk\n2 = Accountant\n3 = Treasury Officer");

            entity.HasOne(d => d.Status).WithMany(p => p.TokenFlows)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("token_flow_status_id_fkey");

            entity.HasOne(d => d.Toke).WithMany(p => p.TokenFlows)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("token_flow_token_id_fkey");
        });

        modelBuilder.Entity<TokenHasObjection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("token_has_objection_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('cts.token_has_objection_id_seq'::regclass)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.GobalObjection).WithMany(p => p.TokenHasObjections).HasConstraintName("token_has_objection_gobal_objection_id_fkey");

            entity.HasOne(d => d.Token).WithMany(p => p.TokenHasObjections)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("token_has_objection_token_id_fkey");
        });

        modelBuilder.Entity<TrMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TR_Master_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Treasury>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("treasury_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).IsFixedLength();
        });

        modelBuilder.Entity<VoucherEntry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("voucher_entry_pkey");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.TreasuryCode).IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
