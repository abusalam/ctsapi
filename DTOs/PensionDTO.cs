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
        [PastDateWithinYears(100)]
        [FutureDateUptoYears(10)]
        public DateOnly DateOnly { get; set; }
    }
    
    public class PensionStatusDTO : BaseDTO {
        [Required]
        // [EnumDataType(typeof(PensionStatusFlag))]
        // public PensionStatusFlag StatusFlag {get; set; }
        public int StatusFlag {get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly StatusWef { get; set; }
    }

    public class PensionStatusEntryDTO : PensionStatusDTO {
        [Required]
        public int PpoId { get; set; }
    }
    
    public class ManualPpoReceiptEntryDTO {

        [StringLength(100)]
        public required string PpoNo { get; set; } = null!;

        [StringLength(100)]
        public required string PensionerName { get; set; }

        [DataType(DataType.Date)]
        [PastDateWithinYears(100)]
        public required DateOnly DateOfCommencement { get; set; }

        [StringLength(10)]
        [RegularExpression(@"^[6-9]\d{9}$")]
        public string? MobileNumber { get; set; }

        [DataType(DataType.Date)]
        [PastDateWithinYears(10)]
        public required DateOnly ReceiptDate { get; set; }

        [RegularExpression(@"[ADO]", ErrorMessage = "{0} must be one of the following (A, D & O)")]
        public required char PsaCode { get; set; }

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
    }

    public class PensionerEntryDTO {
        public long ReceiptId { get; set; }

        [StringLength(100)]
        public string PpoNo { get; set; } = null!;

        [RegularExpression(@"[PFC]", ErrorMessage = "{0} must be one of the following (P, F & C)")]
        public char PpoType { get; set; }

        [RegularExpression(@"[ELUVNRPGJKHW]", ErrorMessage = "{0} must be one of the following (E, L, U, V, N, R, P, G, J, K, H & W)")]
        public char PpoSubType { get; set; }
        public long CategoryId { get; set; }

        [StringLength(100)]
        public string PensionerName { get; set; } = null!;
        
        [RegularExpression(@"[MF]", ErrorMessage = "{0} must be one of the following (M - Male; F - Female;)")]
        public char? Gender { get; set; }

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

        [DataType(DataType.Date)]
        [PastDateWithinYears(100)]
        public DateOnly DateOfRetirement { get; set; }

        [DataType(DataType.Date)]
        [PastDateWithinYears(100)]
        public DateOnly DateOfCommencement { get; set; }
        public int BasicPensionAmount { get; set; }
        public int CommutedPensionAmount { get; set; }
        public int EnhancePensionAmount { get; set; }
        public int ReducedPensionAmount { get; set; }

        [RegularExpression(@"[HMO]", ErrorMessage = "{0} must be one of the following (H, M & O)")]
        public char Religion { get; set; }
    }

    public class PensionerResponseDTO : PensionerEntryDTO {
        public long Id { get; set; }
        public int PpoId { get; set; }
        public PensionCategoryResponseDTO? Category { get; set; }
        public ManualPpoReceiptResponseDTO? Receipt { get; set; }
        public List<PensionerBankAcDTO>? BankAccounts { get; set; }
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

    public class PensionerBankAcDTO : BaseDTO {

        [StringLength(100)]
        public string AccountHolderName { get; set; } = null!;

        [StringLength(30)]
        public string? BankAcNo { get; set; }

        [StringLength(11)]
        public string? IfscCode { get; set; }

        [Required]
        public long? BankCode { get; set; }

        [Required]
        public long? BranchCode { get; set; }
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
        public long PrimaryCategoryId { get; set; }
        public long SubCategoryId { get; set; }
    }

    public partial class PensionCategoryResponseDTO : PensionCategoryEntryDTO {
        public long Id { get; set; }

        [StringLength(100)]
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
        [StringLength(100)]
        public string ComponentName { get; set; } = null!;

        /// <summary>
        /// P - Payment; D - Deduction;
        /// </summary>
        [RegularExpression(@"[PD]", ErrorMessage = "{0} must be one of the following (P - Payment; D - Deduction)")]
        public char ComponentType { get; set; }

        /// <summary>
        /// Relief Allowed (true/false)
        /// </summary>
        public bool ReliefFlag { get; set; }
    }

    public partial class PensionBreakupResponseDTO : PensionBreakupEntryDTO {
        public long Id { get; set; }
    }

    public partial class ComponentRateEntryDTO : BaseDTO {
        public long CategoryId { get; set; }
        public long BreakupId { get; set; }
        public PensionBreakupResponseDTO? Breakup { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly EffectiveFromDate { get; set; }
        public int RateAmount { get; set; }

        /// <summary>
        /// P - Percentage; A - Amount;
        /// </summary>
        [RegularExpression(@"[PA]", ErrorMessage = "{0} must be one of the following (P - Percentage; A - Amount;)")]
        public char RateType { get; set; }
    }

    public partial class ComponentRateResponseDTO : ComponentRateEntryDTO {
        public long Id { get; set; }
    }

    public partial class InitiateFirstPensionBillDTO : BaseDTO {
        [Required]
        public int PpoId { get; set; }
        public DateOnly ToDate { get; set; }
    }

    public partial class PensionerFirstBillResponseDTO : BaseDTO {

        [Required]
        public required PensionerListItemDTO Pensioner { get; set; }

        [Required]
        public required PensionerBankAcDTO BankAccount { get; set; }

        [Required]
        public PensionCategoryResponseDTO PensionCategory { get; set; } = null!;

        [Required]
        public ICollection<PpoPaymentListItemDTO>? PensionerPayments { get; set; }
    }

    public partial class InitiateFirstPensionBillResponseDTO : PensionerFirstBillResponseDTO {

        [Required]
        [DataType(DataType.Date)]
        public DateOnly BillGeneratedUptoDate { get; set; }
        public long BillId { get; set; }
        public DateOnly BillDate { get; set; }
        public string TreasuryVoucherNo { get; set; } = null!;
        public long TreasuryVoucherId { get; set; }
        public DateOnly TreasuryVoucherDate { get; set; }
        public long GrossAmount { get; set; }
        public long NetAmount { get; set; }
    }

    public partial class PpoPaymentListItemDTO : BaseDTO {
        // public int PpoId { get; set; }
        // public long BillId { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public long BasicPensionAmount { get; set; }
        public long BreakupId { get; set; }
        public string ComponentName { get; set; } = null!;
        public char ComponentType { get; set; }
        public long BreakupAmount { get; set; }
        public long RateId { get; set; }
        public char RateType { get; set; }
        public int RateAmount { get; set; }
        public int PeriodInMonths { get; set; }
        public long DueAmount { get; set; }
        public long DrawnAmount { get; set; }
        public long NetAmount { get; set; }
    }

    public partial class PpoComponentRateEntryDTO : BaseDTO {
        [Required]        
        public int PpoId { get; set; }
        [Required]
        public long BreakupId { get; set; }

        /// <summary>
        /// From date is the Date of Commencement of pension of the pensioner
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateOnly FromDate { get; set; }

        /// <summary>
        /// To date (will be null for regular active bills)
        /// </summary>
        [DataType(DataType.Date)]
        public DateOnly? ToDate { get; set; }

        /// <summary>
        /// Amount per month is the actual amount paid for the mentioned period
        /// </summary>
        [Required]
        public int AmountPerMonth { get; set; }
    }

    public partial class PpoComponentRateResponseDTO : PpoComponentRateEntryDTO {
        public long Id { get; set; }
    }
}