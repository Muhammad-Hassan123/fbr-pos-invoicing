namespace fbr_full.Server.Models
{
    public class FbrApiLog
    {
        public int Id { get; set; }
        public string Endpoint { get; set; } = string.Empty;
        public string? RequestData { get; set; }
        public string? ResponseData { get; set; }
        public string Status { get; set; } = string.Empty; // Success, Failed, Pending
        public string? ErrorMessage { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public TimeSpan? ResponseTime { get; set; }
    }
} 