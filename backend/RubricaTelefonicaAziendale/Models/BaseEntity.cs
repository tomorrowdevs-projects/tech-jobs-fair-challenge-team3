namespace RubricaTelefonicaAziendale.Models
{
    public class BaseEntity
    {
        public Int32 Id { get; set; }
        public DateTime? InsertTimestamp { get; set; }
        public String? InsertUser { get; set; }
        public DateTime? UpdateTimestamp { get; set; }
        public String? UpdateUser { get; set; }
    }
}