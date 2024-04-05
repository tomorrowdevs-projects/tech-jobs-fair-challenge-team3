namespace RubricaTelefonicaAziendale.Dtos
{
    public class ListDto<T>
    {
        public Int32 RecordsFiltered { get; set; } 
        public Int32 RecordsTotal { get; set; }
        public List<T> Data { get; set; } = [];
    }
}