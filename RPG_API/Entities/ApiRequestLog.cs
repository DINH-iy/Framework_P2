using System.ComponentModel.DataAnnotations;

namespace Game.Domain.Entities;

public class ApiRequestLog
{
    [Key]
    public long Id { get; set; }
    public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
    public required string Method { get; set; }
    public required string Path { get; set; }
    public int StatusCode { get; set; }
    public long DurationMilliseconds { get; set; }
    public string? ExceptionMessage { get; set; }
}
