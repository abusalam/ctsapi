using System.ComponentModel.DataAnnotations;
using CTS_BE.PensionEnum;

namespace CTS_BE.DTOs
{
    public class DateOnlyDTO {
        [DataType(DataType.Date)]
        public DateOnly DateOnly { get; set; }
    }
    
    public class PensionStatusDTO {
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
        [StringLength(100)]
        public string PpoNo { get; set; } = null!;

        [RegularExpression(@"[PFC]", ErrorMessage = "{0} must be one of the following (P, F & C)")]
        public char PpoType { get; set; }
        
        [RegularExpression(@"[ADO]", ErrorMessage = "{0} must be one of the following (A, D & O)")]
        public char PsaType { get; set; }

        [RegularExpression(@"[ELUVNRPGJKHW]", ErrorMessage = "{0} must be one of the following (E, L, U, V, N, R, P, G, J, K, H & W)")]
        public char PpoSubType { get; set; }
        public char PpoCategory { get; set; }
        public char PpoSubCategory { get; set; }

        [StringLength(100)]
        public string PensionerName { get; set; } = null!;

        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        [RegularExpression(@"[MFO]", ErrorMessage = "{0} must be one of the following (M, F & O)")]
        public char? Gender { get; set; }

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
        public int PpoId { get; set; }
    }
}