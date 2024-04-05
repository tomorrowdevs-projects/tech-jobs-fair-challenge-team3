namespace RubricaTelefonicaAziendale.Models
{
    public class ListRequest
    {
        public Int32 Page { get; set; } = 1;
        public Int32 EntriesPerPage { get; set; } = 10;
        public List<SortParams> Sorting { get; set; } = [];
    }

    public class SortParams
    {
        public String Column { get; set; } = "Id";
        public String Direction { get; set; } = "ASC";
    }
}