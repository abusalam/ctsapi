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
        public long TokenNumber { get; set; }
        public DateOnly TokenDate { get; set; }
        public string FinancialYear { get; set; }
        public string ReferenceNo { get; set; }
        public string CurrentStatus { get; set; }
        public string CurrentStatusSlug { get; set; }
        public string DdoCode { get; set; }
    }
    public class TokenCount
    {
        public int AllTokens { get; set; }
        public int BillCheckingPending { get; set; }
        public int ReturnMemoPending { get; set; }
    }
    public class TokenDetailsDto
    {
        public long TokenNumber { get; set; }
        public DateOnly TokenDate { get; set; }
        public string ReferenceNo { get; set; }
        public string Status { get; set; }
        public int? StatusId { get; set; }
    }
}
