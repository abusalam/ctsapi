using System.ComponentModel.DataAnnotations;

namespace CTS_BE.DTOs
{
    public class ChequeEntryDTO : IValidatableObject
    {
        public string Series { get; set; }

        [Range(1, short.MaxValue, ErrorMessage = "Start must be a positive value")]
        public short Start { get; set; }

        [Range(1, short.MaxValue, ErrorMessage = "End must be a positive value")]
        public short End { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Start >= End)
            {
                yield return new ValidationResult("Start must be less than End", new[] { nameof(Start), nameof(End) });
            }
        }
    }
    public class ChequeListDTO
    {
        public string Series { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public int Quantity { get; set; }
    }
    public class ChequeIndentDTO
    {
        public string IndentDate { get; set; }
        public string MemoNumber { get; set; }
        public string MemoDate { get; set; }
        public string Remarks { get; set; }
        public List<ChequeIndentDeatilsDTO> ChequeIndentDeatils { get; set; }
    }
    public class ChequeIndentDeatilsDTO
    {
        public short ChequeType { get; set; } 
        public string MicrCode { get; set; }    
        public short Quantity { get; set; }
    }
    public class ChequeInvoiceDTO
    {
        public long ChequeIndentId { get; set; }
        public string InvoiceDate{ get; set; }
        public string InvoiceNumber{ get; set; }
        public List<ChequeInvoiceDeatilsDTO> ChequeInvoiceDeatils { get; set; }
    }
    public class ChequeInvoiceDeatilsDTO
    {
        public long ChequeIndentDetailId { get; set; }
        public long ChequeEntryId { get; set; }
        public short Start { get; set; }
        public string End { get; set; }
        public short Quantity { get; set; }
    }
}
