namespace SocialMedia_Web.Models
{
    public class ArticleDetailDto
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public int UserId { get; set; }
        public string TopicTitle { get; set; }
        public string UserName { get; set; }
        public string? UserImage { get; set; }
        public string Content { get; set; }
        public string SharingDate { get; set; }
        public List<CommentDetail> CommentDetails { get; set; }
    }


}
