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
        public List<SelectedObjection> ?LocalObjections { get; set; }
    }
    public class SelectedObjection
    {
        public int? Id { get; set; }
        public string? Description { get; set; }
        public string? Remark { get; set; }
    }
    public class OverruledObjection
    {
        public int ?TokenObjectionId { get; set; }
        public string ?Remark { get; set; }
    }
}
