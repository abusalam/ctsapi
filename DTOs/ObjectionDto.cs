namespace CTS_BE.DTOs
{
    public class ObjectionDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string ObjectionType { get; set; }
    }
    public class NewObjectionDto
    {
        public string description { get; set; }
    }
    public class TokenWithObjectionDto
    {
        public long Id { get; set; }
        public long ToeknId { get; set; }
        public string ObjectionDescription { get; set; }
        public int? ObjectionId { get; set; }
        public long? ObjectionBy {get; set; }
        public string ObjectionType { get; set; }
        public string ObjectionRemark { get; set; }
        public bool? IsOverruled { get; set; }
        public string? OverruledBy { get; set; }
    }
}
