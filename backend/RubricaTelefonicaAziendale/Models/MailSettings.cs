namespace RubricaTelefonicaAziendale.Models
{
    public partial class MailSettings
    {
        public String? ServerAddress { get; set; }
        public Int32? ServerPort { get; set; }
        public String? Username { get; set; }
        public String? Password { get; set; }
        public String? SenderMailAddress { get; set; }
        public String? SenderDisplayName { get; set; }
    }
}