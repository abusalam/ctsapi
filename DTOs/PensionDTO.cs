using System.ComponentModel.DataAnnotations;
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
        public required DateOnly DateOfCommencement { get; set; }

        [StringLength(10)]
        [RegularExpression(@"^[6-9]\d{9}$")]
        public string? MobileNumber { get; set; }

        [DataType(DataType.Date)]
        public required DateOnly ReceiptDate { get; set; }

        [RegularExpression(@"[ADO]", ErrorMessage = "{0} must be one of the following (A, D & O)")]
        public required char PsaCode { get; set; }

        [RegularExpression(@"[NRPO]", ErrorMessage = "{0} must be one of the following (N, R, P & O)")]
        public required char PpoType { get; set; }
    }


    public class ManualPpoReceiptResponseDTO : ManualPpoReceiptEntryDTO {

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
        public DateOnly DateOfBirth { get; set; }

        [StringLength(10)]
        [RegularExpression(@"^[6-9]\d{9}$")]
        public string? MobileNumber { get; set; }
        public string? EmailId { get; set; }
        public string? PensionerAddress { get; set; }
        public string? IdentificationMark { get; set; }
        public string? PanNo { get; set; }
        public string? AadhaarNo { get; set; }

        [DataType(DataType.Date)]
        public DateOnly DateOfRetirement { get; set; }

        [DataType(DataType.Date)]
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
        public DateOnly DateOfBirth { get; set; }

        [DataType(DataType.Date)]
        public DateOnly DateOfRetirement { get; set; }
        
        [DataType(DataType.Date)]
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

        [StringLength(100)]
        public string? BankName { get; set; }

        [StringLength(100)]
        public string? BranchName { get; set; }
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

    public partial class PensionRatesEntryDTO : BaseDTO {
        public long CategoryId { get; set; }
        public long BreakupId { get; set; }

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

    public partial class PensionRatesResponseDTO : PensionRatesEntryDTO {
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
    }

    public partial class InitiateFirstPensionBillResponseDTO : PensionerFirstBillResponseDTO {

        [Required]
        [DataType(DataType.Date)]
        public DateOnly BillGeneratedUptoDate { get; set; }
        public ICollection<PensionRatesResponseDTO>? Rates { get; set; }
    }
}