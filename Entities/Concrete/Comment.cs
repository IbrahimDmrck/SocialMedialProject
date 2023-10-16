using Core.Entities;

namespace Entities.Concrete
{
    public class Comment : IEntity
    {
        public int Id { get; set; }
        public int ContentId { get; set; }
        public int UserId { get; set; }
        public string CommentText { get; set; }
        public DateTime CommentDate { get; set; }
    }
}
