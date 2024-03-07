namespace SocialMedia_Web.Models
{
    public class CommentDetail
    {
        public int? Id { get; set; }
        public int ArticleId { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Image { get; set; }
        public string CommentText { get; set; }
        public DateTime CommentDate { get; set; }
        public bool Status { get; set; }
    }
}
