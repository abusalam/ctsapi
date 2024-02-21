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

    public virtual DbSet<ActiveHoa> ActiveHoas { get; set; }

    public virtual DbSet<AdvanceVoucherDetail> AdvanceVoucherDetails { get; set; }

    public virtual DbSet<AllotmentTransaction> AllotmentTransactions { get; set; }

    public virtual DbSet<BillCurrentStatusMaster> BillCurrentStatusMasters { get; set; }

    public virtual DbSet<BillstatusMaster> BillstatusMasters { get; set; }

    public virtual DbSet<BtDetail> BtDetails { get; set; }

    public virtual DbSet<BtDetailMaster> BtDetailMasters { get; set; }

    public virtual DbSet<ClaimForHoaMapping> ClaimForHoaMappings { get; set; }

    public virtual DbSet<CpinMaster> CpinMasters { get; set; }

    public virtual DbSet<CpinVenderMst> CpinVenderMsts { get; set; }

    public virtual DbSet<Ddo> Ddos { get; set; }

    public virtual DbSet<DdoAllotmentActualrelease> DdoAllotmentActualreleases { get; set; }

    public virtual DbSet<DdoAllotmentTransaction> DdoAllotmentTransactions { get; set; }

    public virtual DbSet<DdoWallet> DdoWallets { get; set; }

    public virtual DbSet<DdoWalletActualrelease> DdoWalletActualreleases { get; set; }

    public virtual DbSet<DemandMajorMapping> DemandMajorMappings { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<DetailHead> DetailHeads { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<FinancialYear> FinancialYears { get; set; }

    public virtual DbSet<GobalObjection> GobalObjections { get; set; }

    public virtual DbSet<LocalObjection> LocalObjections { get; set; }

    public virtual DbSet<MajorHead> MajorHeads { get; set; }

    public virtual DbSet<MinorHead> MinorHeads { get; set; }

    public virtual DbSet<OperatorCode> OperatorCodes { get; set; }

    public virtual DbSet<RbiIfscStock> RbiIfscStocks { get; set; }

    public virtual DbSet<Sao> Saos { get; set; }

    public virtual DbSet<SchemeHead> SchemeHeads { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<SubDetailHead> SubDetailHeads { get; set; }

    public virtual DbSet<SubDetailHead1> SubDetailHeads1 { get; set; }

    public virtual DbSet<SubMajorHead> SubMajorHeads { get; set; }

    public virtual DbSet<Token> Tokens { get; set; }

    public virtual DbSet<TokenEntry> TokenEntries { get; set; }

    public virtual DbSet<TokenFlow> TokenFlows { get; set; }

    public virtual DbSet<TokenHasObjection> TokenHasObjections { get; set; }

    public virtual DbSet<TpBill> TpBills { get; set; }

    public virtual DbSet<TpBtdetail> TpBtdetails { get; set; }

    public virtual DbSet<TpSubdetailInfo> TpSubdetailInfos { get; set; }

    public virtual DbSet<TrMaster> TrMasters { get; set; }

    public virtual DbSet<TrMasterContent> TrMasterContents { get; set; }

    public virtual DbSet<Treasury> Treasuries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       => optionsBuilder.UseNpgsql("Name=ConnectionStrings:DBConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pldbgapi");

        modelBuilder.Entity<ActiveHoa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("active_hoas_pk");

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

        modelBuilder.Entity<AllotmentTransaction>(entity =>
        {
            entity.HasKey(e => e.AllotmentId).HasName("allotment_transactions_pkey");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.DemandNo).IsFixedLength();
            entity.Property(e => e.DeptCode).IsFixedLength();
            entity.Property(e => e.DetailHead).IsFixedLength();
            entity.Property(e => e.FinancialYear).IsFixedLength();
            entity.Property(e => e.MajorHead).IsFixedLength();
            entity.Property(e => e.MemoNumber).IsFixedLength();
            entity.Property(e => e.MinorHead).IsFixedLength();
            entity.Property(e => e.PlanStatus).IsFixedLength();
            entity.Property(e => e.ReceiverSaoDdoCode).IsFixedLength();
            entity.Property(e => e.SchemeHead).IsFixedLength();
            entity.Property(e => e.SenderSaoDdoCode).IsFixedLength();
            entity.Property(e => e.SubdetailHead).IsFixedLength();
            entity.Property(e => e.SubmajorHead).IsFixedLength();
        });

        modelBuilder.Entity<BillCurrentStatusMaster>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("bill_current_status_master_pkey");

            entity.Property(e => e.StatusId).ValueGeneratedNever();
        });

        modelBuilder.Entity<BtDetail>(entity =>
        {
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

        modelBuilder.Entity<BtDetailMaster>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("bt_detail_master_pkey");

            entity.Property(e => e.Code).ValueGeneratedNever();
        });

        modelBuilder.Entity<CpinMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cpin_master_pkey");
        });

        modelBuilder.Entity<CpinVenderMst>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cpin_vender_mst_pkey");

            entity.HasOne(d => d.CpinMst).WithMany(p => p.CpinVenderMsts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_cpin");
        });

        modelBuilder.Entity<Ddo>(entity =>
        {
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

        modelBuilder.Entity<DdoWallet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ddo_wallet_pk");

            entity.Property(e => e.Id).ValueGeneratedNever();
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

        modelBuilder.Entity<DemandMajorMapping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("demand_major_mapping_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DemandCode).IsFixedLength();
            entity.Property(e => e.MajorHeadCode).IsFixedLength();

            entity.HasOne(d => d.DemandCodeNavigation).WithMany(p => p.DemandMajorMappings)
                .HasPrincipalKey(p => p.DemandCode)
                .HasForeignKey(d => d.DemandCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_demand_code");

            entity.HasOne(d => d.MajorHeadCodeNavigation).WithMany(p => p.DemandMajorMappings)
                .HasPrincipalKey(p => p.Code)
                .HasForeignKey(d => d.MajorHeadCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_major_head_code");
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

        modelBuilder.Entity<FinancialYear>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("financial_year_pkey");
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

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).IsFixedLength();
        });

        modelBuilder.Entity<MinorHead>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("minor_head_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).IsFixedLength();

            entity.HasOne(d => d.SubMajor).WithMany(p => p.MinorHeads).HasConstraintName("FK_sub_major_head");
        });

        modelBuilder.Entity<Sao>(entity =>
        {
            entity.Property(e => e.Code).IsFixedLength();
            entity.Property(e => e.Name).IsFixedLength();
            entity.Property(e => e.NextLevelCode).IsFixedLength();
        });

        modelBuilder.Entity<SchemeHead>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("scheme_head_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).IsFixedLength();
            entity.Property(e => e.DemandCode).IsFixedLength();

            entity.HasOne(d => d.DemandCodeNavigation).WithMany(p => p.SchemeHeads)
                .HasPrincipalKey(p => p.DemandCode)
                .HasForeignKey(d => d.DemandCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_demand");

            entity.HasOne(d => d.MinorHead).WithMany(p => p.SchemeHeads)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_minor_head_id");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("true");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.StateCode).IsFixedLength();
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
        });

        modelBuilder.Entity<SubDetailHead1>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sub_detail_head_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).IsFixedLength();

            entity.HasOne(d => d.DetailHead).WithMany(p => p.SubDetailHead1s).HasConstraintName("FK_Detail_head");
        });

        modelBuilder.Entity<SubMajorHead>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sub_major_head_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).IsFixedLength();

            entity.HasOne(d => d.MajorHead).WithMany(p => p.SubMajorHeads).HasConstraintName("Fk_MajorHead");
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

        modelBuilder.Entity<TpBill>(entity =>
        {
            entity.HasKey(e => e.BillId).HasName("TP_Bill_pkey");

            entity.Property(e => e.BillId).HasDefaultValueSql("nextval('billing.\"TP_Bill_bill_id_seq1\"'::regclass)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("0");
            entity.Property(e => e.TreasuryCode).IsFixedLength();

            entity.HasOne(d => d.DemandNavigation).WithMany(p => p.TpBills)
                .HasPrincipalKey(p => p.DemandCode)
                .HasForeignKey(d => d.Demand)
                .HasConstraintName("tp_bill_demand_fkey");

            entity.HasOne(d => d.TrMaster).WithMany(p => p.TpBills).HasConstraintName("tp_bill_tr_master_id_fkey");
        });

        modelBuilder.Entity<TpBtdetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tp_btdetail_pkey");

            entity.Property(e => e.DdoCode).IsFixedLength();

            entity.HasOne(d => d.Bill).WithMany(p => p.TpBtdetails).HasConstraintName("tp_btdetail_bill_id_fkey");

            entity.HasOne(d => d.TrMaster).WithMany(p => p.TpBtdetails).HasConstraintName("tp_btdetail_tr_master_id_fkey");
        });

        modelBuilder.Entity<TpSubdetailInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TP_subdetailInfo_pkey");

            entity.Property(e => e.TreasuryCode).IsFixedLength();

            entity.HasOne(d => d.Bill).WithMany(p => p.TpSubdetailInfos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tp_subdetailInfo_bill_id_fkey");

            entity.HasOne(d => d.DdoWallet).WithOne(p => p.TpSubdetailInfo)
                .HasPrincipalKey<DdoWallet>(p => new { p.ActiveHoaId, p.TreasuryCode })
                .HasForeignKey<TpSubdetailInfo>(d => new { d.ActiveHoaId, d.TreasuryCode })
                .HasConstraintName("TP_subdetailInfo_active_hoa_id_treasury_code_fkey");
        });

        modelBuilder.Entity<TrMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tr_master_from_ebill_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Treasury>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("treasury_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
