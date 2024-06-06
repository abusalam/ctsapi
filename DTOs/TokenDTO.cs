namespace CTS_BE.DTOs
{
    public class TokenDTO
    {
        public long BillId { get; set; }
        public string PhysicalBillDate { get; set; }
        public string? Remarks { get; set; } = null;

    }
    public class TokenList
    {
        public long TokenId { get; set; }
        public long TokenNumberr { get; set; }
        public DateOnly TokenDate { get; set; }
        public string FinancialYear { get; set; }
        public string ReferenceNo { get; set; }
        public string CurrentStatus { get; set; }
        public string? CurrentStatusSlug { get; set; }
        public int? CurrentStatusId { get; set; }
        public string DdoCode { get; set; }
    }
    public class TokenListQueryParameters
    {
        public string ListType { get; set; }
    }
    public class TokenCount
    {
        public int NewBills { get; set; }
        public int AllTokens { get; set; }
        public int BillCheckingPending { get; set; }
        public int ReturnMemoPending { get; set; }
    }
    public class TokenDetailsDto
    {
        public long TokenId { get; set; }
        public long TokenNumber { get; set; }
        public DateOnly TokenDate { get; set; }
        public string ReferenceNo { get; set; }
        public long BillId { get; set; }
        public string Status { get; set; }
        public int? StatusId { get; set; }
    }
    public class TokenPrintDTO
    {
        public long TokenNumber { get; set; }
        public DateOnly TokenDate { get; set; }
        public string BillNo { get; set; }
        public DateOnly? BillDate { get; set; }
        public string DdoCode { get; set; }
        public string PayeeDept { get; set; }
        public HOAChain HOAChain { get; set; }
        public decimal? GrossAmount { get; set; }
        public decimal? NetAmount { get; set; }
    }
    public class GeneratedTokenDTO
    {
        public long TokenId { get; set; }
        public int TokenNumber { get; set; }
    }
}
