using System.ComponentModel.DataAnnotations;

namespace CTS_BE.DTOs.PensionDTO
{
    public class DateOnlyDTO {
        [DataType(DataType.Date)]
        public DateOnly DateOnly { get; set; }
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
}