namespace CTS_BE.DTOs
{
    public class SubDeatilsHeadDto
    {
        public string? SubDeatils { get; set; }
        public string? Description { get; set; }
        public decimal? Amount { get; set; }
        public AllotmentDto? Allotments { get; set; }
    }
}
