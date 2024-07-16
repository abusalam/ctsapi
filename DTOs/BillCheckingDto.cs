namespace CTS_BE.DTOs
{
    public class BillCheckingBillDetailsDto
    {
        public TokenDetailsDto TokenDetails { get; set; }
        public BillDetailsDetailsByRef BillDetailsDetails { get; set; }
    }
    public class BillCheckingDto
    {
        public long TokenId { get; set; }
        public string ReferenceNo { get; set; }
        public BillObjections? BillObjections { get; set; }
        public List<OverruledObjection>? OverruledObjections { get; set; }
    }
    public class BillObjections
    {
        public List<SelectedObjection>? GlobalObjections { get; set; }
        public List<SelectedObjection>? LocalObjections { get; set; }
    }
    public class SelectedObjection
    {
        public int? Id { get; set; }
        public string? Description { get; set; }
        public string? Remark { get; set; }
    }
    public class OverruledObjection
    {
        public int? TokenObjectionId { get; set; }
        public string? Remark { get; set; }
    }
    public class BIllInfoDTO
    {
        public string Target { get; set; }
    }
    public class ECSNEFT
    {
        public string BillNo { get; set; }
        public string BillDate { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal ChequeAmount { get; set; }
        public string PayMode { get; set; }
        public int NoOfBeneficiarys { get; set; }
        public List<BeneficiaryDetailsDTO> Beneficiarys { get; set; }
    }
    public class AllotmentDTO
    {
        public long AllotmentId { get; set; }
        public HOAChain HOA { get; set; }
        public decimal CeilingAmount { get; set; }
        public decimal ActualBalanceAmount { get; set; }
        public decimal BookedAmount { get; set; }
    }
    public class ByTransferDetislDTO
    {
        public decimal? BillBtAmount { get; set; }
        public decimal? AvailableBtAmount { get; set; }
        public decimal? BillNetAmount { get; set; }
        public decimal? TotalByTransfersAmount { get; set; }
        public List<ByTransferDTO>? ByTransfers { get; set; }
    }
    public class chequeDetailsDTO
    {
         public string? BillNo { get; set; }
        public string? BillDate { get; set; }
        public decimal? GrossAmount { get; set; }
        public decimal? NetAmount { get; set; }
        public decimal? ChequeAmount { get; set; }
        public string? PayMode { get; set; }
        // public decimal? TotalBeneficiaryAmount  { get; set; }
        public List<ChequeListDTOs> ChequeDetails { get; set; }
    }
}
