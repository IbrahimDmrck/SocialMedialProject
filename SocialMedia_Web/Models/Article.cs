namespace SocialMedia_Web.Models
{
    public class Article
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public int UserId { get; set; }
        public int? CommentId { get; set; }
        public string Content { get; set; }
        public DateTime SharingDate { get; set; }
    }
}
