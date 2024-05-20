namespace Clicktobuyservice.Models
{
    public class LoginResponse
    {
        public bool IsSuccess { get; set; }
        public string? Token { get; set; }
        public int ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public long UserId { get; set; }

        
    }
}
