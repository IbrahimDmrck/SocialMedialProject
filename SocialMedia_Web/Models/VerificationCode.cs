namespace SocialMedia_Web.Models
{
    public class VerificationCode
    {
        public int UserId { get; set; }
        public string? Email { get; set; }
        public string? Code { get; set; }
    }
}
