namespace SocialMedia_Web.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public int? ImageId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string ImagePath { get; set; }
    }
}
