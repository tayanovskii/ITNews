namespace ITNews.DTO.CommentDto
{
    public class CreateCommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public int NewsId { get; set; }
    }
}
