namespace RubricaTelefonicaAziendale.Entities;

public partial class Logs
{
    public long Id { get; set; }

    public string UsersId { get; set; } = null!;

    public string? IpAddress { get; set; }

    public string? Endpoint { get; set; }

    public string? Message { get; set; }

    public string? RawData { get; set; }

    public DateTime Timestamp { get; set; }
}
