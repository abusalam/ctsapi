namespace CTS_BE.DTOs
{
    public class BillsListDTO
    {
        public long BillId { get; set; }
        public string? ReferenceNo { get; set; }
        public string? DdoCode { get; set; }
        public string? DdoDesignation { get; set; }
        public string? BillNo { get; set; }
        //public DateOnly? BillDate { get; set; }
        public string? BillDate { get; set; }

    }
    public class BillDetailsDetailsByRef
    {
        public string? DdoCode { get; set; }
        public string? ReferenceNo { get; set; }
        public string? BillNo { get; set; }
        public DateOnly? BillDate { get; set; }
        public int? BillType { get; set; }
        public string? BillSubType { get; set; }
        public string? DdoDesignation { get; set; }
        public string? PayeeDepartment { get; set; }
        public HOAChain HOAChain { get; set; }
        public List<SubDeatilsHeadDto>? SubDeatilsHead { get; set; }
        public double? TransferAmount { get; set; }
        public double? GrossAmount { get; set; }
        public double? NetAmount { get; set; }
        public double? AgBTAmount { get; set; }
        public double? TreasuryBTAmount { get; set; }
        public double? TotalBTAmount { get; set; }
        public string? SanctionNo { get; set; }
        public DateOnly? SanctionDate { get; set; }
    }
}
