using System.ComponentModel.DataAnnotations;

namespace CTS_BE.DTOs.PensionDTO
{
    public class ManualPpoReceiptDTO {

        [StringLength(100)]
        public string PpoNo { get; set; } = null!;

        [StringLength(100)]
        public string? PensionerName { get; set; }

        [DataType(DataType.Date)]
        public DateOnly DateOfCommencement { get; set; }

        [StringLength(10)]
        [RegularExpression(@"^[6-9]\d{9}$")]
        public string? MobileNumber { get; set; }

        [DataType(DataType.Date)]
        public DateOnly ReceiptDate { get; set; }

        [RegularExpression(@"[ADO]", ErrorMessage = "{0} must be one of the following (A, D & O)")]
        public char PsaCode { get; set; }

        [RegularExpression(@"[NRPO]", ErrorMessage = "{0} must be one of the following (N, R, P & O)")]
        public char PpoType { get; set; }

    }
}