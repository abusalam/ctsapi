namespace CTS_BE.DTOs
{
    public class FilterParameter
    {
        public string? Field { get; set; }
        public string? Value { get; set; }
        public string? Operator { get; set; }
    }

    public class SortParameter
    {
        public string? Field { get; set; } = "";
        public string? Order { get; set; } = "ASC";
    }

    public class ListHeader
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public string FieldName { get; set; }
        public string FilterField { get; set; }
        public List<FilterEnum>? FilterEnums { get; set; }
        public bool IsSortable { get; set; }
        public bool IsFilterable { get; set; }
        public string? ObjectTypeValueField { get; set; }
    }

    public class FilterEnum
    {
        public int Value { get; set; }
        public string Label { get; set; }
        public string? StyleClass { get; set; }
    }
}
