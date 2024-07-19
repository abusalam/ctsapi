namespace CTS_BE.DTOs
{
    public class SubDeatilsHeadWithToatlDto
    {
        public List<SubDeatilsHeadDto>? SubDeatils { get; set; }
        public decimal Total { get; set; }
    }
    public class SubDeatilsHeadDto
    {
        public string? SubDeatils { get; set; }
        public string? Description { get; set; }
        public decimal? Amount { get; set; }
        // public AllotmentDto? Allotments { get; set; }
    }
}
