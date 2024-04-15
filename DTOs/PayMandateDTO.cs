namespace CTS_BE.DTOs
{
    public class PayMandateShortListDTO
    {
        public long TokenId { get; set; }
        public DateOnly? TokenDate { get; set; }
        public string? BillNo { get; set; }    
        public DateOnly? BillDate { get; set; }
        public string? TRFormats { get; set; }
        public string? BillTypes { get; set; }
        public string? BillModule { get; set; }
        public string? BillPeriod { get; set; }
        public short? NoOfBeneficiarie { get; set; }
        public decimal? NeAmount { get; set; }
        public double? ECSAmount { get; set; }
        public double? ChequeAmount { get; set; }
        public string? DetailHead { get; set; }
        public HOAChain? HeadOfAccounts { get; set; }
        public string? DDOCode { get; set; } 
    }
    public class CreateShrtListDTO
    {
        public long TokenId {get; set; } 
        public string PaymentDate { get; set; }
    }
    public class NewShortlistDTO
    {
        public long TokenId { get; set; }
        public string PaymentDate { get; set; }
    }
}
