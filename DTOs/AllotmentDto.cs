namespace CTS_BE.DTOs
{
    public class AllotmentDto
    {
        public HOAChain HOAChain { get; set; }
        public decimal? AllotmentAmount { get; set; }
        public double? PreviousBalance { get; set; }
        public decimal? AdjustedAmount { get; set; }
        public double? BalanceAmount { get; set; }
        public string? SubDetailHead { get; set; }
        public double? OverDrawalAmount { get; set; }
        public string? FinalProjectDetails { get; set; }
    }
}
