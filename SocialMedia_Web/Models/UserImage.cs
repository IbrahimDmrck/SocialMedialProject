namespace SocialMedia_Web.Models
{
    public class UserImage
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public IFormFile ImagePath { get; set; }
        public DateTime Date { get; set; }
    }
}
