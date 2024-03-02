namespace CTS_BE.DTOs
{
    public class DynamicListQueryParameters
    {
        public string? ListType { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 0;
        public List<FilterParameter>? filterParameters { get; set; }
        public SortParameter? sortParameters { get; set; }
    }    
    public class FilterParameter
    {
        public string Field { get; set; }
        public string Value { get; set; }
        public string Operator { get; set; }
    }
    public class SortParameter
    {
        public string Field { get; set; } = "";
        public string Order { get; set; } = "ASC";
    }
    public class DynamicListResult<T>
    {
        public List<ListHeader> ListHeaders { get; set; }
        public T Data { get; set; }
        public int DataCount { get; set; }
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
