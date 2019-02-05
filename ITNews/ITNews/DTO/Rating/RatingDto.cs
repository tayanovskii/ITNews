namespace ITNews.DTO.Rating
{
    public class RatingDto
    {
        public int Id { get; set; }
        public short Value { get; set; }
        public int NewsId { get; set; }
        public string UserId { get; set; }
    }
}
