using System.ComponentModel.DataAnnotations;

namespace CTS_BE.DTOs
{
    public class ChequeEntryDTO : IValidatableObject
    {
        public string TreasurieCode { get; set; }
        public string MicrCode { get; set; }
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
        public long? Id { get; set; }
        public string? TreasurieCode { get; set; }
        public string? MicrCode { get; set; }
        public string Series { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public int Quantity { get; set; }
        public short AvailableQuantity { get; set; }

    }
    public class ChequeSeriesDetailDTO
    {
        public long? Id { get; set; }
        public string? TreasurieCode { get; set; }
        public string? MicrCode { get; set; }
        public string? Series { get; set; }
        public int? Start { get; set; }
        public int? End { get; set; }
        public int? Quantity { get; set; }
        public int? AvailableQuantity { get; set; }
    }
    public class ChequeIndentListDTO
    {
        public long Id { get; set; }
        public int? IndentId { get; set; }
        public string? IndentDate { get; set; }
        public string? MemoNo { get; set; }
        public string? MemoDate { get; set; }
        public string? ChequeType { get; set; }
        public string? MicrCode { get; set; }
        public int? Quantity { get; set; }
        public string? Remarks { get; set; }
        public string? CurrentStatus { get; set; }
        public int? CurrentStatusId { get; set; }
    }
    public class IndentFrowardApproveRjectDTO
    {
        public long IndentId { get; set; }
    }
    public class InvoiceFrowardDTO
    {
        public long InvoiceId { get; set; }
    }
    public class ChequeIndentDTO
    {
        public long? IndentId { get; set; }
        public string IndentDate { get; set; }
        public string MemoNumber { get; set; }
        public string MemoDate { get; set; }
        public string Remarks { get; set; }
        public string? TreasurieCode { get; set; }
        public List<ChequeIndentDeatilsDTO> ChequeIndentDeatils { get; set; }
    }
    public class ChequeIndentDeatilsDTO
    {
        public long? IndentDeatilsId { get; set; }
        public short ChequeType { get; set; }
        public string MicrCode { get; set; }
        public int Quantity { get; set; }
    }
    public class ChequeInvoiceDTO
    {
        public long ChequeIndentId { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public List<ChequeInvoiceDeatilsDTO> ChequeInvoiceDeatils { get; set; }
    }
    public class ChequeInvoiceDeatilsDTO
    {
        // public long ChequeIndentDetailId { get; set; }
        public string MicrCode { get; set; }
        // public short Start { get; set; }
        // public short End { get; set; }
        public short Quantity { get; set; }
    }
    public class ChequeInvoiceListDTO
    {
        public long Id { get; set; }
        public string InvoiceDate { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
        public string InvoiceNumber { get; set; } = "";
        public string MemoNumber { get; set; } = "";
        public int? Quantity { get; set; }
        public string? CurrentStatus { get; set; }
        public int? CurrentStatusId { get; set; }
    }

    public class ChequeInvoiceDetailsByIdDTO
    {
        public long Id { get; set; }
        public int? Quantity { get; set; }
        public List<ChequeInvoiceSeriesDTO> ChequeInvoiceSeries { get; set; }
    }

    public class ChequeInvoiceSeriesDTO
    {
        public long InvoiceDeatilsId { get; set; }

        public string TreasuryCode { get; set; }
        public string MicrCode { get; set; }
        public short Quantity { get; set; }
        public string Series { get; set; }
        // public short AvailableQuantity { get; set; }
    }

        public class ChequeReceivedDTO
    {
        public long Id { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public int Quantity { get; set; }
        public int ReceivedUser { get; set; }
        public int InvoiceId { get; set; }
        public int ChequeEntryId { get; set; }
    }

}
