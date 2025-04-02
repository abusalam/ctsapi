using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using CTS_BE.DTOs.Validators;
using CTS_BE.Filters;
using CTS_BE.PensionEnum;

namespace CTS_BE.DTOs
{
    public class BaseDTO
    {
        [SwaggerExclude]
        public ExpandoObject? DataSource { get; set; }
    }

    public class DateOnlyDTO
    {
        [DataType(DataType.Date)]
        public DateOnly DateOnly { get; set; }
    }

    public class PensionStatusDTO : BaseDTO
    {
        [Required]
        [EnumDataType(typeof(PensionStatusFlag))]
        public PensionStatusFlag StatusFlag { get; set; }

        // public int StatusFlag {get; set; }
        public PensionStatusReassonFlag ReasonFlag { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly StatusWef { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? StatusUpto { get; set; }
        public string? ReasonRemark { get; set; }
    }

    public class PensionStatusEntryDTO : PensionStatusDTO
    {
        [Required]
        public int PpoId { get; set; }
    }

    public class ManualPpoReceiptEntryDTO : BaseDTO
    {
        [Required]
        [StringLength(100)]
        public required string PpoNo { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public required string PensionerName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [PastDateWithinYears(100)]
        public required DateOnly DateOfCommencement { get; set; }

        [StringLength(10)]
        [RegularExpression(@"^[6-9]\d{9}$")]
        public string? MobileNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [PastDateWithinYears(10)]
        public required DateOnly ReceiptDate { get; set; }

        [Required]
        [RegularExpression(@"[ADO]", ErrorMessage = "{0} must be one of the following (A, D & O)")]
        public required char PsaCode { get; set; }

        [Required]
        [RegularExpression(
            @"[NRPO]",
            ErrorMessage = "{0} must be one of the following (N, R, P & O)"
        )]
        public required char PpoType { get; set; }
    }

    public class ManualPpoReceiptResponseDTO : ManualPpoReceiptEntryDTO
    {
        public long Id { get; set; }

        [StringLength(13)]
        public string TreasuryReceiptNo { get; set; } = null!;
    }

    public class ListAllPpoReceiptsResponseDTO
    {
        public long Id { get; set; }

        [StringLength(13)]
        public string TreasuryReceiptNo { get; set; } = null!;

        [StringLength(100)]
        public required string PpoNo { get; set; } = null!;

        [StringLength(100)]
        public required string PensionerName { get; set; }

        [DataType(DataType.Date)]
        public required DateOnly ReceiptDate { get; set; }

        [DataType(DataType.Date)]
        public required DateOnly DateOfCommencement { get; set; }
    }

    public partial class FileEntryDTO : BaseDTO
    {
        public string FileName { get; set; } = null!;
        public byte[] Contents { get; set; } = null!;
    }

    public partial class FileResponseDTO : FileEntryDTO
    {
        public long Id { get; set; }
        public string FilePath { get; set; } = null!;
        public string FileMimeType { get; set; } = null!;
    }

    public partial class EPpoReceiptEntryDTO : BaseDTO
    {
        [Required]
        public string PpoNo { get; set; } = null!;

        [Required]
        public string PensionApplnNo { get; set; } = null!;

        [Required]
        public string TreasuryCode { get; set; } = null!;

        [Required]
        public char FreshRevisionFlag { get; set; }

        [Required]
        public char PpoTypeCode { get; set; }

        [Required]
        public string? IssuingLetterNo { get; set; }

        [Required]
        public DateOnly? IssuingLetterDate { get; set; }

        [Required]
        public int PenCatId { get; set; }

        [Required]
        public string SanctionAuthority { get; set; } = null!;

        [Required]
        public string SanctionNo { get; set; } = null!;

        [Required]
        public DateOnly SanctionDate { get; set; }

        [Required]
        public char ProvisionalPensionStatus { get; set; }

        [Required]
        public string PensionerName { get; set; } = null!;

        [Required]
        public char Religion { get; set; }
        public string? PensionerAddress { get; set; }
        public string? MobileNumber { get; set; }
        public string? AadhaarNo { get; set; }

        [Required]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        public DateOnly DateOfRetirement { get; set; }
        public DateOnly? DateOfDeath { get; set; }
        public int? QualifyingServiceGrossYears { get; set; }
        public int? QualifyingServiceGrossMonths { get; set; }
        public int? QualifyingServiceGrossDays { get; set; }
        public int? EmployeeLastPay { get; set; }
        public int? EmployeeLastPayNotional { get; set; }
        public int CommutedPensionAmount { get; set; }
        public List<EPpoAmountEntryDTO> EppoAmounts { get; set; } = [];
        public List<EPpoNomineeEntryDTO> EppoNominees { get; set; } = [];
        public FileEntryDTO? PhotoFile { get; set; }
        public FileEntryDTO? SignatureFile { get; set; }
        public FileEntryDTO? EPpoFile { get; set; }
    }

    public partial class EPpoReceiptDetailDTO : EPpoReceiptEntryDTO
    {
        public long Id { get; set; }
    }

    public partial class EPpoReceiptListDTO : BaseDTO
    {
        public int EPpoReceiptCount
        {
            get { return EPpoReceipts?.Count ?? 0; }
        }
        public List<EPpoReceiptDetailDTO>? EPpoReceipts { get; set; }
    }

    public partial class EPpoReceiptResponseDTO : BaseDTO
    {
        public long Id { get; set; }
        public string PensionApplnNo { get; set; } = null!;
    }

    public partial class EPpoReceiptRevisionEntryDTO : BaseDTO
    {
        [Required]
        public string PpoNo { get; set; } = null!;

        [Required]
        public string PensionApplnNo { get; set; } = null!;

        [Required]
        public string TreasuryCode { get; set; } = null!;

        public int? PpoId { get; set; }

        public string? IssuingLetterNo { get; set; }

        public DateOnly? IssuingLetterDate { get; set; }

        [Required]
        public char FreshRevisionFlag { get; set; }

        [Required]
        public char PpoTypeCode { get; set; }

        [Required]
        public char PpoSubType { get; set; }

        [Required]
        public int PenCatId { get; set; }

        public int? EmployeeLastPay { get; set; }

        public int? EmployeeLastPayNotional { get; set; }

        [Required]
        public int CommutedPensionAmount { get; set; }

        public long? EppoFileId { get; set; }

        public FileEntryDTO? EPpoFile { get; set; }
    }

    public partial class EPpoReceiptRevisionResponseDTO : BaseDTO
    {
        public long Id { get; set; }
        public string PensionApplnNo { get; set; } = null!;
    }

    public partial class EPpoReceiptWithdrawlEntryDTO : BaseDTO
    {
        [Required]
        public string PensionApplnNo { get; set; } = null!;

        [Required]
        public char FreshRevisionFlag { get; set; }

        [Required]
        public string? WithdrawReason { get; set; }
    }

    public partial class EPpoReceiptWithdrawlResponseDTO : BaseDTO
    {
        public long Id { get; set; }
        public string PensionApplnNo { get; set; } = null!;
        public string PensionerName { get; set; } = null!;
        public char PpoTypeCode { get; set; }
        public int? PpoId { get; set; }
        public string TreasuryCode { get; set; } = null!;
    }

    public partial class EPpoReceiptPpoIdResponseDTO : BaseDTO
    {
        public long Id { get; set; }
        public string PensionApplnNo { get; set; } = null!;
        public string PpoNo { get; set; } = null!;
        public string PensionerName { get; set; } = null!;
        public char PpoTypeCode { get; set; }
        public int? PpoId { get; set; }
        public string TreasuryCode { get; set; } = null!;
    }

    public class EPpoAmountEntryDTO : BaseDTO
    {
        [Required]
        [StringLength(3)]
        [RegularExpression(
            @"^(CLS|EFP|BSC|NFP|BYT)$",
            ErrorMessage = "Amount type must be one of: CLS (Classification), EFP (Enhanced Family Pension), BSC (Basic Pension), NFP (Normal Family Pension), BYT (By Transfer)"
        )]
        public string AmountType { get; set; } = null!;
        public long? ClassificationId { get; set; }
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
        public int Amount { get; set; }
        public bool Consolidated { get; set; }
        public long? CategoryId { get; set; }
    }

    public class EPpoNomineeEntryDTO : BaseDTO
    {
        [Required]
        [RegularExpression(
            @"^[PFD]$",
            ErrorMessage = "Nominee type must be: P (Pensioner), F (Family), or D (Dependent)"
        )]
        public char NomineeType { get; set; }

        [Required]
        [Range(1, 20)]
        public int SerialNo { get; set; }

        [Required]
        [StringLength(100)]
        public string NomineeName { get; set; } = null!;

        [Required]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        [RegularExpression(
            @"^[WEHSDOMRNAFKYCUITJBPVL]$",
            ErrorMessage = "Relation must be one of: [WEHSDOMRNAFKYCUITJBPVL] E - Employed; L - Widow Daughter; U - Unmarried Daughter; V - Divorced Daughter; N - Minor Son; R - Minor Daughter; P - Handicapped Son; G - Handicapped Daughter; J - Dependent Father; K - Dependent Mother; H - Husband; W - Wife;"
        )]
        public char Relation { get; set; }

        [Range(0, 100)]
        public int? NomineeShare { get; set; }

        [Required]
        [RegularExpression(@"^[AM]$", ErrorMessage = "Must be one of: A (Adult) or M (Minor)")]
        public char? NomineeAdultMinor { get; set; }
    }

    public class PensionerEntryDTO : BaseDTO
    {
        [Required]
        [StringLength(100)]
        public string PpoNo { get; set; } = null!;

        [Required]
        [RegularExpression(@"[PFC]", ErrorMessage = "{0} must be one of the following (P, F & C)")]
        /// <value>Property <c>PpoType</c> Must be one of the following (P, F, C).</value>
        public char PpoType { get; set; }

        [Required]
        [RegularExpression(
            @"[ELUVNRPGJKHW]",
            ErrorMessage = "{0} must be one of the following (E, L, U, V, N, R, P, G, J, K, H & W)"
        )]
        /// <value>Property <c>PpoSubType</c> Must be one of the following (E, L, U, V, N, R, P, G, J, K, H, W).</value>
        public char PpoSubType { get; set; }

        [Required]
        public long CategoryId { get; set; }
        public virtual long BankId { get; set; }

        [Required]
        public long BranchId { get; set; }

        [Required]
        [StringLength(100)]
        public string AccountHolderName { get; set; } = null!;

        [Required]
        [RegularExpression(@"[QB]", ErrorMessage = "{0} must be one of the following (Q, B)")]
        public char PayMode { get; set; }

        [Required]
        [StringLength(30)]
        public string BankAcNo { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string PensionerName { get; set; } = null!;

        [RegularExpression(
            @"[MF]",
            ErrorMessage = "{0} must be one of the following (M - Male; F - Female;)"
        )]
        /// <value>Property <c>Gender</c> Must be one of the following (M - Male; F - Female;).</value>
        public char? Gender { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [PastDateWithinYears(100)]
        public DateOnly DateOfBirth { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? DateOfDeath { get; set; }

        [StringLength(10)]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Invalid Mobile Number")]
        public string? MobileNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        public string? EmailId { get; set; }
        public string? PensionerAddress { get; set; }
        public string? IdentificationMark { get; set; }

        [RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]{1}$", ErrorMessage = "Invalid PAN Number")]
        public string? PanNo { get; set; }

        [StringLength(12)]
        public string? AadhaarNo { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [PastDateWithinYears(100)]
        public DateOnly DateOfRetirement { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [PastDateWithinYears(100)]
        public DateOnly DateOfCommencement { get; set; }

        [Required]
        public int BasicPensionAmount { get; set; }

        [Required]
        public int? CommutedPensionAmount { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? CommutedFromDate { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? CommutedUptoDate { get; set; }

        [Required]
        public int EnhancePensionAmount { get; set; }

        public int? EfpAmount { get; set; }
        public DateOnly? EfpWefDate { get; set; }
        public DateOnly? EfpUptoDate { get; set; }
        public int? NfpAmount { get; set; }
        public DateOnly? NfpWefDate { get; set; }
        public int? NotionalPensionAmount { get; set; }
        public DateOnly? NotionalWefDate { get; set; }

        [StringLength(100)]
        public string? GpfTpfNo { get; set; }
        public bool? HealthScheme { get; set; }
        public bool? EmployedPensioner { get; set; }
        public bool? ReEmployedPensioner { get; set; }
        public bool? DoublePension { get; set; }
        public bool? AdhocPension { get; set; }
        public bool? ProvisionalPension { get; set; }
        public bool? InterimAllowance { get; set; }
        public bool? SharedPension { get; set; }

        [StringLength(500)]
        public string? Remarks { get; set; }

        [Required]
        public int ReducedPensionAmount { get; set; }

        /// <summary>
        /// Must be one of the following (H, M, O)
        /// </summary>
        [Required]
        [RegularExpression(@"[HMO]", ErrorMessage = "{0} must be one of the following (H, M & O)")]
        public char Religion { get; set; }
    }

    public class BankListResponseDTO : BaseDTO
    {
        public int BankCount
        {
            get { return Banks.Count; }
        }
        public List<BankResponseDTO> Banks { get; set; } = null!;
    }

    public class BranchListItemResponseDTO : BaseDTO
    {
        public long Id { get; set; }
        public string BranchName { get; set; } = null!;
        public string IfscCode { get; set; } = null!;
    }

    public class BranchListResponseDTO : BaseDTO
    {
        public int BranchCount
        {
            get { return Branches.Count; }
        }
        public BankResponseDTO? Bank { get; set; }
        public List<BranchListItemResponseDTO> Branches { get; set; } = null!;
    }

    public class BankResponseDTO : BaseDTO
    {
        public long Id { get; set; }
        public string BankName { get; set; } = null!;
    }

    public class BankBranchNameResponseDTO : BaseDTO
    {
        public long BankId { get; set; }
        public long BranchId { get; set; }
        public string BankBranchName { get; set; } = null!;
    }

    public class BranchResponseDTO : BaseDTO
    {
        public long Id { get; set; }
        public long BankId { get; set; }
        public BankResponseDTO? Bank { get; set; }
        public string BranchName { get; set; } = null!;
        public string BranchAddress { get; set; } = null!;
        public string IfscCode { get; set; } = null!;
        public string CityName { get; set; } = null!;
        public string DistrictName { get; set; } = null!;
        public string StateName { get; set; } = null!;
        public string PhoneNo { get; set; } = null!;
    }

    public class PensionerResponseDTO : PensionerEntryDTO
    {
        public long Id { get; set; }
        public int PpoId { get; set; }
        public PensionCategoryResponseDTO? Category { get; set; }
        public ManualPpoReceiptResponseDTO? Receipt { get; set; }
        public BranchResponseDTO? Branch { get; set; }
        public List<PpoSanctionDetailsResponseDTO>? PpoSanctionDetails { get; set; }
        public override long BankId =>
            Branch != null
                ? Branch.Bank != null
                    ? Branch.Bank.Id
                    : 0
                : 0;
        public string? PensionerStatus { get; set; }
        public bool? FirstPensionGenerated { get; set; }
    }

    public class PensionerListItemDTO : BaseDTO
    {
        public long Id { get; set; }

        public int PpoId { get; set; }

        [StringLength(100)]
        public string PensionerName { get; set; } = null!;

        [StringLength(10)]
        [RegularExpression(@"^[6-9]\d{9}$")]
        public string? MobileNumber { get; set; }

        [DataType(DataType.Date)]
        [PastDateWithinYears(100)]
        public DateOnly DateOfBirth { get; set; }

        [DataType(DataType.Date)]
        [PastDateWithinYears(100)]
        public DateOnly DateOfRetirement { get; set; }

        [DataType(DataType.Date)]
        [PastDateWithinYears(100)]
        public DateOnly DateOfCommencement { get; set; }

        [StringLength(100)]
        public string PpoNo { get; set; } = null!;
    }

    public class AccountHeadResponseDTO : BaseDTO
    {
        public long Id { get; set; }
        public string? MajorHead { get; set; }
        public string? SubmajorHead { get; set; }
        public string? MinorHead { get; set; }
        public string? PlanStatus { get; set; }
        public string? SchemeHead { get; set; }
        public string? DetailHead { get; set; }
        public string? SubdetailHead { get; set; }
        public char? VotedCharged { get; set; }
    }

    public class AccountHeadListItemResponseDTO : BaseDTO
    {
        public long Id { get; set; }
        public string? HeadDetails { get; set; }
        public string? HeadDescription { get; set; }
    }

    public partial class PensionPrimaryCategoryEntryDTO : BaseDTO
    {
        [Required]
        public long AccountHeadId { get; set; }

        [Required]
        [StringLength(100)]
        public string PrimaryCategoryName { get; set; } = null!;
    }

    public partial class PensionPrimaryCategoryResponseDTO : PensionPrimaryCategoryEntryDTO
    {
        public long Id { get; set; }
        public string? HeadDetails
        {
            get =>
                $"{AccountHead?.MajorHead}-{AccountHead?.SubmajorHead}-{AccountHead?.MinorHead}-{AccountHead?.PlanStatus}-{AccountHead?.SchemeHead}-{AccountHead?.VotedCharged}-{AccountHead?.DetailHead}-{AccountHead?.SubdetailHead}";
        }
        public AccountHeadResponseDTO? AccountHead { get; set; }
    }

    public partial class PensionSubCategoryEntryDTO : BaseDTO
    {
        [Required]
        [StringLength(100)]
        public string SubCategoryName { get; set; } = null!;
    }

    public partial class PensionSubCategoryResponseDTO : PensionSubCategoryEntryDTO
    {
        public long Id { get; set; }
    }

    public partial class PensionCategoryEntryDTO : BaseDTO
    {
        [Required]
        public long PrimaryCategoryId { get; set; }

        [Required]
        public long SubCategoryId { get; set; }
    }

    public partial class PensionCategoryResponseDTO : PensionCategoryEntryDTO
    {
        public long Id { get; set; }
        public string CategoryName { get; set; } = null!;
        public PensionPrimaryCategoryResponseDTO PrimaryCategory { get; set; } = null!;
        public PensionSubCategoryResponseDTO SubCategory { get; set; } = null!;
        public List<ComponentRateResponseDTO>? ComponentRates { get; set; }
    }

    public class PensionCategoryListDTO : BaseDTO
    {
        public long Id { get; set; }
        public long PrimaryCategoryId { get; set; }
        public long SubCategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
    }

    public partial class PensionBreakupEntryDTO : BaseDTO
    {
        [Required]
        [StringLength(100)]
        public string ComponentName { get; set; } = null!;

        /// <summary>
        /// P - Payment; D - Deduction;
        /// </summary>
        [Required]
        [RegularExpression(
            @"[PD]",
            ErrorMessage = "{0} must be one of the following (P - Payment; D - Deduction)"
        )]
        public char ComponentType { get; set; }

        /// <summary>
        /// Relief Allowed (true/false)
        /// </summary>
        [Required]
        public bool ReliefFlag { get; set; }
    }

    public partial class PensionBreakupResponseDTO : PensionBreakupEntryDTO
    {
        public long Id { get; set; }
    }

    public partial class ComponentRateEntryDTO : BaseDTO
    {
        [Required]
        public long CategoryId { get; set; }

        [Required]
        public long BreakupId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly EffectiveFromDate { get; set; }

        [Required]
        public int RateAmount { get; set; }

        /// <summary>
        /// P - Percentage; A - Amount;
        /// </summary>
        [Required]
        [RegularExpression(
            @"[PA]",
            ErrorMessage = "{0} must be one of the following (P - Percentage; A - Amount;)"
        )]
        public char RateType { get; set; }
    }

    public partial class ComponentRateResponseDTO : ComponentRateEntryDTO
    {
        public long Id { get; set; }
        public string ComponentName
        {
            get { return Breakup?.Id + "-" + Breakup?.ComponentName; }
        }
        public string ComponentRate
        {
            get
            {
                return RateType == BreakupRateType.Amount
                    ? "₹" + RateAmount
                    : "" + RateAmount + "%";
            }
        }
        public string ComponentType
        {
            get
            {
                return Breakup?.ComponentType == BreakupComponentType.Payment
                    ? "Payment"
                    : "Deduction";
            }
        }
        public string WithEffectFrom
        {
            get { return EffectiveFromDate.ToString("dd-MM-yyyy"); }
        }
        public PensionBreakupResponseDTO? Breakup { get; set; }
    }

    public partial class InitiateFirstPensionBillEntryDTO : BaseDTO
    {
        [Required]
        public virtual int PpoId { get; set; }

        [Required]
        // [CurrentOrFutureDateUptoYears(
        //     1,
        //     ErrorMessage = "Date of bill should be today or within next 1 year from today"
        // )]
        public virtual DateOnly ToDate { get; set; }
    }

    public partial class PensionerFirstBillResponseDTO : InitiateFirstPensionBillEntryDTO
    {
        public long Id { get; set; }

        // public override int PpoId { get {return this.Pensioner.PpoId;} }
        public DateOnly FromDate { get; set; }
        public char BillType { get; set; } = 'F';

        // public required PensionerBankAcResponseDTO BankAccount { get; set; }
        // public long BankAccountId { get {return this.BankAccount.Id;} }
        // public PensionCategoryResponseDTO PensionCategory { get; set; } = null!;
        public ICollection<PpoPaymentListItemDTO>? PensionerPayments { get; set; }
        public List<PpoBillBreakupResponseDTO>? PpoBillBreakups { get; set; }

        // public List<PpoComponentRevisionResponseDTO>? PpoComponentRevisions { get; set; }
        public DateOnly BillGeneratedUptoDate { get; set; }

        // public long BillId { get; set; }
        public override DateOnly ToDate
        {
            get { return this.BillGeneratedUptoDate; }
        }
        public DateOnly BillDate { get; set; }
        public string TreasuryVoucherNo { get; set; } = null!;
        public long TreasuryVoucherId { get; set; }
        public DateOnly TreasuryVoucherDate { get; set; }
        public long BranchId { get; set; }
        public long GrossAmount { get; set; }
        public long NetAmount { get; set; }
        public string PreparedBy { get; set; } = null!;
        public DateOnly PreparedOn { get; set; }
    }

    public partial class InitiateFirstPensionBillResponseDTO : PensionerFirstBillResponseDTO
    {
        public long PensionerId
        {
            get { return this.Pensioner?.Id ?? 0; }
        }
        public PensionerResponseDTO? Pensioner { get; set; } = null!;
        public string BankBranchName
        {
            get
            {
                return this.Pensioner?.Branch?.Bank?.BankName
                    + " - "
                    + this.Pensioner?.Branch?.BranchName;
            }
        }
    }

    public partial class PpoPaymentListItemDTO : BaseDTO
    {
        // public int PpoId { get; set; }
        // public long BillId { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public int BasicPensionAmount { get; set; }

        /// <summary>
        /// BaseAmount will be same as AmountPerMonth except in case of DA where BaseAmount will be BasicPensionAmount
        /// </summary>
        public int BaseAmount { get; set; }
        public long BreakupId { get; set; }
        public string ComponentName { get; set; } = null!;
        public char ComponentType { get; set; }
        public int AmountPerMonth { get; set; }
        public long RateId { get; set; }
        public char RateType { get; set; }
        public int RateAmount { get; set; }
        public int PeriodInMonths { get; set; }
        public int PeriodInDays { get; set; }
        public int DueAmount { get; set; }
        public int DrawnAmount { get; set; }
        public int NetAmount { get; set; }
    }

    public partial class PpoComponentRevisionEntryDTO : BaseDTO
    {
        [Required]
        public long RateId { get; set; }

        /// <summary>
        /// From date is the Date of Commencement of pension of the pensioner
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateOnly FromDate { get; set; }

        /// <summary>
        /// Amount per month is the actual amount paid for the mentioned period
        /// </summary>
        [Required]
        public int AmountPerMonth { get; set; }
    }

    public partial class PpoComponentRevisionResponseDTO : PpoComponentRevisionEntryDTO
    {
        public long Id { get; set; }

        /// <summary>
        /// To date (will be null for regular active bills)
        /// </summary>
        [DataType(DataType.Date)]
        public DateOnly? ToDate { get; set; }
        public string ComponentDescription
        {
            get { return Rate?.ComponentName ?? "--"; }
        }

        public ComponentRateResponseDTO? Rate { get; set; }
    }

    public partial class PpoComponentRevisionUpdateDTO : BaseDTO
    {
        /// <summary>
        /// From date is the Date of Commencement of pension of the pensioner
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateOnly FromDate { get; set; }

        /// <summary>
        /// Amount per month is the actual amount paid for the mentioned period
        /// </summary>
        [Required]
        public int AmountPerMonth { get; set; }
    }

    public partial class PpoBillEntryDTO : BaseDTO
    {
        [Required]
        public int PpoId { get; set; }

        [Required]
        [Range(1, 12, ErrorMessage = "Month should be between 1 and 12 without leading 0s")]
        public int Month { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [CurrentOrFutureDateUptoYears(
            1,
            ErrorMessage = "To date of bill should be current or future date upto 1 year from today"
        )]
        public DateOnly ToDate { get; set; }
    }

    public partial class PpoBillResponseDTO : BaseDTO
    {
        public long Id { get; set; }
        public long PensionerId { get; set; }
        public string BankBranchName { get; set; } = null!;
        public string TreasuryName { get; set; } = null!;

        [DataType(DataType.Date)]
        public DateOnly FromDate { get; set; }

        // {
        //     get { return Bill.FromDate; }
        // }
        public DateOnly ToDate { get; set; }

        // {
        //     get { return Bill.ToDate; }
        // }
        public char BillType { get; set; }
        public int BillNo { get; set; }

        // {
        //     get { return Bill.BillNo; }
        // }
        public DateOnly BillDate { get; set; }

        // {
        //     get { return Bill.BillDate; }
        // }
        public int GrossAmount { get; set; }
        public int ByTransferAmount { get; set; }
        public int NetAmount { get; set; }
        public string AmountInWords { get; set; } = null!;
        public virtual List<PpoBillBreakupEntryDTO> Breakups { get; set; } = null!;
        public long DrawnAmount { get; set; } = 0;
        public string? TreasuryVoucherNo { get; set; }

        // {
        //     get { return $"TV-{Bill.Id}-{Id}"; }
        // }
        public DateOnly? TreasuryVoucherDate { get; set; }

        // {
        //     get { return Bill.BillDate; }
        // }
        public PensionerResponseDTO Pensioner { get; set; } = null!;
        public List<PpoBillBreakupResponseDTO> PpoBillBreakups { get; set; } = null!;

        // public BillResponseDTO Bill { get; set; } = null!;
        public string PreparedBy { get; set; } = null!;
        public DateOnly PreparedOn { get; set; }
    }

    public partial class PpoBillBreakupEntryDTO : BaseDTO
    {
        // public long BillId { get; set; }
        [Required]
        public int PpoId { get; set; }

        // public long RateId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [PastDateWithinYears(
            100,
            ErrorMessage = "Date of bill should be within 100 years from today"
        )]
        public DateOnly FromDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly ToDate { get; set; }

        [Required]
        public int BreakupAmount { get; set; }
        public int DueAmount
        {
            get { return this.BreakupAmount; }
            set { this.BreakupAmount = value; }
        }
        public int DrawnAmount { get; set; } = 0;
        public int NetAmount
        {
            get { return this.BreakupAmount - this.DrawnAmount; }
        }
    }

    public partial class PpoBillBreakupResponseDTO : PpoBillBreakupEntryDTO
    {
        public long Id { get; set; }
        public long RevisionId
        {
            get { return this.Revision?.Id ?? 0; }
        }
        public PpoComponentRevisionResponseDTO Revision { get; set; } = null!;
        public string ComponentName { get; set; } = null!;
        public char ComponentType { get; set; }
        public int AmountPerMonth { get; set; }
        public int BaseAmount { get; set; }
    }

    public partial class PpoComponentRevisionListEntryDTO : BaseDTO
    {
        public List<PpoComponentRevisionEntryDTO>? Revisions { get; set; }
    }

    public partial class PpoListResponseDTO : BaseDTO
    {
        public List<PensionerListItemDTO> PpoList { get; set; } = null!;
        public int PpoCount
        {
            get { return this.PpoList?.Count ?? 0; }
        }
    }

    public partial class PpoBillListResponseDTO : BaseDTO
    {
        public long Id { get; set; }

        [StringLength(50)]
        public long AccountHeadId { get; set; }
        public int BillNo { get; set; }
        public DateOnly BillDate { get; set; }

        [DataType(DataType.Date)]
        public DateOnly FromDate { get; set; }

        [DataType(DataType.Date)]
        public DateOnly ToDate { get; set; }
        public long GrossAmount { get; set; }
        public long ByTransferAmount { get; set; }
        public long NetAmount { get; set; }
        public string? TreasuryVoucherNo { get; set; }
        public DateOnly? TreasuryVoucherDate { get; set; }
        public List<PpoBillResponseDTO> PpoBills { get; set; } = null!;
        public string PreparedBy { get; set; } = null!;
        public DateOnly PreparedOn { get; set; }
    }

    public partial class BillResponseDTO : BaseDTO
    {
        public long Id { get; set; }
        public string FinancialYear { get; set; } = null!;
        public long AccountHeadId { get; set; }
        public int BillNo { get; set; }
        public DateOnly BillDate { get; set; }
        public string TreasuryVoucherNo { get; set; } = null!;
        public DateOnly TreasuryVoucherDate { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public List<PpoBillResponseDTO> PpoBills { get; set; } = null!;
        public long PpoBillCount
        {
            get { return this.PpoBills?.Count ?? 0; }
        }
        public int GrossAmount { get; set; }
        public int ByTransferAmount { get; set; }
        public int NetAmount { get; set; }
        public string PreparedBy { get; set; } = null!;
        public DateOnly PreparedOn { get; set; }
    }

    public partial class BillListResponseDTO : BaseDTO
    {
        public List<BillResponseDTO> Bills { get; set; } = null!;
        public long BillCount
        {
            get { return this.Bills?.Count ?? 0; }
        }
        public string PreparedBy { get; set; } = null!;
        public DateOnly PreparedOn { get; set; }
    }

    public partial class PpoBillSaveResponseDTO : BaseDTO
    {
        public int PpoId { get; set; }
        public DateOnly BillDate { get; set; }
        public char BillType { get; set; }
    }

    public partial class RegularBillListResponseDTO : BaseDTO
    {
        public long RegularBillCount
        {
            get { return this.RegularBills?.Count ?? 0; }
        }
        public List<RegularBillResponseDTO>? RegularBills { get; set; }
    }

    public partial class RegularBillResponseDTO : BaseDTO
    {
        public long Id { get; set; }
        public string TreasuryName { get; set; } = null!;
        public string Month { get; set; } = null!;
        public string Year { get; set; } = null!;
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public string BankBranchName { get; set; } = null!;
        public string Category { get; set; } = null!;
        public long AccountHeadId { get; set; }
        public string? CategoryDescription { get; set; }
        public int BillNo { get; set; }
        public DateOnly BillDate { get; set; }
        public string TreasuryVoucherNo { get; set; } = null!;
        public DateOnly TreasuryVoucherDate { get; set; }
        public int GrossAmount { get; set; }
        public int NetAmount { get; set; }
        public int AccountHeadwiseAmount
        {
            get { return NetAmount; }
        }
        public int PayAmount
        {
            get { return NetAmount; }
        }
        public string AmountInWords { get; set; } = null!;
        public int ByTransferAmount { get; set; }
        public BranchResponseDTO? Branch { get; set; }
        public long PpoBillCount
        {
            get { return this.PpoBills?.Count ?? 0; }
        }
        public List<PpoRegularBillDetailsDTO> PpoBills { get; set; } = null!;
        public string PreparedBy { get; set; } = null!;
        public DateOnly PreparedOn { get; set; }
    }

    public partial class PpoRegularBillDetailsDTO : BaseDTO
    {
        public int PpoId { get; set; }
        public string PpoNo { get; set; } = null!;
        public string PensionerName { get; set; } = null!;
        public string BankAcNo { get; set; } = null!;
        public int TotalPayableAmount
        {
            get
            {
                return BasicPensionAmount
                    + DearnessReliefAmount
                    + MedicalReliefAmount
                    - CommutedPensionAmount
                    - OverdrawlAmount
                    + DpPensionAmount
                    + AdditionalPensionAmount
                    + ArrearPensionAmount
                    + InterimReliefAmount
                    - ByTransferAmount;
            }
        }
        public int BasicPensionAmount { get; set; }
        public int DearnessReliefAmount { get; set; }
        public int MedicalReliefAmount { get; set; }
        public int CommutedPensionAmount { get; set; }
        public int OverdrawlAmount { get; set; }
        public int DpPensionAmount { get; set; }
        public int AdditionalPensionAmount { get; set; }
        public int ArrearPensionAmount { get; set; }
        public int InterimReliefAmount { get; set; }
        public int ByTransferAmount { get; set; }
        public List<PpoBillBreakupResponseDTO> PpoBillBreakups { get; set; } = null!;
        public PensionerResponseDTO? Pensioner { get; set; }
    }

    public partial class TableResponseDTO<T> : BaseDTO
    {
        public List<TableHeader> Headers { get; set; } = null!;
        public List<T> Data { get; set; } = null!;
        public int DataCount
        {
            get { return this.Data.Count; }
        }
    }

    public partial class TableHeader
    {
        public string Name { get; set; } = null!;
        public string FieldName { get; set; } = null!;
    }

    public partial class PpoComponentRevisionPpoListItemDTO : BaseDTO
    {
        public int PpoId { get; set; }
        public string PpoNo { get; set; } = null!;
        public string PensionerName { get; set; } = null!;
        public string CategoryDescription { get; set; } = null!;
        public string BankBranchName { get; set; } = null!;
        public PensionCategoryResponseDTO? Category { get; set; }
        public BranchResponseDTO? Branch { get; set; }
    }

    public partial class PpoSanctionDetailsEntryDTO : BaseDTO
    {
        [Required]
        public int PpoId { get; set; }

        [Required]
        public long PensionerId { get; set; }

        [Required]
        public string EmployeeName { get; set; } = null!;

        [Required]
        public string SanctionAuthority { get; set; } = null!;

        [Required]
        public string SanctionNo { get; set; } = null!;

        [Required]
        public DateOnly SanctionDate { get; set; }
        public DateOnly? EmployeeDob { get; set; }
        public char? EmployeeGender { get; set; }
        public DateOnly? EmployeeDateOfAppointment { get; set; }
        public string? EmployeeOffice { get; set; }
        public string? EmployeeDesignation { get; set; }
        public int? EmployeeLastPay { get; set; }
        public int? AverageEmolument { get; set; }
        public string? EmployeeHrmsId { get; set; }
        public string? IssuingAuthority { get; set; }
        public string? IssuingLetterNo { get; set; }
        public DateOnly? IssuingLetterDate { get; set; }
        public int? QualifyingServiceGrossYears { get; set; }
        public int? QualifyingServiceGrossMonths { get; set; }
        public int? QualifyingServiceGrossDays { get; set; }
        public int? QualifyingServiceNetYears { get; set; }
        public int? QualifyingServiceNetMonths { get; set; }
        public int? QualifyingServiceNetDays { get; set; }
    }

    public partial class PpoSanctionDetailsResponseDTO : PpoSanctionDetailsEntryDTO
    {
        public long Id { get; set; }
        // public PensionerResponseDTO? Pensioner { get; set; }
    }

    public partial class NomineeEntryDTO : BaseDTO
    {
        [Required]
        public int PpoId { get; set; }

        [Required]
        public int SerialNo { get; set; }

        [Required]
        public string NomineeName { get; set; } = null!;

        [Required]
        [RegularExpression(
            @"[FMHWSDBTEIACO]",
            ErrorMessage = "{0} must be one of the following (F - Father; M - Mother; H - Husband; W - Wife; S - Son; D - Daughter; B - Brother; T - Sister; E - Self; I - Brother(Minor); A - Sister(Unmarried); C - Sister(Widowed); O - Other;)"
        )]
        public char Relation { get; set; }

        [Required]
        public DateOnly DateOfBirth { get; set; }
        public DateOnly? DateOfDeath { get; set; }

        [Required]
        [RegularExpression(
            @"[056]",
            ErrorMessage = "{0} must be one of the following (0 - Family; 5 - LTA; 6 - Death Gratuity;)"
        )]
        public char? NomineeType { get; set; }

        [Required]
        [RegularExpression(
            @"[AM]",
            ErrorMessage = "{0} must be one of the following (A - Adult; M - Minor;)"
        )]
        public char? NomineeAdultMinor { get; set; }

        [RegularExpression(
            @"[12345]",
            ErrorMessage = "{0} must be one of the following (1 - First; 2 - Second; 3 - Third; 4 - Fourth; 5 - Fifth;)"
        )]
        public int? NomineePriority { get; set; }
        public int? NomineeShare { get; set; }
        public bool? FamilyPension { get; set; }
        public bool? Refused { get; set; }
        public bool? NomineeActive { get; set; }
        public bool Handicapped { get; set; }
        public string? IdentificationMark { get; set; }
        public string? BankAcNo { get; set; }
        public virtual long BankId { get; set; }
        public long? BranchId { get; set; }
    }

    public partial class NomineeResponseDTO : NomineeEntryDTO
    {
        public long Id { get; set; }
        public BranchResponseDTO? Branch { get; set; }
        public override long BankId => Branch != null ? Branch.BankId : 0;
    }

    public partial class NomineeListResponseDTO : BaseDTO
    {
        public int NomineeCount
        {
            get { return Nominees?.Count ?? 0; }
        }
        public List<NomineeResponseDTO>? Nominees { get; set; }
    }

    public partial class LifeCertificateEntryDTO : BaseDTO
    {
        [Required]
        public int FinancialYear { get; set; }

        [Required]
        public int PpoId { get; set; }

        [Required]
        public bool? CertificateSubmitted { get; set; }
    }

    public partial class LifeCertificateResponseDTO : LifeCertificateEntryDTO
    {
        public long Id { get; set; }
    }

    public partial class LifeCertificateDetailsResponseDTO : BaseDTO
    {
        public long Id { get; set; }
        public int PpoId { get; set; }
        public string PpoNo { get; set; } = null!;
        public string PensionerName { get; set; } = null!;
        public string BankAcNo { get; set; } = null!;
        public string MobileNumber { get; set; } = null!;
        public bool DigitalMode { get; set; }
        public bool CertificateSubmitted { get; set; }
    }

    public partial class LifeCertificateListResponseDTO : BaseDTO
    {
        public int LifeCertificateCount
        {
            get { return LifeCertificates?.Count ?? 0; }
        }
        public List<LifeCertificateDetailsResponseDTO>? LifeCertificates { get; set; }
    }

    public partial class FinancialYearResponseDTO : BaseDTO
    {
        public short CurrentYear { get; set; }
    }

    //new added

    public partial class ByTransferHeadEntryDTO : BaseDTO
    {
        [Required]
        [RegularExpression(
            @"[PR]",
            ErrorMessage = "{0} must be one of the following (P - Payment; R - Recovery)"
        )]
        public char ByTransferType { get; set; }

        [Required]
        public long AccountHeadId { get; set; }

        [Required]
        [StringLength(500)]
        public string ByTransferDescription { get; set; } = null!;

        public bool AgBytransfer { get; set; }
    }

    public partial class ByTransferHeadResponseDTO : BaseDTO
    {
        public long Id { get; set; }

        public char ByTransferType { get; set; }

        public long AccountHeadId { get; set; }

        public string ByTransferDescription { get; set; } = null!;

        public bool AgBytransfer { get; set; }

        //public List<ByTransferHeadEntryDTO> ByTransferHeadList { get; set; } = new();

        //public int ByTransferHeadCount
        //{
        //    get { return this.ByTransferHeadList?.Count ?? 0; }
        //}
    }

    public partial class PpoByTransferEntryDTO : BaseDTO
    {
        public long PensionerId { get; set; }

        public int PpoId { get; set; }

        public DateOnly FromDate { get; set; }

        public DateOnly ToDate { get; set; }

        public long BytransferHeadId { get; set; }

        public int BytransferAmount { get; set; }

        public string? Remarks { get; set; }
    }

    public partial class PpoByTransferHeadResponseDTO : PpoByTransferEntryDTO
    {
        public long Id { get; set; }
    }

    public class PpoPaymentHistoryResponseDTO : BaseDTO
    {
        public long ComponentId { get; set; }
        public string ComponentName { get; set; } = null!;
        public int BreakupAmount { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
    }

    public class PpoArrearBillEntryDTO : BaseDTO
    {
        [Required]
        public int PpoId { get; set; }

        [Required]
        public DateOnly PeriodFrom { get; set; }

        [Required]
        public DateOnly PeriodTo { get; set; }
    }

    public class PpoArrearBillResponseDTO : BaseDTO
    {
        public long Id { get; set; }
        public int PpoId { get; set; }
        public string PpoNo { get; set; } = null!;
        public string PensionerName { get; set; } = null!;
        public string BankName { get; set; } = null!;
        public string BankAcNo { get; set; } = null!;
    }

    public partial class PpoArrearBillListResponseDTO : BaseDTO
    {
        public List<PpoArrearBillResponseDTO> PpoList { get; set; } = null!;
        public int PpoCount
        {
            get { return this.PpoList?.Count ?? 0; }
        }
    }
}
