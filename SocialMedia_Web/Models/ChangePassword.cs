namespace SocialMedia_Web.Models
{
    public class ChangePassword
    {
        public string Email { get; set; }
        public string? OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
