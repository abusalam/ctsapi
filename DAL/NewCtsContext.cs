using System;
using System.Collections.Generic;
using CTS_BE.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL;

public partial class NewCtsContext : DbContext
{
    public NewCtsContext()
    {
    }

    public NewCtsContext(DbContextOptions<NewCtsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<PMdEmpNomineeDtl> PMdEmpNomineeDtls { get; set; }

    public virtual DbSet<PMdPenEmpCopyForwardTo> PMdPenEmpCopyForwardTos { get; set; }

    public virtual DbSet<PMdPenEmpWiseForwardList> PMdPenEmpWiseForwardLists { get; set; }

    public virtual DbSet<PMdPenPrepAttachment> PMdPenPrepAttachments { get; set; }

    public virtual DbSet<PMdPenPrepCalculatedAmt> PMdPenPrepCalculatedAmts { get; set; }

    public virtual DbSet<PMdPenPrepCalculationHdr> PMdPenPrepCalculationHdrs { get; set; }

    public virtual DbSet<PMdPenPrepEmolumentDtl> PMdPenPrepEmolumentDtls { get; set; }

    public virtual DbSet<PMdPenPrepOthOutstanding> PMdPenPrepOthOutstandings { get; set; }

    public virtual DbSet<PMdPenPrepOutstandingLoan> PMdPenPrepOutstandingLoans { get; set; }

    public virtual DbSet<PMdPenPrepOutstndLoanDtl> PMdPenPrepOutstndLoanDtls { get; set; }

    public virtual DbSet<PMdPenPrepPayDtl> PMdPenPrepPayDtls { get; set; }

    public virtual DbSet<PMdPenPrepProvPensionDtl> PMdPenPrepProvPensionDtls { get; set; }

    public virtual DbSet<PMdPenPrepServiceDtl> PMdPenPrepServiceDtls { get; set; }

    public virtual DbSet<PMdPenPrepType> PMdPenPrepTypes { get; set; }

    public virtual DbSet<PMdPensionerAddress> PMdPensionerAddresses { get; set; }

    public virtual DbSet<PMmExitForwardingList> PMmExitForwardingLists { get; set; }

    public virtual DbSet<PMmPenBenfTypeRelMap> PMmPenBenfTypeRelMaps { get; set; }

    public virtual DbSet<PMmPenCommutationRate> PMmPenCommutationRates { get; set; }

    public virtual DbSet<PMmPenHoaMap> PMmPenHoaMaps { get; set; }

    public virtual DbSet<PMmPenPrepAttachement> PMmPenPrepAttachements { get; set; }

    public virtual DbSet<PMmPenPrepCalcHeader> PMmPenPrepCalcHeaders { get; set; }

    public virtual DbSet<PMmPenPrepDeclaration> PMmPenPrepDeclarations { get; set; }

    public virtual DbSet<PMmPenPrepPayRecoType> PMmPenPrepPayRecoTypes { get; set; }

    public virtual DbSet<PMmPenPrepPensionerDetl> PMmPenPrepPensionerDetls { get; set; }

    public virtual DbSet<PMmPenProcessingFlag> PMmPenProcessingFlags { get; set; }

    public virtual DbSet<PMmPenRetirementBenfType> PMmPenRetirementBenfTypes { get; set; }

    public virtual DbSet<PMmPenRuleDtl> PMmPenRuleDtls { get; set; }

    public virtual DbSet<PMmPenRuleName> PMmPenRuleNames { get; set; }

    public virtual DbSet<PMmPenRuleSubDtl> PMmPenRuleSubDtls { get; set; }

    public virtual DbSet<PTdPenPrepAttachementDtl> PTdPenPrepAttachementDtls { get; set; }

    public virtual DbSet<PTdPenPrepAttachmentDtl> PTdPenPrepAttachmentDtls { get; set; }

    public virtual DbSet<PTdPenPrepFamilyAddress> PTdPenPrepFamilyAddresses { get; set; }

    public virtual DbSet<PTdPenPrepFamilyDtl> PTdPenPrepFamilyDtls { get; set; }

    public virtual DbSet<PTdPenPrepFamilyPenDtl> PTdPenPrepFamilyPenDtls { get; set; }

    public virtual DbSet<PTdPenPrepFileProcessLog> PTdPenPrepFileProcessLogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConnectionStrings:DBConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("cts_pension", "ACTIVE_FLAG", new[] { "Y", "N" })
            .HasPostgresEnum("cts_pension", "ADDR_TYPE", new[] { "PM", "PR", "CC" })
            .HasPostgresEnum("cts_pension", "COMPONENT_TYPE", new[] { "P", "F", "C" })
            .HasPostgresEnum("cts_pension", "COURT_CASE_PENDING_STATUS", new[] { "Y", "N" })
            .HasPostgresEnum("cts_pension", "DEPT_CRIMINAL_PROC_PENDING_FLG", new[] { "Y", "N" })
            .HasPostgresEnum("cts_pension", "EMP_GENDER_OBS", new[] { "M", "F" })
            .HasPostgresEnum("cts_pension", "FAMILY_PENSIONER_EMPLOYED_FLAG", new[] { "Y", "N" })
            .HasPostgresEnum("cts_pension", "FAMILY_PEN_RCPT_OTH_PEN_FLAG", new[] { "Y", "N" })
            .HasPostgresEnum("cts_pension", "FINAL_GPF_APPLIED_FLAG", new[] { "Y", "N" })
            .HasPostgresEnum("cts_pension", "GENDER_OBS", new[] { "M", "F" })
            .HasPostgresEnum("cts_pension", "HANDICAPP_FLAG_OBS", new[] { "Y", "N" })
            .HasPostgresEnum("cts_pension", "MANDATORY_FLAG", new[] { "Y", "O", "N" })
            .HasPostgresEnum("cts_pension", "MINOR_FLAG_OBS", new[] { "Y", "N" })
            .HasPostgresEnum("cts_pension", "OPTIONAL_MANDATORY_FLAG", new[] { "O", "M" })
            .HasPostgresEnum("cts_pension", "OTHER_PENSION_RECEIPT_FLAG", new[] { "Y", "N" })
            .HasPostgresEnum("cts_pension", "PAYMENT_ORDER_FLAG", new[] { "P", "G", "C" })
            .HasPostgresEnum("cts_pension", "PAY_RECO_FLAG", new[] { "P", "R" })
            .HasPostgresEnum("cts_pension", "PERIOD_TYPE", new[] { "SHRMS", "SNHRMS", "SADD", "SDEPH", "SDEPNH", "SNQHL", "SNQNHL", "SNQHS", "SNQNHS", "SNQO", "WSADM" })
            .HasPostgresEnum("cts_pension", "PREVIOUS_PENSION_SOURCE", new[] { "M", "C", "O" })
            .HasPostgresEnum("cts_pension", "PREVIOUS_PENSION_TYPE", new[] { "N", "F" })
            .HasPostgresEnum("cts_pension", "PROVISIONAL_GRATUITY_RCVD_FLAG", new[] { "Y", "N" })
            .HasPostgresEnum("cts_pension", "PROVISIONAL_PEN_RECEIVED_FLAG", new[] { "Y", "N" })
            .HasPostgresEnum("cts_pension", "QUAL_NON_QUAL_WEIGHTAGE_TYPE", new[] { "Q", "N", "W" })
            .HasPostgresEnum("cts_pension", "RECOVERY_FROM", new[] { "P", "G" })
            .HasPostgresEnum("cts_pension", "RE_EMPLOYED_AFTER_RETIRE_FLAG", new[] { "Y", "N" })
            .HasPostgresEnum("cts_pension", "TI_APPLICABLE_FLAG_OBS", new[] { "Y", "N" })
            .HasPostgresEnum("cts_pension", "TRES_BREAKUP_CLASS_TYPE", new[] { "B", "C" })
            .HasPostgresEnum("cts_pension", "VIGILANCE_CASE_PENDING_FLAG", new[] { "Y", "N" });

        modelBuilder.Entity<PMdEmpNomineeDtl>(entity =>
        {
            entity.Property(e => e.AgeOnCreatedDate).HasComment("Nominee age on created date for report");
            entity.Property(e => e.CreatedTimestamp)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("nominee creation and application date -- added by ritu");
            entity.Property(e => e.CreationAfterDeathFlag).HasComment("Nominee created after death of employee Y/N");
            entity.Property(e => e.MinorFlag).HasDefaultValueSql("'N'::bpchar");
            entity.Property(e => e.MinorFlagCalculatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.ModifiedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.NomDeceasedFlag)
                .HasDefaultValueSql("'N'::character varying")
                .HasComment("'Y' for alive 'N' for dead");
            entity.Property(e => e.NomineeLegalHireFlag)
                .HasDefaultValueSql("'N'::character varying")
                .HasComment("N - nominee L - Legal Hier --added by pk pandit");
            entity.Property(e => e.NomineeType).HasDefaultValueSql("'O'::bpchar");
            entity.Property(e => e.OrgNomIdForAlternate).HasComment("original nominee id for whom this person is an alternate --added by ritu");
            entity.Property(e => e.ProcessingFlag).HasComment("0 initiate, 1    approved, -1 rejected");
            entity.Property(e => e.Reason).HasComment("reason for relation - out --added by ritu");
            entity.Property(e => e.RelationInOut).HasComment("Relation In - 0, out - 1 --added by ritu");
        });

        modelBuilder.Entity<PMdPenEmpCopyForwardTo>(entity =>
        {
            entity.HasKey(e => e.IntCopyForwardId).HasName("PK_P_MD_PEN_COPY_FORWARD_TO");
        });

        modelBuilder.Entity<PMdPenEmpWiseForwardList>(entity =>
        {
            entity.HasKey(e => e.IntPenForwardingId).HasName("PK_MD_PEN_EMP_WISE_FORWARDING");

            entity.Property(e => e.ActiveFlag)
                .HasDefaultValueSql("'Y'::character varying")
                .HasComment("'Y' / 'N' Flag");
        });

        modelBuilder.Entity<PMdPenPrepAttachment>(entity =>
        {
            entity.HasKey(e => e.IntPenPrepAttachement).HasName("PK_P_MD_PEN_PREP_ATTACHEMENT");

            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("'Y'::character varying");
            entity.Property(e => e.CreatedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IntOmiUpdDocType).HasComment("document type from other master master type: 'AFT'");
            entity.Property(e => e.ModifiedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.ModifiedUserId).HasDefaultValueSql("0");
            entity.Property(e => e.ServiceBookExitMngmntFlag).HasComment("For Service book : 'S' for Exit Management 'E'");
        });

        modelBuilder.Entity<PMdPenPrepCalculatedAmt>(entity =>
        {
            entity.HasKey(e => e.IntPrepCalculatedAmtId).HasName("PK_PEN_PREP_CALCULATED_AMT");

            entity.Property(e => e.CreatedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.ModifiedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<PMdPenPrepCalculationHdr>(entity =>
        {
            entity.HasKey(e => e.IntPenPrepCalcHdrId).HasName("PK_PEN_PREP_CALCULATION_HDR");

            entity.Property(e => e.CreatedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.ModifiedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<PMdPenPrepEmolumentDtl>(entity =>
        {
            entity.HasKey(e => e.IntPenPrepEmlDtlsId).HasName("PK_P_MD_PEN_PREP_EMOL_DTLS");

            entity.Property(e => e.CreatedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.ModifiedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<PMdPenPrepOthOutstanding>(entity =>
        {
            entity.HasKey(e => e.IntPenOthOutstandingId).HasName("PK_P_MD_PEN_PREP_OTH_OUTSTAND");

            entity.Property(e => e.CreatedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.ModifiedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.OutstandingType).HasComment("'PA'--- Pay Allowance type overdrawl out standing, 'OTH' other outstanding");
        });

        modelBuilder.Entity<PMdPenPrepOutstandingLoan>(entity =>
        {
            entity.HasKey(e => e.IntPenOutstandingLoanId).HasName("PK_P_MD_PEN_PREP_OS_LOAN");

            entity.Property(e => e.CreatedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.ModifiedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<PMdPenPrepOutstndLoanDtl>(entity =>
        {
            entity.HasKey(e => e.IntPenOutstndLoanDtlId).HasName("PK_P_MD_PEN_OUTS_LOAN_DTL");

            entity.Property(e => e.CreatedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.ModifiedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<PMdPenPrepPayDtl>(entity =>
        {
            entity.Property(e => e.CreatedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.ModifiedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<PMdPenPrepProvPensionDtl>(entity =>
        {
            entity.HasKey(e => e.IntPenProvPensionId).HasName("PK_PEN_PREP_PROV_PENSION_DTL");

            entity.Property(e => e.CreatedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.CreatedUserId).HasDefaultValueSql("99999");
            entity.Property(e => e.ModifiedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.ModifiedUserId).HasDefaultValueSql("99999");
            entity.Property(e => e.PensionRatePerMonthUser).HasComment("as per CR 181305");
            entity.Property(e => e.TotalPensionPeriodUser).HasComment("as per CR 181305");
        });

        modelBuilder.Entity<PMdPenPrepServiceDtl>(entity =>
        {
            entity.Property(e => e.IntPenServiceDtlsId).ValueGeneratedNever();
            entity.Property(e => e.AmtOfContribution).HasComment("Added for Block 'B' & 'C' see CR No 181316 for Details");
            entity.Property(e => e.BusinessPkType).HasComment("Not required from fron end");
            entity.Property(e => e.ContributionReceivedFlag).HasComment("22c Any Contribution Received Y/N flag");
            entity.Property(e => e.ContributionReceivedSource).HasComment("22f");
            entity.Property(e => e.CreatedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.FamilyPensionFlag).HasComment("22e Y/N flag");
            entity.Property(e => e.GoDate).HasComment("Added for Block 'B' & 'C' see CR No 181316 for Details");
            entity.Property(e => e.GoNo).HasComment("Added for Block 'B' & 'C' see CR No 181316 for Details");
            entity.Property(e => e.GovtName).HasComment("Government under which the service");
            entity.Property(e => e.GratuityReceived).HasComment("22d");
            entity.Property(e => e.IntEmpWorkingDtlsId).HasComment("Not required from fron end");
            entity.Property(e => e.IntEmpWorkingDtlsIdDepu).HasComment("Not required from fron end For Deputation");
            entity.Property(e => e.IntLeaveId).HasComment("Not required from fron end");
            entity.Property(e => e.IntLeaveTypeId).HasComment("Not required from fron end");
            entity.Property(e => e.IntOtherServiceTypeId).HasComment("will come from hr_mm_gen_other_master p WHERE p.master_abbr = 'OST'");
            entity.Property(e => e.ModifiedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.PensionReceived).HasComment("22d");
            entity.Property(e => e.PeriodType).HasComment("SHRMS-  Service HRMS,                 SNHRMS- Service Non HRMS,\nSADD-   Additional Service,\nSDEPH-  herms deputation,             SDEPNH- deputation non hrms,\nSNQHL-  non qualifying hrms leave,    SNQNHL-non qualifying non hrms leave,\nSNQHS-  non qualifying hrms suspence, SNQNHS-non qualifying non hrms suspence,\nSNQO-   non qualifying others,        WSADM --- Weightage  ");
        });

        modelBuilder.Entity<PMdPenPrepType>(entity =>
        {
            entity.Property(e => e.PenTypeId)
                .ValueGeneratedNever()
                .HasComment("Unique pension ID type.");
            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("'Y'::bpchar");
            entity.Property(e => e.CategoryId)
                .ValueGeneratedOnAdd()
                .HasComment("Reference column from P_MM_PEN_PREP_CATEGORY..");
            entity.Property(e => e.CreatedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.ModifiedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.PenTypeAbbr).HasComment("added kalyan");
            entity.Property(e => e.PensionCalcType).HasComment("Types are commomn Superannuation & 'Death' 'SP' & 'DP'");
            entity.Property(e => e.PensionTypeFlag).HasComment("'P' for Provisional & 'F' for Final Pension");
            entity.Property(e => e.TypeAbbr).HasComment("'F' = Family");
        });

        modelBuilder.Entity<PMdPensionerAddress>(entity =>
        {
            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.AddrType)
                .HasDefaultValueSql("'PM'::character varying")
                .HasComment("PM for Permanent, PR for present CM for communication address");
            entity.Property(e => e.CreatedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.ModifiedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.SameAsPermanentAddr)
                .HasDefaultValueSql("'N'::character varying")
                .HasComment("N or Y");
        });

        modelBuilder.Entity<PMmExitForwardingList>(entity =>
        {
            entity.HasKey(e => e.IntFrwdingListId).HasName("PK_FORWARDING_LIST");

            entity.ToTable("P_MM_EXIT_FORWARDING_LIST", "cts_pension", tb => tb.HasComment("Details required for generating forwarding letter report"));

            entity.Property(e => e.IntFrwdingListId).HasComment("primary key of this table");
            entity.Property(e => e.ActiveFlag).HasComment("value - Y for active and N for inactive");
            entity.Property(e => e.CreatedTimestamp)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("time when this record was craeted.");
            entity.Property(e => e.InUserId).HasComment("user id of the person who will be generating the report");
            entity.Property(e => e.IntPensionerId).HasComment("foreign key from pension master table");
            entity.Property(e => e.MandatoryFlag).HasComment("value - Y (when mandatory for all benefit types), O (when mandatory based on a certain benefit type), N - (when not mandatory)");
            entity.Property(e => e.OdrSlNo).HasComment("used for showing the list in particular order");
            entity.Property(e => e.PenTypeAbbr).HasComment("abbr for pension type");
            entity.Property(e => e.PenTypeId).HasComment("foreign key from pension type table");
            entity.Property(e => e.ReportName).HasComment("name of the reports whose name needs to be displayed in the forwardign letter report");
        });

        modelBuilder.Entity<PMmPenBenfTypeRelMap>(entity =>
        {
            entity.HasKey(e => e.IntBenfTypeRelMapId).HasName("PK_BENF_TYPE_REL_MAP");

            entity.Property(e => e.BenfTypeId).HasComment("5=Family Pension,4=Death Gratuity");
            entity.Property(e => e.IntOtherMasterId).HasComment("master_type  = 'RL'");
        });

        modelBuilder.Entity<PMmPenHoaMap>(entity =>
        {
            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("'Y'::bpchar");
            entity.Property(e => e.CreatedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.ModifiedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<PMmPenPrepAttachement>(entity =>
        {
            entity.HasKey(e => e.IntPenPrepAttachement).HasName("PK_P_MM_PEN_PREP_ATTACH");

            entity.Property(e => e.CreatedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.ModifiedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<PMmPenPrepCalcHeader>(entity =>
        {
            entity.Property(e => e.CommutationSanctionedFlag).HasComment("'Y'/'N' flag");
            entity.Property(e => e.CreatedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.GratuityAgreedPerAmtFlag).HasComment("'P' for percentage 'A' for Amount");
            entity.Property(e => e.ImmediateReliefAmount).HasComment("If Immidiate Relief Flag is Yes then Immidiate Relief Amount to be filled");
            entity.Property(e => e.ImmediateReliefFlag).HasComment("'Y'/'N' flag");
            entity.Property(e => e.IntPenProvGratRuleDtlsId).HasComment("Rule used for calculation of normal provisional Gratuity");
            entity.Property(e => e.IntPenProvRuleDtlsId).HasComment("Rule used for calculation of normal provisional pension");
            entity.Property(e => e.LastDaRate).HasComment("Percentage at which DA calculated");
            entity.Property(e => e.MemoDate).HasComment("memo date to be given before approving calculation");
            entity.Property(e => e.MemoNo).HasComment("memo no  to be given before approving calculation");
            entity.Property(e => e.ModifiedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.PensionAgreedPerAmtFlag).HasComment("'P' for percentage 'A' for Amount");
            entity.Property(e => e.PensionCalculated).HasComment("data will be storred for Supperannuation system_calc_Basic Pension_amt or User_updated_Basic Pension_amt");
            entity.Property(e => e.ProvGratIncludeExcludeFlag).HasComment("'Y' = include 'N' = Exclude");
            entity.Property(e => e.ProvPenIncludeExcludeFlag).HasComment("'Y' = include 'N' = Exclude");
            entity.Property(e => e.SatisfiedFlag).HasComment("'Y'/'N' flag");
        });

        modelBuilder.Entity<PMmPenPrepDeclaration>(entity =>
        {
            entity.Property(e => e.CreatedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.ModifiedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.PenTypeId).HasComment("Unique pension ID type.");
            entity.Property(e => e.RetirementType).HasComment("P for pension, C for CVP application etc");
        });

        modelBuilder.Entity<PMmPenPrepPayRecoType>(entity =>
        {
            entity.Property(e => e.PayRecoTypeId).HasComment("Recovery type id..");
            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("'Y'::bpchar");
            entity.Property(e => e.CreatedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.ModifiedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.PayRecoAbbr)
                .IsFixedLength()
                .HasComment("Single word abbreviation for recovery type description..");
            entity.Property(e => e.PayRecoDesc).HasComment("Recovery type description..");
            entity.Property(e => e.PayRecoFlag).HasComment("R for recovery , P --- payment");
            entity.Property(e => e.PaymentOrderFlag).HasDefaultValueSql("'P'::bpchar");
        });

        modelBuilder.Entity<PMmPenPrepPensionerDetl>(entity =>
        {
            entity.HasKey(e => e.IntPensionerId).HasName("PK_P_MM_PEN_PREP_PEN_DETL");

            entity.ToTable("P_MM_PEN_PREP_PENSIONER_DETL", "cts_pension", tb => tb.HasComment("Store Pensioner Personal information"));

            entity.Property(e => e.IntPensionerId).HasComment("pensioner id which is unique");
            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("'Y'::bpchar");
            entity.Property(e => e.ApprovalAuthDesignation).HasComment("as per CR 208818 Point 1");
            entity.Property(e => e.ApproveTimeStamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.BankAcNo).HasComment("bank account id..");
            entity.Property(e => e.BenfTypeId).HasComment("no use of this");
            entity.Property(e => e.CaseRelOtherReason).HasComment("For Judicial Proceedings Only");
            entity.Property(e => e.CommutedValuePayOrderNo).HasComment("as per CR 181305 Point 2");
            entity.Property(e => e.CourtCaseNo).HasComment("For Judicial Proceedings Only");
            entity.Property(e => e.CourtCasePendingStatus).HasComment("Y/N");
            entity.Property(e => e.CourtCaseRemarks).HasComment("For Departmental Proceedings & 'Judicial' Proceedings.");
            entity.Property(e => e.CourtCaseType).HasComment("For Judicial Proceedings Only");
            entity.Property(e => e.CourtCaseYear).HasComment("For Judicial Proceedings Only");
            entity.Property(e => e.CpfShareRemarks).HasComment("19");
            entity.Property(e => e.CreatedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.CvpOptedFlag).HasDefaultValueSql("'Y'::character varying");
            entity.Property(e => e.DeclrnCheckFlag).HasDefaultValueSql("'N'::bpchar");
            entity.Property(e => e.FinalGpfAppliedFlag)
                .HasDefaultValueSql("'N'::bpchar")
                .HasComment("Y - Yes, N - No");
            entity.Property(e => e.GratuityPaymentOrderNo).HasComment("as per CR 181305 Point 2");
            entity.Property(e => e.InstituteIdObs).HasComment("unique id of the institute..");
            entity.Property(e => e.IntDistrictIdObs).HasComment("District id of pensioner");
            entity.Property(e => e.IntGpfNoIdObs).HasComment("gpf-tpf number id referencing column from PF_MM_GEN_SUBSCR_DETL");
            entity.Property(e => e.IntHeadOfOfficeIdSb).HasComment("SERVICE BOOK HOO");
            entity.Property(e => e.IntMaritalStatusId).HasComment("referencing from PF_MM_GEN_MARITAL_STATUS table indication maritial states.");
            entity.Property(e => e.IntOmiCaseRelationTo).HasComment("For Judicial Proceedings Only");
            entity.Property(e => e.IntOmiCaseStatus).HasComment("For Departmental Proceedings & 'Judicial' Proceedings.");
            entity.Property(e => e.IntOmiCourtCaseType).HasComment("Case Type: 'Judicial Proceedings', �Departmental Proceedings� Taken from OtherMaster Master Type' CCT'");
            entity.Property(e => e.IntPsaDdoId).HasComment("Tagged DDO of Service Book HOO");
            entity.Property(e => e.IntPsaTreasuryCode).HasComment("treasury attached with DDO of Service Book HOO");
            entity.Property(e => e.IntSpouseReligionId).HasComment("as per CR 275470");
            entity.Property(e => e.LastPostIdObs).HasComment("Last post dat the pensioner hold referencing from P_MM_PEN_PREP_POST");
            entity.Property(e => e.ModifiedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.NameOfCourt).HasComment("For Judicial Proceedings Only");
            entity.Property(e => e.NameOfTheAuthority).HasComment("For Departmental Proceedings Only");
            entity.Property(e => e.OtherPensionReceiptFlag).HasComment("22/f");
            entity.Property(e => e.OtherPpoDetails).HasComment("If family pensioner received any other pension or family pension then that PPO Details to be provided");
            entity.Property(e => e.OtherPpoNo).HasComment("If family pensioner received any other pension or family pension then that PPO Number to be provided");
            entity.Property(e => e.PayableOutTreasuryName).HasComment("For PAYABLE_TREASURY_FLAG 'I' Traeasury name 'O' State name");
            entity.Property(e => e.PayableTreasuryFlag).HasComment("B: Bank, I-Treasury, O: outside WB(State ) ");
            entity.Property(e => e.PenFileId).HasComment("unique for every pensioner");
            entity.Property(e => e.PenTypeId).HasComment("Unique pension ID type.");
            entity.Property(e => e.PensionCalcType).HasComment("Types are commomn Superannuation & 'Death' 'SP' & 'DP'");
            entity.Property(e => e.PensionTypeFlag).HasComment("'F' for Final 'P' for Provisional");
            entity.Property(e => e.PreviousPensionSource).HasComment("M - Military, C - Civil, O - Others");
            entity.Property(e => e.PreviousPensionType).HasComment("N for Normal, F for Family");
            entity.Property(e => e.ProcessingStageDtl).HasComment("For Detail Of Processing Status");
            entity.Property(e => e.ProvGratSystemRcvdFlag)
                .HasDefaultValueSql("'N'::character varying")
                .HasComment("If Provisional Gratuity is system calculated then 'Y' Else 'N'");
            entity.Property(e => e.ProvPenSystemRcvdFlag)
                .HasDefaultValueSql("'N'::character varying")
                .HasComment("If Provisional Pension is system calculated then 'Y' Else 'N'");
            entity.Property(e => e.ProvPensionPeriod).HasComment("as per CR 181305 Point 1");
            entity.Property(e => e.ProvisionalGratuityRcvdFlag).HasComment("35/b");
            entity.Property(e => e.ProvisionalPenReceivedFlag).HasComment("35/a");
            entity.Property(e => e.PsaDdoAvlFlag)
                .HasDefaultValueSql("'N'::character varying")
                .HasComment("SERVICE BOOK HOO tagged with DDO or Not");
            entity.Property(e => e.PsaTreasuryModifiableFlag).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.RateOfProvGratuity).HasComment("as per CR 181305 Point 1");
            entity.Property(e => e.RateOfProvPension).HasComment("as per CR 181305 Point 1");
            entity.Property(e => e.ReEmployedAfterRetireFlag).HasComment("Y/N");
            entity.Property(e => e.Remarks).HasComment("Remarks For General Purpose");
            entity.Property(e => e.RetirementDate).HasComment("to be fetched from employee master table -- added by ritu");
            entity.Property(e => e.RetirementForeAftNoon)
                .HasDefaultValueSql("1")
                .HasComment("0 for forenoon and 1 for afternoon");
            entity.Property(e => e.SanctioningAuthorityType).HasComment("AA --------- Appointing Auth, HOO----------- Head Of Office");
            entity.Property(e => e.TreasuryBank).HasComment("as per CR 480156");
            entity.Property(e => e.TreasuryName).HasComment("as per CR 480156");
            entity.Property(e => e.WritPetitionDetails).HasComment("If court case pending is Yes Then W.P(Writ Petition) Details to be provided");
            entity.Property(e => e.WritPetitionNo).HasComment("If court case pending is Yes Then W.P(Writ Petition) No to be provided");
        });

        modelBuilder.Entity<PMmPenProcessingFlag>(entity =>
        {
            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("'Y'::character varying");
            entity.Property(e => e.CreatedTimestamp).HasDefaultValueSql("CURRENT_DATE");
            entity.Property(e => e.ModifiedTimestamp).HasDefaultValueSql("CURRENT_DATE");
        });

        modelBuilder.Entity<PMmPenRetirementBenfType>(entity =>
        {
            entity.HasKey(e => e.BenfTypeId).HasName("PK_P_MM_PEN_RETIRE_BENF_TYPE");

            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("'Y'::bpchar");
            entity.Property(e => e.CreatedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.ModifiedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.NomineeFlag).HasComment("For selecting nominee, value should be 'Y'-- added by ritu");
        });

        modelBuilder.Entity<PMmPenRuleDtl>(entity =>
        {
            entity.Property(e => e.RopaAbbr).HasComment("pay_allowance_abbr");
        });

        modelBuilder.Entity<PMmPenRuleName>(entity =>
        {
            entity.Property(e => e.CreatedTimeStamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.ModifiedTimeStamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<PMmPenRuleSubDtl>(entity =>
        {
            entity.HasKey(e => e.IntPenRuleSubDtlsId).HasName("UK_P_MM_PEN_RULE_SUB_DTLS");
        });

        modelBuilder.Entity<PTdPenPrepAttachementDtl>(entity =>
        {
            entity.HasKey(e => e.IntAttachementDtlId).HasName("PK_P_TD_PEN_PREP_ATTACH_DTL");

            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("'Y'::character varying");
            entity.Property(e => e.CreatedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.ModifiedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.ModifiedUserId).HasDefaultValueSql("0");
        });

        modelBuilder.Entity<PTdPenPrepAttachmentDtl>(entity =>
        {
            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("'Y'::character varying");
            entity.Property(e => e.CreatedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IntAttachementDtlId).ValueGeneratedOnAdd();
            entity.Property(e => e.IntOmiUpdDocType).HasComment("document type from other master master type: 'AFT'");
            entity.Property(e => e.ModifiedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.ModifiedUserId).HasDefaultValueSql("0");
        });

        modelBuilder.Entity<PTdPenPrepFamilyAddress>(entity =>
        {
            entity.HasKey(e => e.IntFamilyAddrId).HasName("PK_P_TD_PEN_PREP_FAMILY_ADDR");

            entity.Property(e => e.ActiveFlag).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.CreatedTimestamp).HasDefaultValueSql("CURRENT_DATE");
            entity.Property(e => e.ModifiedTimestamp).HasDefaultValueSql("CURRENT_DATE");
            entity.Property(e => e.SameAsPermanentAddr).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.Wef).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<PTdPenPrepFamilyDtl>(entity =>
        {
            entity.Property(e => e.FamilyId).HasComment("unique family id..");
            entity.Property(e => e.BankAcNoObs).HasComment("bank account id..");
            entity.Property(e => e.CreatedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.DeceasedFlag)
                .HasDefaultValueSql("'N'::character varying")
                .HasComment("'Y' for alive 'N' for dead");
            entity.Property(e => e.Dob).HasComment("date of birth of pensioner..");
            entity.Property(e => e.EfpUpto).HasComment("EFP up to date..");
            entity.Property(e => e.FamilyPensionFlag)
                .HasDefaultValueSql("'N'::character varying")
                .HasComment("'Y' for family pension admisible else 'N'");
            entity.Property(e => e.GenderObs).HasComment("gender of the pensioner..");
            entity.Property(e => e.HandicappFlagObs).HasComment("Whether this member is handicapped..");
            entity.Property(e => e.HandicappTypeObs).HasComment("'P' for Physically Handicapped and 'M' for Mentally Handicapped");
            entity.Property(e => e.IntMaritalStatusId).HasComment("mariatial status of the relationship of pensionar");
            entity.Property(e => e.IntPensionerId).HasComment("pensioner id which is unique");
            entity.Property(e => e.IntRelIdObs).HasComment("relationshp of pensioner with this member..");
            entity.Property(e => e.MarriageDate).HasComment("Added for Requirement of Exit Management");
            entity.Property(e => e.MinorFlagCalculatedOnObs).HasDefaultValueSql("CURRENT_DATE");
            entity.Property(e => e.MinorFlagObs).HasComment("whether this member is minor..");
            entity.Property(e => e.ModifiedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.PenFileIdObs).HasComment("unique pension id for the pensioner..");
            entity.Property(e => e.RelFirstName).HasComment("name of the family member");
            entity.Property(e => e.RelIntOmiPhysicChallenged).HasComment("PHC in Otehr Master");
            entity.Property(e => e.RelIntOmiRelationship).HasComment("RL type in other master");
            entity.Property(e => e.SharePercentageObs).HasComment("Share percentage of the family member..");
        });

        modelBuilder.Entity<PTdPenPrepFamilyPenDtl>(entity =>
        {
            entity.HasKey(e => e.IntPrepFamilyPenDtl).HasName("PK_P_TD_PEN_PREP_FAM_PEN_DTL");

            entity.Property(e => e.CreatedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.ModifiedTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<PTdPenPrepFileProcessLog>(entity =>
        {
            entity.HasKey(e => e.PenFileProcessLogId).HasName("PK_P_TD_PEN_PREP_FILE_PROC_LOG");

            entity.Property(e => e.ProcessDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
