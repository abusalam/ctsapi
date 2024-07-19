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

    public virtual DbSet<Advice> Advices { get; set; }

    public virtual DbSet<Advice1> Advices1 { get; set; }

    public virtual DbSet<Available> Availables { get; set; }

    public virtual DbSet<Bank> Banks { get; set; }

    public virtual DbSet<BankAccount> BankAccounts { get; set; }

    public virtual DbSet<BankMaster> BankMasters { get; set; }

    public virtual DbSet<BeneficiariesMaster> BeneficiariesMasters { get; set; }

    public virtual DbSet<BeneficiaryMaster> BeneficiaryMasters { get; set; }

    public virtual DbSet<BeneficiaryType> BeneficiaryTypes { get; set; }

    public virtual DbSet<BeneficiaryType1> BeneficiaryTypes1 { get; set; }

    public virtual DbSet<BillBtdetail> BillBtdetails { get; set; }

    public virtual DbSet<BillDetail> BillDetails { get; set; }

    public virtual DbSet<BillSubdetailInfo> BillSubdetailInfos { get; set; }

    public virtual DbSet<Branch> Branchs { get; set; }

    public virtual DbSet<BtDetail> BtDetails { get; set; }

    public virtual DbSet<Challan> Challans { get; set; }

    public virtual DbSet<ChallanEntry> ChallanEntries { get; set; }

    public virtual DbSet<ChequeCount> ChequeCounts { get; set; }

    public virtual DbSet<ChequeCountRecord> ChequeCountRecords { get; set; }

    public virtual DbSet<ChequeDamage> ChequeDamages { get; set; }

    public virtual DbSet<ChequeDetail> ChequeDetails { get; set; }

    public virtual DbSet<ChequeDistribute> ChequeDistributes { get; set; }

    public virtual DbSet<ChequeEntry> ChequeEntries { get; set; }

    public virtual DbSet<ChequeIndent> ChequeIndents { get; set; }

    public virtual DbSet<ChequeIndentDetail> ChequeIndentDetails { get; set; }

    public virtual DbSet<ChequeInvoice> ChequeInvoices { get; set; }

    public virtual DbSet<ChequeInvoiceDetail> ChequeInvoiceDetails { get; set; }

    public virtual DbSet<ChequeLocker> ChequeLockers { get; set; }

    public virtual DbSet<ChequeReceived> ChequeReceiveds { get; set; }

    public virtual DbSet<Ddo> Ddos { get; set; }

    public virtual DbSet<DdoAllotmentActual> DdoAllotmentActuals { get; set; }

    public virtual DbSet<DdoAllotmentBookedBill> DdoAllotmentBookedBills { get; set; }

    public virtual DbSet<DdoAllotmentProvisional> DdoAllotmentProvisionals { get; set; }

    public virtual DbSet<DdoAllotmentTransaction> DdoAllotmentTransactions { get; set; }

    public virtual DbSet<DdoWallet> DdoWallets { get; set; }

    public virtual DbSet<DdoWalletActual> DdoWalletActuals { get; set; }

    public virtual DbSet<DdoWalletProvisional> DdoWalletProvisionals { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<DetailHead> DetailHeads { get; set; }

    public virtual DbSet<DiscountDetail> DiscountDetails { get; set; }

    public virtual DbSet<DmlHistory> DmlHistories { get; set; }

    public virtual DbSet<EcsNeftDetail> EcsNeftDetails { get; set; }

    public virtual DbSet<EcsNeftPaymentStatusDetail> EcsNeftPaymentStatusDetails { get; set; }

    public virtual DbSet<EcsNeftPreviousRecord> EcsNeftPreviousRecords { get; set; }

    public virtual DbSet<FailedBeneficiaryRecord> FailedBeneficiaryRecords { get; set; }

    public virtual DbSet<FinancialYearMaster> FinancialYearMasters { get; set; }

    public virtual DbSet<GobalObjection> GobalObjections { get; set; }

    public virtual DbSet<GroupType> GroupTypes { get; set; }

    public virtual DbSet<LfplEc> LfplEcs { get; set; }

    public virtual DbSet<LfplSchemesWallet> LfplSchemesWallets { get; set; }

    public virtual DbSet<LifeCertificate> LifeCertificates { get; set; }

    public virtual DbSet<LocalObjection> LocalObjections { get; set; }

    public virtual DbSet<MajorHead> MajorHeads { get; set; }

    public virtual DbSet<MdStampCategoryHoa> MdStampCategoryHoas { get; set; }

    public virtual DbSet<MdStampSubcategory> MdStampSubcategories { get; set; }

    public virtual DbSet<MmGenStamp> MmGenStamps { get; set; }

    public virtual DbSet<MmStampCategory> MmStampCategories { get; set; }

    public virtual DbSet<MmStampDenomination> MmStampDenominations { get; set; }

    public virtual DbSet<MmStampDiscount> MmStampDiscounts { get; set; }

    public virtual DbSet<MmStampLabel> MmStampLabels { get; set; }

    public virtual DbSet<MmStampVendorType> MmStampVendorTypes { get; set; }

    public virtual DbSet<Nominee> Nominees { get; set; }

    public virtual DbSet<OperatorMaster> OperatorMasters { get; set; }

    public virtual DbSet<PaymentAdvice> PaymentAdvices { get; set; }

    public virtual DbSet<PaymentAdviceHasBeneficiary> PaymentAdviceHasBeneficiarys { get; set; }

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

    public virtual DbSet<RbiIfscStock> RbiIfscStocks { get; set; }

    public virtual DbSet<Reference> References { get; set; }

    public virtual DbSet<ReferenceDetail> ReferenceDetails { get; set; }

    public virtual DbSet<ReferenceStatusMaster> ReferenceStatusMasters { get; set; }

    public virtual DbSet<ReferenceType> ReferenceTypes { get; set; }

    public virtual DbSet<StampCategory> StampCategories { get; set; }

    public virtual DbSet<StampCombination> StampCombinations { get; set; }

    public virtual DbSet<StampIndent> StampIndents { get; set; }

    public virtual DbSet<StampInventory> StampInventories { get; set; }

    public virtual DbSet<StampInventory1> StampInventories1 { get; set; }

    public virtual DbSet<StampInvoice> StampInvoices { get; set; }

    public virtual DbSet<StampLabelMaster> StampLabelMasters { get; set; }

    public virtual DbSet<StampType> StampTypes { get; set; }

    public virtual DbSet<StampVendor> StampVendors { get; set; }

    public virtual DbSet<StampVendorType> StampVendorTypes { get; set; }

    public virtual DbSet<StampWallet> StampWallets { get; set; }

    public virtual DbSet<StampWalletTransaction> StampWalletTransactions { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<SubDetailHead> SubDetailHeads { get; set; }

    public virtual DbSet<TMmGenVendor> TMmGenVendors { get; set; }

    public virtual DbSet<TTmStampBill> TTmStampBills { get; set; }

    public virtual DbSet<TTmStampStockSummary> TTmStampStockSummaries { get; set; }

    public virtual DbSet<Test> Tests { get; set; }

    public virtual DbSet<Token> Tokens { get; set; }

    public virtual DbSet<TokenEntry> TokenEntries { get; set; }

    public virtual DbSet<TokenFlow> TokenFlows { get; set; }

    public virtual DbSet<TokenHasObjection> TokenHasObjections { get; set; }

    public virtual DbSet<Tr7Form> Tr7Forms { get; set; }

    public virtual DbSet<TrMaster> TrMasters { get; set; }

    public virtual DbSet<TransactionLot> TransactionLots { get; set; }

    public virtual DbSet<TransactionLotHasBeneficiary> TransactionLotHasBeneficiaries { get; set; }

    public virtual DbSet<TransactionLotHistory> TransactionLotHistories { get; set; }

    public virtual DbSet<Treasury> Treasuries { get; set; }

    public virtual DbSet<TreasuryHasBranch> TreasuryHasBranches { get; set; }

    public virtual DbSet<UploadedFile> UploadedFiles { get; set; }

    public virtual DbSet<UserList> UserLists { get; set; }

    public virtual DbSet<VAvailable> VAvailables { get; set; }

    public virtual DbSet<VBillDetail> VBillDetails { get; set; }

    public virtual DbSet<VPaymentAdvice> VPaymentAdvices { get; set; }

    public virtual DbSet<VTokenDeatil> VTokenDeatils { get; set; }

    public virtual DbSet<VendorRequisitionApprove> VendorRequisitionApproves { get; set; }

    public virtual DbSet<VendorRequisitionChallanGenerate> VendorRequisitionChallanGenerates { get; set; }

    public virtual DbSet<VendorRequisitionStaging> VendorRequisitionStagings { get; set; }

    public virtual DbSet<VendorStampRequisition> VendorStampRequisitions { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    public virtual DbSet<VoucherEntry> VoucherEntries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=10.176.100.34;Database=new_cts;Username=postgres;Password=pgsql");

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

        modelBuilder.Entity<Advice>(entity =>
        {
            entity.HasKey(e => e.AdviceId).HasName("advice_pkey");

            entity.Property(e => e.TreasuryCode).IsFixedLength();

            entity.HasOne(d => d.Op).WithMany(p => p.Advices)
                .HasPrincipalKey(p => p.OpId)
                .HasForeignKey(d => d.OpId)
                .HasConstraintName("op_id_fkey");
        });

        modelBuilder.Entity<Advice1>(entity =>
        {
            entity.HasKey(e => e.TreasuryAdviceId).HasName("advice_pkey");

            entity.Property(e => e.TreasuryCode).IsFixedLength();

            entity.HasOne(d => d.Op).WithMany(p => p.Advice1s)
                .HasPrincipalKey(p => p.OpId)
                .HasForeignKey(d => d.OpId)
                .HasConstraintName("op_id_fkey");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.Advice1s).HasConstraintName("status_fkey");
        });

        modelBuilder.Entity<Bank>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.BankCode }).HasName("banks_pkey");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<BankAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("bank_accounts_pkey");

            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("true");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<BankMaster>(entity =>
        {
            entity.HasKey(e => e.BankId).HasName("bank_master_pkey");
        });

        modelBuilder.Entity<BeneficiariesMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("beneficiaries_pkey");

            entity.Property(e => e.GpfNo).IsFixedLength();
            entity.Property(e => e.Gstin).IsFixedLength();
            entity.Property(e => e.IsVerified).HasDefaultValueSql("0");
            entity.Property(e => e.MobileNo).IsFixedLength();
            entity.Property(e => e.Status).HasDefaultValueSql("0");

            entity.HasOne(d => d.BenTypeNavigation).WithMany(p => p.BeneficiariesMasters)
                .HasPrincipalKey(p => p.Desc)
                .HasForeignKey(d => d.BenType)
                .HasConstraintName("Fk_beneficiary_type");
        });

        modelBuilder.Entity<BeneficiaryMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("beneficiary_master_pkey");

            entity.Property(e => e.AadhaarNo).IsFixedLength();
            entity.Property(e => e.GpfNo).IsFixedLength();
            entity.Property(e => e.GpfPranNumber).IsFixedLength();
            entity.Property(e => e.MobileNo).IsFixedLength();
            entity.Property(e => e.PanNo).IsFixedLength();
            entity.Property(e => e.TrsyCode).IsFixedLength();
            entity.Property(e => e.VendorGstin).IsFixedLength();
            entity.Property(e => e.VerifiedByDdo).IsFixedLength();

            entity.HasOne(d => d.BankCodeNavigation).WithMany(p => p.BeneficiaryMasters).HasConstraintName("beneficiary_master_bank_code_fkey");

            entity.HasOne(d => d.BeneficiaryTypeNavigation).WithMany(p => p.BeneficiaryMasters).HasConstraintName("beneficiary_master_beneficiary_type_fkey");

            entity.HasOne(d => d.Group).WithMany(p => p.BeneficiaryMasters).HasConstraintName("beneficiary_master_group_id_fkey");

            entity.HasOne(d => d.IfscCodeNavigation).WithMany(p => p.BeneficiaryMasters).HasConstraintName("beneficiary_master_ifsc_code_fkey");

            entity.HasOne(d => d.VerifiedByDdoNavigation).WithMany(p => p.BeneficiaryMasters)
                .HasPrincipalKey(p => p.Code)
                .HasForeignKey(d => d.VerifiedByDdo)
                .HasConstraintName("beneficiary_master_verified_by_ddo_fkey");
        });

        modelBuilder.Entity<BeneficiaryType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Beneficiary_Type_pkey");
        });

        modelBuilder.Entity<BeneficiaryType1>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("beneficiary_type_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('epradan_master.beneficiary_type_id_seq1'::regclass)");
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
            entity.Property(e => e.CssBenType).HasComment("{ name: 'Non-SNA', code: 2 }, { name: 'SNA', code: 1 }");
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
            entity.Property(e => e.SnaGrantType).HasComment("{ name: 'Grant-in-Aid in Cash', code: 1 }, { name: 'Grant-in-Aid in Kind', code: 2 }");
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
        });

        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("branchs_pkey");

            entity.Property(e => e.MicrCode).IsFixedLength();
            entity.Property(e => e.Pincode).IsFixedLength();

            entity.HasOne(d => d.BankCodeNavigation).WithMany(p => p.Branches)
                .HasPrincipalKey(p => p.BankCode)
                .HasForeignKey(d => d.BankCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("bank_code_fkey");
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

        modelBuilder.Entity<Challan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("challan_pkey");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.MajorHead).IsFixedLength();
        });

        modelBuilder.Entity<ChallanEntry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("challan_entry_pkey");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.MajorHead).IsFixedLength();
            entity.Property(e => e.TreasuryCode).IsFixedLength();
        });

        modelBuilder.Entity<ChequeCount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cheque_count_pkey");

            entity.Property(e => e.MicrCode).IsFixedLength();
            entity.Property(e => e.TreasuryCode).IsFixedLength();
            entity.Property(e => e.Utilized).HasDefaultValueSql("0");
        });

        modelBuilder.Entity<ChequeDamage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cheque_damage_pkey");
        });

        modelBuilder.Entity<ChequeDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cheque_details_pkey");

            entity.Property(e => e.IsActive).HasDefaultValueSql("0");

            entity.HasOne(d => d.Bill).WithMany(p => p.ChequeDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cheque_details_bill_id_fkey");
        });

        modelBuilder.Entity<ChequeDistribute>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.MicrCode).IsFixedLength();
            entity.Property(e => e.TreasurieCode).IsFixedLength();
        });

        modelBuilder.Entity<ChequeEntry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cheque_pkey1");

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('cts.cheque_id_seq1'::regclass)");
            entity.Property(e => e.IsActive).HasDefaultValueSql("true");
            entity.Property(e => e.MicrCode).IsFixedLength();
            entity.Property(e => e.TreasurieCode).IsFixedLength();
        });

        modelBuilder.Entity<ChequeIndent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cheque_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('cts.cheque_id_seq'::regclass)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.MemoNo).IsFixedLength();
            entity.Property(e => e.Status).HasComment("1 = new indent , 2 =  approve by TO , 3= Reject by TO");
            entity.Property(e => e.TotalApprovedQuantity).HasDefaultValueSql("0");
            entity.Property(e => e.TreasurieCode).IsFixedLength();

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.ChequeIndents).HasConstraintName("cheque_indent_status_fkey");
        });

        modelBuilder.Entity<ChequeIndentDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cheque_indent_details_pkey");

            entity.Property(e => e.ApprovedMicrCode).IsFixedLength();
            entity.Property(e => e.ChequeType).HasComment("1= treasury 2= others");
            entity.Property(e => e.MicrCode).IsFixedLength();

            entity.HasOne(d => d.ChequeIndent).WithMany(p => p.ChequeIndentDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cheque_indent_details-cheque_indent_id-fkey");
        });

        modelBuilder.Entity<ChequeInvoice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cheque_invoice_pkey");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.InvoiceNumber).IsFixedLength();

            entity.HasOne(d => d.ChequeIndent).WithMany(p => p.ChequeInvoices).HasConstraintName("cheque_invoice_cheque_indent_id_fkey");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.ChequeInvoices).HasConstraintName("cheque_invoice_status_fkey");
        });

        modelBuilder.Entity<ChequeInvoiceDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cheque_invoice_details_pkey");

            entity.HasOne(d => d.ChequeEntry).WithMany(p => p.ChequeInvoiceDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cheque_invoice_details__cheque_entry_id__fkey");

            entity.HasOne(d => d.ChequeIndentDetail).WithMany(p => p.ChequeInvoiceDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cheque_invoice_details__cheque_indent_detail_id__fkey");

            entity.HasOne(d => d.ChequeInvoice).WithMany(p => p.ChequeInvoiceDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cheque_invoice_details__cheque_invoice_id__fkey");
        });

        modelBuilder.Entity<ChequeReceived>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cheque_received_pkey");

            entity.Property(e => e.MicrCode).IsFixedLength();
            entity.Property(e => e.TreasurieCode).IsFixedLength();
        });

        modelBuilder.Entity<Ddo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ddo_pkey");

            entity.Property(e => e.Code).IsFixedLength();
            entity.Property(e => e.Phone).IsFixedLength();
            entity.Property(e => e.TreasuryCode).IsFixedLength();
        });

        modelBuilder.Entity<DdoAllotmentActual>(entity =>
        {
            entity.HasKey(e => e.AllotmentId).HasName("ddo_transactions_pkey");

            entity.Property(e => e.ActualReleasedAmount).HasDefaultValueSql("0");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.SaoDdoCode).IsFixedLength();
            entity.Property(e => e.TreasuryCode).IsFixedLength();

            entity.HasOne(d => d.FinancialYearNavigation).WithMany(p => p.DdoAllotmentActuals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ddo_allotment_actual_financial_year_fkey");

            entity.HasOne(d => d.SaoDdoCodeNavigation).WithMany(p => p.DdoAllotmentActuals)
                .HasPrincipalKey(p => p.Code)
                .HasForeignKey(d => d.SaoDdoCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ddo_allotment_actual_sao_ddo_code_fkey");

            entity.HasOne(d => d.TreasuryCodeNavigation).WithMany(p => p.DdoAllotmentActuals)
                .HasPrincipalKey(p => p.Code)
                .HasForeignKey(d => d.TreasuryCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ddo_allotment_actual_treasury_code_fkey");
        });

        modelBuilder.Entity<DdoAllotmentBookedBill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("billing_ddo_allotment_booked_bill_pkey");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.DdoCode).IsFixedLength();
            entity.Property(e => e.TreasuryCode).IsFixedLength();

            entity.HasOne(d => d.ActiveHoa).WithMany(p => p.DdoAllotmentBookedBills)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("active_hoa_id_bantan_master_hoa_id");

            entity.HasOne(d => d.Allotment).WithMany(p => p.DdoAllotmentBookedBills).HasConstraintName("allotment_id_cts_ddo_allotment_actual_fkey");

            entity.HasOne(d => d.AllotmentNavigation).WithMany(p => p.DdoAllotmentBookedBills).HasConstraintName("ddo_allotment_booked_bill_allotment_id_fkey");

            entity.HasOne(d => d.DdoCodeNavigation).WithMany(p => p.DdoAllotmentBookedBills)
                .HasPrincipalKey(p => p.Code)
                .HasForeignKey(d => d.DdoCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ddo_allotment_booked_bill_ddo_code_fkey");

            entity.HasOne(d => d.FinancialYearNavigation).WithMany(p => p.DdoAllotmentBookedBills)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ddo_allotment_booked_bill_financial_year_fkey");

            entity.HasOne(d => d.TreasuryCodeNavigation).WithMany(p => p.DdoAllotmentBookedBills)
                .HasPrincipalKey(p => p.Code)
                .HasForeignKey(d => d.TreasuryCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ddo_allotment_booked_bill_treasury_code_fkey");
        });

        modelBuilder.Entity<DdoAllotmentProvisional>(entity =>
        {
            entity.HasKey(e => e.AllotmentId).HasName("ddo_transactions_pkey");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.SaoDdoCode).IsFixedLength();
            entity.Property(e => e.TreasuryCode).IsFixedLength();

            entity.HasOne(d => d.FinancialYearNavigation).WithMany(p => p.DdoAllotmentProvisionals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ddo_allotment_provisional_financial_year_fkey");

            entity.HasOne(d => d.SaoDdoCodeNavigation).WithMany(p => p.DdoAllotmentProvisionals)
                .HasPrincipalKey(p => p.Code)
                .HasForeignKey(d => d.SaoDdoCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ddo_allotment_provisional_sao_ddo_code_fkey");

            entity.HasOne(d => d.TreasuryCodeNavigation).WithMany(p => p.DdoAllotmentProvisionals)
                .HasPrincipalKey(p => p.Code)
                .HasForeignKey(d => d.TreasuryCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ddo_allotment_provisional_treasury_code_fkey");
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

        modelBuilder.Entity<DdoWalletActual>(entity =>
        {
            entity.HasKey(e => e.WalletId).HasName("ddo_wallet_actual_pkey");

            entity.Property(e => e.WalletId).ValueGeneratedNever();
            entity.Property(e => e.ActualReleasedAmount).HasDefaultValueSql("0");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.SaoDdoCode).IsFixedLength();
            entity.Property(e => e.TreasuryCode).IsFixedLength();

            entity.HasOne(d => d.FinancialYearNavigation).WithMany(p => p.DdoWalletActuals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ddo_wallet_actual_financial_year_fkey");

            entity.HasOne(d => d.SaoDdoCodeNavigation).WithMany(p => p.DdoWalletActuals)
                .HasPrincipalKey(p => p.Code)
                .HasForeignKey(d => d.SaoDdoCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ddo_wallet_actual_sao_ddo_code_fkey");

            entity.HasOne(d => d.TreasuryCodeNavigation).WithMany(p => p.DdoWalletActuals)
                .HasPrincipalKey(p => p.Code)
                .HasForeignKey(d => d.TreasuryCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ddo_wallet_actual_treasury_code_fkey");

            entity.HasOne(d => d.Wallet).WithOne(p => p.DdoWalletActual)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ddo_wallet_actual_wallet_id_fkey");
        });

        modelBuilder.Entity<DdoWalletProvisional>(entity =>
        {
            entity.HasKey(e => e.WalletId).HasName("ddo_wallet_provisional_pkey");

            entity.Property(e => e.WalletId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.SaoDdoCode).IsFixedLength();
            entity.Property(e => e.TreasuryCode).IsFixedLength();

            entity.HasOne(d => d.FinancialYearNavigation).WithMany(p => p.DdoWalletProvisionals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ddo_wallet_provisional_financial_year_fkey");

            entity.HasOne(d => d.SaoDdoCodeNavigation).WithMany(p => p.DdoWalletProvisionals)
                .HasPrincipalKey(p => p.Code)
                .HasForeignKey(d => d.SaoDdoCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ddo_wallet_provisional_sao_ddo_code_fkey");

            entity.HasOne(d => d.TreasuryCodeNavigation).WithMany(p => p.DdoWalletProvisionals)
                .HasPrincipalKey(p => p.Code)
                .HasForeignKey(d => d.TreasuryCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ddo_wallet_provisional_treasury_code_fkey");

            entity.HasOne(d => d.Wallet).WithOne(p => p.DdoWalletProvisional)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ddo_wallet_provisional_wallet_id_fkey");
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

        modelBuilder.Entity<DiscountDetail>(entity =>
        {
            entity.HasKey(e => e.DiscountId).HasName("discount_details_pkey");

            entity.Property(e => e.IsActive).HasDefaultValueSql("true");

            entity.HasOne(d => d.StampCategory).WithMany(p => p.DiscountDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("discount_details_stamp_category_id_fkey");

            entity.HasOne(d => d.VendorTypeNavigation).WithMany(p => p.DiscountDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("discount_details_vendor_type_fkey");
        });

        modelBuilder.Entity<DmlHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("dml_history_pkey");

            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<EcsNeftDetail>(entity =>
        {
            entity.Property(e => e.BankAccountNumber).IsFixedLength();
            entity.Property(e => e.ContactNumber).IsFixedLength();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.IfscCode).IsFixedLength();
            entity.Property(e => e.IsActive).HasDefaultValueSql("1");
            entity.Property(e => e.PanNo).IsFixedLength();

            entity.HasOne(d => d.Bill).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("bill_id_fkey");

            entity.HasOne(d => d.PayeeTypeNavigation).WithMany().HasConstraintName("payee_type_fkey");
        });

        modelBuilder.Entity<EcsNeftPaymentStatusDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ecs_neft_payment_status_details_pkey");
        });

        modelBuilder.Entity<EcsNeftPreviousRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ecs_neft_previous_record_pkey");
        });

        modelBuilder.Entity<FailedBeneficiaryRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("failed_beneficiary_records_pkey");

            entity.Property(e => e.AccountNumber).IsFixedLength();
            entity.Property(e => e.IfscCode).IsFixedLength();
            entity.Property(e => e.MobileNo).IsFixedLength();
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

        modelBuilder.Entity<GroupType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("group_type_pkey");
        });

        modelBuilder.Entity<LfplEc>(entity =>
        {
            entity.Property(e => e.BeneficiaryAccno).IsFixedLength();
            entity.Property(e => e.BeneficiaryIfsc).IsFixedLength();
            entity.Property(e => e.BeneficiaryName).IsFixedLength();
            entity.Property(e => e.BtCount).HasDefaultValueSql("0");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.TreasuryCode).IsFixedLength();

            entity.HasOne(d => d.Beneficiary).WithMany().HasConstraintName("ecs_beneficiary_id_fkey");

            entity.HasOne(d => d.RefNoNavigation).WithMany()
                .HasPrincipalKey(p => p.RefNo)
                .HasForeignKey(d => d.RefNo)
                .HasConstraintName("ref_no_fkey");
        });

        modelBuilder.Entity<LfplSchemesWallet>(entity =>
        {
            entity.Property(e => e.TreasuryCode).IsFixedLength();
        });

        modelBuilder.Entity<LifeCertificate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("life_certificates_pkey");

            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("true");
            entity.Property(e => e.CertificateFlag).HasDefaultValueSql("false");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
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

        modelBuilder.Entity<Nominee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("nominees_pkey");

            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("true");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.PhotoFile).WithMany(p => p.NomineePhotoFiles).HasConstraintName("nominees_photo_file_id_fkey");

            entity.HasOne(d => d.SignatureFile).WithMany(p => p.NomineeSignatureFiles).HasConstraintName("nominees_signature_file_id_fkey");
        });

        modelBuilder.Entity<OperatorMaster>(entity =>
        {
            entity.HasKey(e => new { e.OperatorCode, e.OperatorName, e.PaymentHoa, e.ReceiptHoa, e.TreasuryCode }).HasName("operator_master_pkey1");

            entity.Property(e => e.TreasuryCode).IsFixedLength();
            entity.Property(e => e.OpId).ValueGeneratedOnAdd();
            entity.Property(e => e.OperatorType).IsFixedLength();
        });

        modelBuilder.Entity<PaymentAdvice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("payment_advice_pkey");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("1")
                .HasComment("1=sortlist 2=paymandate");
        });

        modelBuilder.Entity<PaymentAdviceHasBeneficiary>(entity =>
        {
            entity.Property(e => e.BankAccountNumber).IsFixedLength();
            entity.Property(e => e.ContactNumber).IsFixedLength();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.IfscCode).IsFixedLength();
            entity.Property(e => e.PanNo).IsFixedLength();
        });

        modelBuilder.Entity<Pensioner>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pensioners_pkey");

            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("true");
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

            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("true");
            entity.Property(e => e.AdhocPensionFlag).HasDefaultValueSql("false");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.DoublePensionFlag).HasDefaultValueSql("false");
            entity.Property(e => e.EmployeedFlag).HasDefaultValueSql("false");
            entity.Property(e => e.FirstPensionFlag).HasDefaultValueSql("false");
            entity.Property(e => e.HealthSchemeFlag).HasDefaultValueSql("false");
            entity.Property(e => e.InterimPensionFlag).HasDefaultValueSql("false");
            entity.Property(e => e.PpoApprovedFlag).HasDefaultValueSql("false");
            entity.Property(e => e.ReemployedFlag).HasDefaultValueSql("false");
            entity.Property(e => e.SharedPensionFlag).HasDefaultValueSql("false");
            entity.Property(e => e.StatusActiveFlag).HasDefaultValueSql("false");
        });

        modelBuilder.Entity<Reference>(entity =>
        {
            entity.HasKey(e => e.RefId).HasName("reference_pkey");

            entity.Property(e => e.Status).HasDefaultValueSql("2");
            entity.Property(e => e.TreasuryCode).IsFixedLength();

            entity.HasOne(d => d.Advice).WithMany(p => p.References).HasConstraintName("advice_id_fkey");

            entity.HasOne(d => d.RefTypeNavigation).WithMany(p => p.References).HasConstraintName("ref_type_fkey");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.References).HasConstraintName("ref_status_fkey");
        });

        modelBuilder.Entity<ReferenceDetail>(entity =>
        {
            entity.Property(e => e.RefId).ValueGeneratedOnAdd();
            entity.Property(e => e.TreasuryCode).IsFixedLength();

            entity.HasOne(d => d.RefNoNavigation).WithMany()
                .HasPrincipalKey(p => p.RefNo)
                .HasForeignKey(d => d.RefNo)
                .HasConstraintName("ref_no_fkey");
        });

        modelBuilder.Entity<ReferenceStatusMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("status_master_pkey");
        });

        modelBuilder.Entity<ReferenceType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("reference_type_pkey");
        });

        modelBuilder.Entity<StampCategory>(entity =>
        {
            entity.HasKey(e => e.StampCategoryId).HasName("stamp_category_pkey");

            entity.Property(e => e.IsActive).HasDefaultValueSql("true");
            entity.Property(e => e.StampCategory1).IsFixedLength();
            entity.Property(e => e.SubDtlHead).IsFixedLength();
        });

        modelBuilder.Entity<StampCombination>(entity =>
        {
            entity.HasKey(e => e.StampCombinationId).HasName("stamp_combination_pkey");

            entity.Property(e => e.IsActive).HasDefaultValueSql("true");

            entity.HasOne(d => d.StampCategory).WithMany(p => p.StampCombinations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("stamp_combination_stamp_category_id_fkey");

            entity.HasOne(d => d.StampLabel).WithMany(p => p.StampCombinations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("stamp_combination_stamp_label_id_fkey");

            entity.HasOne(d => d.StampType).WithMany(p => p.StampCombinations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("stamp_combination_stamp_type_id_fkey");
        });

        modelBuilder.Entity<StampIndent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("stamp_indent_pkey");

            entity.Property(e => e.RaisedByTreasuryCode).IsFixedLength();
            entity.Property(e => e.RaisedToTreasuryCode).IsFixedLength();

            entity.HasOne(d => d.StampCombination).WithMany(p => p.StampIndents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("stamp_indent_stamp_combination_id_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.StampIndents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("stamp_indent_status_fkey");
        });

        modelBuilder.Entity<StampInventory>(entity =>
        {
            entity.HasKey(e => e.StampInventoryId).HasName("stamp_inventory_pkey");
        });

        modelBuilder.Entity<StampInventory1>(entity =>
        {
            entity.HasKey(e => e.StampInventoryId).HasName("stamp_inventory_pkey");

            entity.Property(e => e.StampCategory).IsFixedLength();
            entity.Property(e => e.TreasuryCode).IsFixedLength();
        });

        modelBuilder.Entity<StampInvoice>(entity =>
        {
            entity.HasKey(e => e.StampInvoiceId).HasName("stamp_invoice_pkey");

            entity.HasOne(d => d.StampIndent).WithMany(p => p.StampInvoices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("stamp_invoice_stamp_indent_id_fkey");
        });

        modelBuilder.Entity<StampLabelMaster>(entity =>
        {
            entity.HasKey(e => e.LabelId).HasName("stamp_label_master_pkey");

            entity.Property(e => e.IsActive).HasDefaultValueSql("true");
        });

        modelBuilder.Entity<StampType>(entity =>
        {
            entity.HasKey(e => e.DenominationId).HasName("stamp_type_pkey");

            entity.Property(e => e.IsActive).HasDefaultValueSql("true");
        });

        modelBuilder.Entity<StampVendor>(entity =>
        {
            entity.HasKey(e => e.StampVendorId).HasName("stamp_vendor_pkey");

            entity.Property(e => e.StampVendorId).HasDefaultValueSql("nextval('cts_master.stamp_vendor_vendor_code_seq'::regclass)");
            entity.Property(e => e.IsActive).HasDefaultValueSql("true");
            entity.Property(e => e.Treasury).IsFixedLength();

            entity.HasOne(d => d.VendorTypeNavigation).WithMany(p => p.StampVendors)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("stamp_vendor_vendor_type_fkey");
        });

        modelBuilder.Entity<StampVendorType>(entity =>
        {
            entity.HasKey(e => e.VendorTypeId).HasName("stamp_vendor_type_pkey");

            entity.Property(e => e.VendorTypeId).HasDefaultValueSql("nextval('cts_master.vendor_type_vendor_type_id_seq'::regclass)");
            entity.Property(e => e.IsActive).HasDefaultValueSql("true");
        });

        modelBuilder.Entity<StampWallet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("stamp_wallet_pkey");

            entity.Property(e => e.TreasuryCode).IsFixedLength();

            entity.HasOne(d => d.Combination).WithMany(p => p.StampWallets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("stamp_wallet_combination_id_fkey");
        });

        modelBuilder.Entity<StampWalletTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("stamp_wallet_transaction_pkey");

            entity.Property(e => e.FromTreasuryCode).IsFixedLength();
            entity.Property(e => e.ToTreasuryCode).IsFixedLength();

            entity.HasOne(d => d.ToTreasuryCodeNavigation).WithMany(p => p.StampWalletTransactions)
                .HasPrincipalKey(p => p.Code)
                .HasForeignKey(d => d.ToTreasuryCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("stamp_wallet_transaction_to_treasury_code_fkey");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("status_pkey");

            entity.Property(e => e.Type).HasComment("1 = token flow ,2 = Cheque indent,3 Cheque invoice, 4 = Cheque Received, 7 = Online PL");
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

        modelBuilder.Entity<TransactionLot>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("transaction_lot_pkey");

            entity.Property(e => e.DrnNo).IsFixedLength();
            entity.Property(e => e.LotNo).IsFixedLength();
            entity.Property(e => e.Status).HasComment("1=lot generate");
        });

        modelBuilder.Entity<TransactionLotHasBeneficiary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("transaction_lot_has_beneficiarys_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('cts_payment.transaction_lot_has_beneficiarys_id_seq'::regclass)");
            entity.Property(e => e.AccountNumber).IsFixedLength();
            entity.Property(e => e.IfscCode).IsFixedLength();
            entity.Property(e => e.MobileNo).IsFixedLength();

            entity.HasOne(d => d.TransactionLot).WithMany(p => p.TransactionLotHasBeneficiaries).HasConstraintName("transaction_lot_fky");
        });

        modelBuilder.Entity<TransactionLotHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("transaction_lot_history _pkey");

            entity.Property(e => e.TransactionLotId).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Treasury>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("treasury_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).IsFixedLength();
        });

        modelBuilder.Entity<TreasuryHasBranch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("treasury_has_branch_pkey");

            entity.HasOne(d => d.Branchs).WithMany(p => p.TreasuryHasBranches).HasConstraintName("branch_id_fkey");

            entity.HasOne(d => d.Treasury).WithMany(p => p.TreasuryHasBranches).HasConstraintName("treasury_id_fkey");
        });

        modelBuilder.Entity<UploadedFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("uploaded_files_pkey");

            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("true");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<UserList>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.TreasurieCode).IsFixedLength();
        });

        modelBuilder.Entity<VBillDetail>(entity =>
        {
            entity.Property(e => e.BillNo).IsFixedLength();
            entity.Property(e => e.DdoCode).IsFixedLength();
            entity.Property(e => e.Demand).IsFixedLength();
            entity.Property(e => e.DetailHead).IsFixedLength();
            entity.Property(e => e.MajorHead).IsFixedLength();
            entity.Property(e => e.MinorHead).IsFixedLength();
            entity.Property(e => e.PlanStatus).IsFixedLength();
            entity.Property(e => e.ReferenceNo).IsFixedLength();
            entity.Property(e => e.SchemeHead).IsFixedLength();
            entity.Property(e => e.SubMajorHead).IsFixedLength();
            entity.Property(e => e.TreasuryCode).IsFixedLength();
        });

        modelBuilder.Entity<VPaymentAdvice>(entity =>
        {
            entity.Property(e => e.BankAccountNumber).IsFixedLength();
            entity.Property(e => e.ContactNumber).IsFixedLength();
            entity.Property(e => e.IfscCode).IsFixedLength();
            entity.Property(e => e.PanNo).IsFixedLength();
        });

        modelBuilder.Entity<VendorRequisitionApprove>(entity =>
        {
            entity.HasKey(e => e.VendorRequisitionApproveId).HasName("vendor_requisition_approve_pkey");

            entity.Property(e => e.ApproveDate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.VendorRequisition).WithMany(p => p.VendorRequisitionApproves)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("vendor_requisition_approve_vendor_requisition_id_fkey");
        });

        modelBuilder.Entity<VendorRequisitionChallanGenerate>(entity =>
        {
            entity.HasKey(e => e.VendorRequisitionChallanGenerateId).HasName("vendor_requisition_challan_generate_pkey");

            entity.Property(e => e.VendorRequisitionChallanGenerateId).HasDefaultValueSql("nextval('cts.vendor_requisition_challan_ge_vendor_requisition_challan_ge_seq'::regclass)");
            entity.Property(e => e.IsBilled).HasDefaultValueSql("false");

            entity.HasOne(d => d.VendorRequisitionStaging).WithMany(p => p.VendorRequisitionChallanGenerates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("vendor_requisition_challan_ge_vendor_requisition_staging_i_fkey");
        });

        modelBuilder.Entity<VendorRequisitionStaging>(entity =>
        {
            entity.HasKey(e => e.VendorRequisitionStagingId).HasName("vendor_requisition_staging_pkey");

            entity.HasOne(d => d.VendorRequisition).WithMany(p => p.VendorRequisitionStagings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("vendor_requisition_staging_vendor_requisition_id_fkey");
        });

        modelBuilder.Entity<VendorStampRequisition>(entity =>
        {
            entity.HasKey(e => e.VendorStampRequisitionId).HasName("vendor_stamp_requisition_pkey");

            entity.Property(e => e.RaisedToTreasury).IsFixedLength();

            entity.HasOne(d => d.Combination).WithMany(p => p.VendorStampRequisitions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("vendor_stamp_requisition_combination_id_fkey");

            entity.HasOne(d => d.RaisedToTreasuryNavigation).WithMany(p => p.VendorStampRequisitions)
                .HasPrincipalKey(p => p.Code)
                .HasForeignKey(d => d.RaisedToTreasury)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("vendor_stamp_requisition_raised_to_treasury_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.VendorStampRequisitions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("vendor_stamp_requisition_status_id_fkey");

            entity.HasOne(d => d.Vendor).WithMany(p => p.VendorStampRequisitions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("vendor_stamp_requisition_vendor_id_fkey");

            entity.HasOne(d => d.VendorRequisitionApprove).WithMany(p => p.VendorStampRequisitions).HasConstraintName("vendor_stamp_requisition_vendor_requisition_approve_id_fkey");

            entity.HasOne(d => d.VendorRequisitionChallanGenerate).WithMany(p => p.VendorStampRequisitions).HasConstraintName("vendor_stamp_requisition_vendor_requisition_challan_genera_fkey");

            entity.HasOne(d => d.VendorRequisitionStaging).WithMany(p => p.VendorStampRequisitions).HasConstraintName("vendor_stamp_requisition_vendor_requisition_staging_id_fkey");
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("voucher_pkey");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.MajorHead).IsFixedLength();
        });

        modelBuilder.Entity<VoucherEntry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("voucher_entry_pkey");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.LastVoucherNo).HasDefaultValueSql("1");
            entity.Property(e => e.MajorHead).IsFixedLength();
            entity.Property(e => e.TreasuryCode).IsFixedLength();
        });
        modelBuilder.HasSequence("category_type_category_type_id_seq", "cts_master");
        modelBuilder.HasSequence("discount_details_discount_id_seq", "cts_master");
        modelBuilder.HasSequence("stamp_category_stamp_category_id_seq", "cts_master");
        modelBuilder.HasSequence("stamp_combination_stamp_combination_id_seq", "cts_master");
        modelBuilder.HasSequence("stamp_indent_id_seq", "cts");
        modelBuilder.HasSequence("stamp_invoice_stamp_invoice_id_seq", "cts");
        modelBuilder.HasSequence("stamp_label_master_label_id_seq", "cts_master");
        modelBuilder.HasSequence("stamp_type_denomination_id_seq", "cts_master");
        modelBuilder.HasSequence("stamp_vendor_vendor_code_seq", "cts_master");
        modelBuilder.HasSequence("stamp_wallet_transaction_id_seq", "master");
        modelBuilder.HasSequence("vendor_requisition_approve_vendor_requisition_approve_id_seq", "cts");
        modelBuilder.HasSequence("vendor_requisition_challan_ge_vendor_requisition_challan_ge_seq", "cts");
        modelBuilder.HasSequence("vendor_requisition_staging_vendor_requisition_staging_id_seq", "cts");
        modelBuilder.HasSequence("vendor_stamp_requisition_vendor_stamp_requisition_id_seq", "cts");
        modelBuilder.HasSequence("vendor_type_vendor_type_id_seq", "cts_master");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
