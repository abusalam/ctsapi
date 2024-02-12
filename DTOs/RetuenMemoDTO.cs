namespace CTS_BE.DTOs
{
    public class ReturnMemoTokenDetailsDTO
    {
        public long TokenId { get; set; }
        public string ReferenceNo { get; set; }
    }
    public class ReturnMemoCountDTO
    {
        public int GeneratedReturnMemo { get; set; }
        public int AwatingReturnMemo { get; set; }
    }
    public class ReturnMemoBillDetailsDTO
    {
        public long? TokenId { get; set; }
        public long? TokenNumber { get; set; }
        public DateOnly? TokenDate { get; set; }
        public string? BillNo { get; set; }
        public DateOnly? BillDate { get; set; }
        public string? DdoCode { get; set; }
        public HOAChain? HOAChain  {get;set;}
        public double? GrossAmount { get; set; }
        public double? NetAmount { get; set; }
    }
}
