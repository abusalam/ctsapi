using System.ComponentModel.DataAnnotations;
using CTS_BE.DTOs.Validators;
using System.Dynamic;
using System.Reflection;
using CTS_BE.PensionEnum;

namespace CTS_BE.DTOs
{
    public class BaseDTO {
        public ExpandoObject? DataSource { get; set; }
    }

    public class DateOnlyDTO {
        [DataType(DataType.Date)]
        public DateOnly DateOnly { get; set; }
    }
    
    public class PensionStatusDTO : BaseDTO {
        [Required]
        [EnumDataType(typeof(PensionStatusFlag))]
        public PensionStatusFlag StatusFlag {get; set; }
        // public int StatusFlag {get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly StatusWef { get; set; }
    }

    public class PensionStatusEntryDTO : PensionStatusDTO {
        [Required]
        public int PpoId { get; set; }
    }
    
    public class ManualPpoReceiptEntryDTO : BaseDTO {

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
        [RegularExpression(@"[NRPO]", ErrorMessage = "{0} must be one of the following (N, R, P & O)")]
        public required char PpoType { get; set; }
    }


    public class ManualPpoReceiptResponseDTO : ManualPpoReceiptEntryDTO {
        public long Id { get; set; }
        
        [StringLength(13)]
        public string TreasuryReceiptNo { get; set; } = null!;
    }

    public class ListAllPpoReceiptsResponseDTO {
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

    public class PensionerEntryDTO : BaseDTO {
        [Required]
        [StringLength(100)]
        public string PpoNo { get; set; } = null!;

        [Required]
        [RegularExpression(@"[PFC]", ErrorMessage = "{0} must be one of the following (P, F & C)")]
        /// <value>Property <c>PpoType</c> Must be one of the following (P, F, C).</value>
        public char PpoType { get; set; }


        [Required]
        [RegularExpression(@"[ELUVNRPGJKHW]", ErrorMessage = "{0} must be one of the following (E, L, U, V, N, R, P, G, J, K, H & W)")]
        /// <value>Property <c>PpoSubType</c> Must be one of the following (E, L, U, V, N, R, P, G, J, K, H, W).</value>
        public char PpoSubType { get; set; }
        
        [Required]
        public long CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string PensionerName { get; set; } = null!;
        
        [RegularExpression(@"[MF]", ErrorMessage = "{0} must be one of the following (M - Male; F - Female;)")]
        /// <value>Property <c>Gender</c> Must be one of the following (M - Male; F - Female;).</value>
        public char? Gender { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [PastDateWithinYears(100)]
        public DateOnly DateOfBirth { get; set; }

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
        public long? CommutedPensionAmount { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? CommutedFromDate { get; set; }
        
        [DataType(DataType.Date)]
        public DateOnly? CommutedUptoDate { get; set; }

        [Required]
        public int EnhancePensionAmount { get; set; }
        
        [Required]
        public int ReducedPensionAmount { get; set; }

        /// <summary>
        /// Must be one of the following (H, M, O)
        /// </summary>
        [Required]
        [RegularExpression(@"[HMO]", ErrorMessage = "{0} must be one of the following (H, M & O)")]
        public char Religion { get; set; }
    }

    public class PensionerResponseDTO : PensionerEntryDTO {
        public long Id { get; set; }
        public int PpoId { get; set; }
        public PensionCategoryResponseDTO? Category { get; set; }
        public ManualPpoReceiptResponseDTO? Receipt { get; set; }
        public List<PensionerBankAcResponseDTO>? BankAccounts { get; set; }
    }

    public class PensionerListItemDTO : BaseDTO {
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

    public class PensionerBankAcEntryDTO : BaseDTO {

        [Required]
        [RegularExpression(@"[QB]", ErrorMessage = "{0} must be one of the following (Q, B)")]
        public char PayMode { get; set; }

        [Required]
        [StringLength(100)]
        public string AccountHolderName { get; set; } = null!;

        [Required]
        [StringLength(30)]
        public string? BankAcNo { get; set; }

        [Required]
        [StringLength(11)]
        public string? IfscCode { get; set; }

        [Required]
        public long? BankCode { get; set; }

        [Required]
        public long? BranchCode { get; set; }
    }

    public class PensionerBankAcResponseDTO : PensionerBankAcEntryDTO {
        public long Id { get; set; }
    }

    public partial class PensionPrimaryCategoryEntryDTO : BaseDTO {
        /// <summary>
        /// Head of Account: 2071 - 01 - 109 - 00 - 001 - V - 04 - 00
        /// </summary>
        [Required]
        [StringLength(50)]
        public string HoaId { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string PrimaryCategoryName { get; set; } = null!;
    }

    public partial class PensionPrimaryCategoryResponseDTO : PensionPrimaryCategoryEntryDTO {
        public long Id { get; set; }
    }

    public partial class PensionSubCategoryEntryDTO : BaseDTO {
        [Required]
        [StringLength(100)]
        public string SubCategoryName { get; set; } = null!;
    }

    public partial class PensionSubCategoryResponseDTO : PensionSubCategoryEntryDTO {
        public long Id { get; set; }
    }

    public partial class PensionCategoryEntryDTO : BaseDTO {
        [Required]
        public long PrimaryCategoryId { get; set; }
        
        [Required]
        public long SubCategoryId { get; set; }
    }

    public partial class PensionCategoryResponseDTO : PensionCategoryEntryDTO {
        public long Id { get; set; }
        public string CategoryName { get; set; } = null!;
        public PensionPrimaryCategoryResponseDTO PrimaryCategory { get; set; } = null!;
        public PensionSubCategoryResponseDTO SubCategory { get; set; } = null!;
        public List<ComponentRateResponseDTO>? ComponentRates { get; set; }
    }

    public class PensionCategoryListDTO : BaseDTO {
        public long Id { get; set; }
        public long PrimaryCategoryId { get; set; }
        public long SubCategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
    }

    public partial class PensionBreakupEntryDTO : BaseDTO {
        [Required]
        [StringLength(100)]
        public string ComponentName { get; set; } = null!;

        /// <summary>
        /// P - Payment; D - Deduction;
        /// </summary>
        [Required]
        [RegularExpression(@"[PD]", ErrorMessage = "{0} must be one of the following (P - Payment; D - Deduction)")]
        public char ComponentType { get; set; }

        /// <summary>
        /// Relief Allowed (true/false)
        /// </summary>
        [Required]
        public bool ReliefFlag { get; set; }
    }

    public partial class PensionBreakupResponseDTO : PensionBreakupEntryDTO {
        public long Id { get; set; }
    }

    public partial class ComponentRateEntryDTO : BaseDTO {
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
        [RegularExpression(@"[PA]", ErrorMessage = "{0} must be one of the following (P - Percentage; A - Amount;)")]
        public char RateType { get; set; }
    }

    public partial class ComponentRateResponseDTO : ComponentRateEntryDTO {
        public long Id { get; set; }
        public PensionBreakupResponseDTO? Breakup { get; set; }
    }

    public partial class InitiateFirstPensionBillDTO : BaseDTO {
        [Required]
        public virtual int PpoId { get; set; }

        [Required]
        [CurrentOrFutureDateUptoYears(1, ErrorMessage = "Date of bill should be within 1 years from today")]
        public virtual DateOnly ToDate { get; set; }
    }

    public partial class PensionerFirstBillResponseDTO : InitiateFirstPensionBillDTO {
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
        public override DateOnly ToDate { get {return this.BillGeneratedUptoDate;} }
        public DateOnly BillDate { get; set; }
        public string TreasuryVoucherNo { get; set; } = null!;
        public long TreasuryVoucherId { get; set; }
        public DateOnly TreasuryVoucherDate { get; set; }
        public long GrossAmount { get; set; }
        public long NetAmount { get; set; }
        public string PreparedBy { get; set; } = null!;
        public DateOnly PreparedOn { get; set; }
    }

    public partial class InitiateFirstPensionBillResponseDTO : PensionerFirstBillResponseDTO {
        public long PensionerId { get {return this.Pensioner?.Id ?? 0;} }
        public PensionerResponseDTO? Pensioner { get; set; } = null!;
    }

    public partial class PpoPaymentListItemDTO : BaseDTO {
        // public int PpoId { get; set; }
        // public long BillId { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public long BasicPensionAmount { get; set; }

        /// <summary>
        /// BaseAmount will be same as AmountPerMonth except in case of DA where BaseAmount will be BasicPensionAmount
        /// </summary>
        public long BaseAmount { get; set; }
        public long BreakupId { get; set; }
        public string ComponentName { get; set; } = null!;
        public char ComponentType { get; set; }
        public int AmountPerMonth { get; set; }
        public long RateId { get; set; }
        public char RateType { get; set; }
        public int RateAmount { get; set; }
        public int PeriodInMonths { get; set; }
        public int PeriodInDays { get; set; }
        public long DueAmount { get; set; }
        public long DrawnAmount { get; set; }
        public long NetAmount { get; set; }
    }

    public partial class PpoComponentRevisionEntryDTO : BaseDTO {

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

    public partial class PpoComponentRevisionResponseDTO : PpoComponentRevisionEntryDTO {
        public long Id { get; set; }

        /// <summary>
        /// To date (will be null for regular active bills)
        /// </summary>
        [DataType(DataType.Date)]
        public DateOnly? ToDate { get; set; }

        public ComponentRateResponseDTO? Rate { get; set; }
    }

    public partial class PpoComponentRevisionUpdateDTO : BaseDTO {

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

    public partial class PpoBillEntryDTO : BaseDTO {
        [Required]
        public int PpoId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [CurrentOrFutureDateUptoYears(1, ErrorMessage = "To date of bill should be current or future date upto 1 year from today")]
        public DateOnly ToDate { get; set; }

    }

    public partial class PpoBillResponseDTO : PpoBillEntryDTO {
        public long Id { get; set; }
        public long PensionerId { get; set; }
        public long BankAccountId { get; set; }
        [DataType(DataType.Date)]
        public DateOnly FromDate { get; set; }
        public char BillType { get; set; }
        public int BillNo { get; set; }
        public DateOnly BillDate { get; set; }
        public int GrossAmount { get; set; }
        public int ByTransferAmount { get; set; }
        public int NetAmount { get; set; }
        public virtual List<PpoBillBreakupEntryDTO> Breakups { get; set; } = null!;
        public long DrawnAmount { get; set; } = 0;
        public string? TreasuryVoucherNo { get; set; }
        public DateOnly? TreasuryVoucherDate { get; set; }
        public PensionerResponseDTO Pensioner { get; set; } = null!;
        public List<PpoBillBreakupResponseDTO> PpoBillBreakups { get; set; } = null!;
        public string PreparedBy { get; set; } = null!;
        public DateOnly PreparedOn { get; set; }
    }

    public partial class PpoBillBreakupEntryDTO : BaseDTO {
        // public long BillId { get; set; }
        [Required]
        public int PpoId { get; set; }
        // public long RateId { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        [PastDateWithinYears(100, ErrorMessage = "Date of bill should be within 100 years from today")]
        public DateOnly FromDate { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        public DateOnly ToDate { get; set; }

        [Required]
        public long BreakupAmount { get; set; }
        public long DueAmount { get {return this.BreakupAmount;} set {this.BreakupAmount = value;} }
        public long DrawnAmount { get; set; } = 0;
        public long NetAmount { get {return this.BreakupAmount - this.DrawnAmount;} }
    }

    public partial class PpoBillBreakupResponseDTO : PpoBillBreakupEntryDTO {
        public long Id { get; set; }
        public long RevisionId { get {return this.Revision?.Id ?? 0;} }
        public PpoComponentRevisionResponseDTO Revision { get; set; } = null!;
        public string ComponentName { get; set; } = null!;
        public char ComponentType { get; set; }
        public int AmountPerMonth { get; set; }
        public long BaseAmount { get; set; }
    }

    public partial class PpoComponentRevisionListEntryDTO : BaseDTO {
        public List<PpoComponentRevisionEntryDTO>? Revisions { get; set; }
    }

    public partial class PpoListResponseDTO : BaseDTO {
        public List<PensionerListItemDTO> PpoList { get; set; } = null!;
        public int PpoCount { get { return this.PpoList?.Count ?? 0;} }
    }

    public partial class PpoBillListResponseDTO : BaseDTO {
        public long Id { get; set; }
        
        [StringLength(50)]
        public string HoaId { get; set; } = null!;
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

    public partial class BillResponseDTO : BaseDTO {
        public long Id { get; set; }
        public string FinancialYear { get; set; } = null!;
        public string HoaId { get; set; } = null!;
        public int BillNo { get; set; }
        public DateOnly BillDate { get; set; }
        public string TreasuryVoucherNo { get; set; } = null!;
        public DateOnly TreasuryVoucherDate { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public List<PpoBillResponseDTO> PpoBills { get; set; } = null!;
        public long PpoBillCount { get { return this.PpoBills?.Count ?? 0;} }
        public int GrossAmount { get; set; }
        public int ByTransferAmount { get; set; }
        public int NetAmount { get; set; }
        public string PreparedBy { get; set; } = null!;
        public DateOnly PreparedOn { get; set; }
    }

    public partial class BillListResponseDTO : BaseDTO {
        public List<BillResponseDTO> Bills { get; set; } = null!;
        public long BillCount { get { return this.Bills?.Count ?? 0;} }
        public string PreparedBy { get; set; } = null!;
        public DateOnly PreparedOn { get; set; }
    }

    public partial class PpoBillSaveResponseDTO : BaseDTO {
        public long Id { get; set; }
        public int PpoId { get; set; }
        public DateOnly BillDate { get; set; }
        public char BillType { get; set; }
    }
}