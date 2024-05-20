namespace Clicktobuyservice.Models
{
    public class CommonResponse
    {
        public bool IsSuccess { get; set; }
        public string? Token { get; set; }
        public int ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }

        public string? Message { get; set; }
        public long UserId { get; set; }

        
    }
}
