namespace SocialMedia_Web.Models
{
    public class UserForLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Token { get; set; }
        public DateTime? Expiration { get; set; }
    }
}
